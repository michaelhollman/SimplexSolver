using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaikesSimplexService.DataModel;
using MathNet.Numerics.LinearAlgebra.Double;
using Matrix = MathNet.Numerics.LinearAlgebra.Generic.Matrix<double>;
using Vector = MathNet.Numerics.LinearAlgebra.Generic.Vector<double>;

namespace RaikesSimplexService.HollmanJagerJohnson
{
    internal class InternalSolution
    {

        public Solution PhaseSolution { get; set; }
        private InternalModel PrevModel { get; set; }

        public bool WasTwoPhase
        {
            get { return PrevModel.IsTwoPhase; }
        }

        public bool WasOptimal
        {
            get { return PhaseSolution.Quality == SolutionQuality.Optimal; }
        }

        public InternalSolution(Solution phaseSolution)
        {
            PhaseSolution = phaseSolution;
        }

        public InternalSolution(Solution phaseSolution, InternalModel prevModel)
        {
            PhaseSolution = phaseSolution;
            PrevModel = prevModel;
        }

        public InternalModel GetNextPhaseModel()
        {
            // this process is essentially the same computation as an iteration of the revised method
            // esentiall, just create (allthethings) p primes
            var basicInverse = DenseMatrix.OfColumnVectors(PrevModel.PColumns.Where(p => p.IsBasic).ToArray()).Inverse();

            var zPrime = DenseVector.OfEnumerable((basicInverse * PrevModel.PColumns[0].ToColumnMatrix()).ToColumnWiseArray());

            var zIndex = zPrime.MaximumIndex();


            var xPrime = (basicInverse * (PrevModel.XColumn.ToColumnMatrix())).ToColumnWiseArray().ToList();

            xPrime.RemoveAt(zIndex);

            var newXColumn = DenseVector.OfEnumerable(xPrime);


            var newPColumns = new List<PVector>();
            foreach (var col in PrevModel.PColumns.Skip(1))
            {
                var pPrime = (basicInverse * col.ToColumnMatrix()).ToColumnWiseArray().Select(x => Math.Round(x, 6)).ToList();
                var newZVal = pPrime.ElementAt(zIndex);
                pPrime.RemoveAt(zIndex);


                var newP = new PVector(DenseVector.OfEnumerable(pPrime), col.IsBasic, col.IsDecision, col.IsArtificial);
                newP.ZValue = newZVal;

                newPColumns.Add(newP);
            }

            var artificialCount = PrevModel.PColumns.Count(p => p.IsArtificial);

            newPColumns = newPColumns.Take(newPColumns.Count - artificialCount).ToList();

            var nextModel = new InternalModel()
            {
                IsTwoPhase = false,
                IsStandardized = true,
                XColumn = newXColumn,
                PColumns = newPColumns,
                GoalKind = PrevModel.GoalKind,
                Goal = PrevModel.Goal
            };
            return nextModel;
        }
    }
}
