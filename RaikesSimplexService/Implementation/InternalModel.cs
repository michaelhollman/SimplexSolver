using RaikesSimplexService.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using Matrix = MathNet.Numerics.LinearAlgebra.Generic.Matrix<double>;
using Vector = MathNet.Numerics.LinearAlgebra.Generic.Vector<double>;

namespace RaikesSimplexService.HollmanJagerJohnson
{
    internal class InternalModel : Model
    {
        public List<PVector> PColumns { get; set; }
        public DenseVector XColumn { get; set; }
        public bool IsStandardized { get; set; }

        private bool? _isTwoPhase;
        public bool IsTwoPhase
        {
            get
            {
                if (!_isTwoPhase.HasValue)
                    _isTwoPhase = Constraints.Any(c => c.Relationship != Relationship.LessThanOrEquals);
                return _isTwoPhase.Value;
            }
            set { _isTwoPhase = value; }
        }

        public InternalModel()
        {
            PColumns = new List<PVector>();
            IsStandardized = false;
        }

        public InternalSolution Solve()
        {
            // first thing's first!
            if (!IsStandardized)
            {
                try
                {
                    Standardize();
                }
                catch
                {
                    var effff = SolutionHelper.Infeasible();
                    return new InternalSolution(effff, this);
                }
            }

            // Loop over finding new basic variables until terminating conditions found
            for (var badUniqueVariableName = 0; badUniqueVariableName < 300; badUniqueVariableName++)
            {
                // create matrix of basics
                var basicInverse = DenseMatrix.OfColumnVectors(PColumns.Where(p => p.IsBasic).ToArray()).Inverse();

                // compute product of inverse and RHS values
                var xPrime = DenseVector.OfEnumerable((basicInverse * (XColumn.ToColumnMatrix())).ToColumnWiseArray().Select(x => Math.Round(x, 6)));


                var pPrimes = new List<PVectorPrime>();
                var zCoefs = DenseVector.OfEnumerable(PColumns.Where(p => p.IsBasic).Select(p => p.ZValue));
                foreach (var col in PColumns.Where(p => !p.IsBasic))
                {
                    //was getting some weird precision issues... so chop 'em short
                    var pPrime = DenseVector.OfEnumerable((basicInverse * col.ToColumnMatrix()).ToColumnWiseArray().Select(x => Math.Round(x, 6)));
                    var cPrime = Math.Round(col.ZValue - (zCoefs * pPrime), 6);

                    pPrimes.Add(new PVectorPrime(col, pPrime, cPrime));
                }

                //var anyNegative = cPrimes.Any(x => x < 0);
                var anyNegative = pPrimes.Any(x => Math.Round(x.CPrime, 2) < 0);
                if (anyNegative)
                {
                    PVector entering, exiting = null;


                    // determine entering basic variable (using Bland's method. inefficient, but reliable)
                    var tempPrime = pPrimes.First(p => Math.Round(p.CPrime, 4) < 0);
                    entering = tempPrime.PColumn;

                    // compute ratio of x prime to p prime assosiated with new basic variable
                    var ratios = (xPrime / tempPrime.PColumnPrime).Select(x => Math.Round(x, 6)).ToList();

                    // make sure there are positives, if not, then the entire problem unbounded
                    var anyPositive = ratios.Any(x => x > 0 && !double.IsInfinity(x));
                    if (anyPositive)
                    {
                        var min = ratios.Min(x => x > 0 ? x : double.PositiveInfinity);

                        var minIndexes = Enumerable.Range(0, ratios.Count).Where(i => ratios[i] == min).ToArray();
                        if (minIndexes.Length > 1)
                        {
                            var potentialExits = PColumns.Where(p => p.IsBasic).Where((x, i) => minIndexes.Contains(i));
                            var artificalCount = PColumns.Count(p => p.IsArtificial);

                            exiting = potentialExits.OrderByDescending(p => PColumns.Skip(PColumns.Count - artificalCount).Take(artificalCount - 1).Contains(p)).First();
                        }
                        else
                        {
                            exiting = PColumns.Where(p => p.IsBasic).ElementAt(minIndexes[0]);
                        }
                    }
                    else
                    {

                        var anyPositiveOrZeros = ratios.Any(x => x >= 0 && !double.IsInfinity(x));
                        if (anyPositiveOrZeros)
                        {
                            var min = ratios.Min(x => x >= 0 ? x : double.PositiveInfinity);

                            var minIndexes = Enumerable.Range(0, ratios.Count).Where(i => ratios[i] == min).ToArray();
                            if (minIndexes.Length > 1)
                            {
                                var potentialExits = PColumns.Where(p => p.IsBasic).Where((x, i) => minIndexes.Contains(i));
                                var artificalCount = PColumns.Count(p => p.IsArtificial);

                                exiting = potentialExits.OrderByDescending(p => PColumns.Skip(PColumns.Count - artificalCount).Take(artificalCount - 1).Contains(p)).First();
                            }
                            else
                            {
                                exiting = PColumns.Where(p => p.IsBasic).ElementAt(minIndexes[0]);
                            }
                        }
                        else
                        {
                            return new InternalSolution(SolutionHelper.Unbounded());
                        }
                    }

                    // actually change the basic-ness
                    entering.IsBasic = true;
                    exiting.IsBasic = false;
                }
                // infeasible
                else if (PColumns.Any(p => p.IsArtificial && p.IsBasic))
                {
                    return new InternalSolution(SolutionHelper.Infeasible(), this);
                }
                // optimal
                else
                {
                    var targetNumDecisions = Goal.Coefficients.Length;

                    var decisions = new List<double>();
                    var xIndex = 0;
                    foreach (var p in PColumns.Where(x => x.IsDecision))
                    {
                        if (p.IsBasic)
                        {
                            decisions.Add(xPrime[xIndex]);
                            xIndex++;
                        }
                        else
                        {
                            decisions.Add(0);
                        }
                    }
                    var value = decisions.Select((x, i) => x * Goal.Coefficients[i]).Sum();
                    value += Goal.ConstantTerm;

                    var allPPrimes = new List<PVectorPrime>();
                    foreach (var col in PColumns)
                    {
                        //was getting some weird precision issues... so chop 'em short
                        var pPrime = DenseVector.OfEnumerable((basicInverse * col.ToColumnMatrix()).ToColumnWiseArray());
                        var cPrime = Math.Round(col.ZValue - (zCoefs * pPrime), 6);

                        allPPrimes.Add(new PVectorPrime(col, pPrime, cPrime));
                    }
                    var infiniteSols = pPrimes.Count(p => Math.Round(p.CPrime, 4) == 0) >= Goal.Coefficients.Length;

                    var sol = SolutionHelper.Optimal(decisions.ToArray(), value, infiniteSols);
                    return new InternalSolution(sol, this);
                }
            }
            var uhoh = SolutionHelper.TimedOut();
            return new InternalSolution(uhoh, this);
        }

        private void Standardize()
        {
            // minimize -> maximize
            var stdGoalCoefs = (double[])Goal.Coefficients.Clone();
            if (GoalKind == GoalKind.Maximize)
            {
                stdGoalCoefs = stdGoalCoefs.Select(x => x *= -1).ToArray();
            }

            // add goal as constraint if two-phase
            if (IsTwoPhase)
            {
                Constraints.Add(new LinearConstraint()
                {
                    Coefficients = stdGoalCoefs,
                    Relationship = Relationship.Equals,
                    Value = 0
                });
            }

            // determine number of coefficients
            var pCount = Constraints[0].Coefficients.Length;
            if (!Constraints.TrueForAll(c => c.Coefficients.Length == pCount))
            {
                throw new Exception("You dirty wanker, all your linear Constraints need to have the same number of coeffecients!");
            }
            if (Goal.Coefficients.Length != pCount)
            {
                throw new Exception("You stinky twat, you need to have the same number of coefficients in your goal as you do in your linear Constraints!");
            }

            // create list of slack/surplus/artifical variable columns
            var sColumns = new List<PVector>();
            var aColumns = new List<PVector>();

            // add vectors of constraint coefficients, marked as non-basic and decision
            for (var i = 0; i < pCount; i++)
            {
                PColumns.Add(new PVector(DenseVector.OfEnumerable(Constraints.Select(c => c.Coefficients[i]).ToArray()), false, true, false));
            }

            // create surplus/slack/artificial variables for each constraint and determine indexes of basic
            for (int i = 0; i < Constraints.Count; i++)
            {
                var rel = Constraints[i].Relationship;

                // create surplus/slack variable column for <= and => relationships
                // will be basic if <=
                if (rel != Relationship.Equals)
                {
                    var vect = DenseVector.Create(Constraints.Count, x => x = 0);
                    vect[i] = rel == Relationship.LessThanOrEquals ? 1 : -1;
                    sColumns.Add(new PVector(vect, rel == Relationship.LessThanOrEquals, false, false));
                }

                // add artifical variable column for == and => relationships
                // always basic
                if (rel != Relationship.LessThanOrEquals)
                {
                    var vect = DenseVector.Create(Constraints.Count, x => x = 0);
                    vect[i] = 1;
                    aColumns.Add(new PVector(vect, true, false, true));
                }
            }

            // combine PColumns SColumns and AColumns into PColumns
            PColumns.AddRange(sColumns);
            PColumns.AddRange(aColumns);

            // mark z column as not artificial, move to front
            if (IsTwoPhase)
            {
                PColumns.Last().IsArtificial = false;
                PColumns.Insert(0, PColumns.Last());
                PColumns.RemoveAt(PColumns.Count - 1);
            }

            // create ZRow 
            var zRow = DenseVector.Create(PColumns.Count, x => x = 0);
            if (IsTwoPhase)
            {
                // add up and negate coefficient values for temp objective function
                // look at all rows containing an artificial variable (except for the last one, which is for the old z)
                foreach (var a in aColumns.Take(aColumns.Count - 1))
                {
                    //super jank
                    zRow += DenseVector.OfEnumerable(PColumns.Take(PColumns.Count - aColumns.Count + 1).Select(v => v[a.MaximumIndex()]).Concat(new double[aColumns.Count - 1]));
                }
                zRow *= -1;
                zRow.At(0, 0);
            }
            else
            {
                // create ZRow vector of standardized goal
                zRow = DenseVector.OfEnumerable(stdGoalCoefs.Concat(new double[PColumns.Count - stdGoalCoefs.Length]));
            }

            //add zRow values to pColumns
            for (int i = 0; i < zRow.Count; i++)
            {
                PColumns[i].ZValue = zRow[i];
            }

            // create XColumn vector of RHS values
            XColumn = DenseVector.OfEnumerable(Constraints.Select(c => c.Value));
            // add extra term for two-phase
            if (IsTwoPhase)
            {
                XColumn[XColumn.Count - 1] = Goal.ConstantTerm;
            }

            IsStandardized = true;
        }

        public static InternalModel FromModel(Model m)
        {
            return new InternalModel()
            {
                IsStandardized = false,
                Constraints = m.Constraints,
                Goal = m.Goal,
                GoalKind = m.GoalKind
            };
        }
    }
}
