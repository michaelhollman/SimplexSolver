using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaikesSimplexService.HollmanJagerJohnson;
using RaikesSimplexService.DataModel;


namespace RaikesSimplexService.HollmanJagerJohnson.Demo
{
    class Program
    {
        static void Main(string[] args)
        {

            var solver = new Solver();

            var constraints = new List<LinearConstraint>();

            constraints.Add(new LinearConstraint()
                {
                    Coefficients = new double[] { 1, 1, 1 },
                    Relationship = Relationship.LessThanOrEquals,
                    Value = 30
                });
            constraints.Add(new LinearConstraint()
                {
                    Coefficients = new double[] { 1, 0, 1 },
                    Relationship = Relationship.GreaterThanOrEquals,
                    Value = 15
                });
            constraints.Add(new LinearConstraint()
                {
                    Coefficients = new double[] { 0, 1, 1 },
                    Relationship = Relationship.GreaterThanOrEquals,
                    Value = 20
                });

            var goal = new Goal()
            {
                Coefficients = new double[] { 2, 1, 4 },
                ConstantTerm = 0
            };

            var model = new Model()
            {
                Constraints = constraints,
                Goal = goal,
                GoalKind = GoalKind.Maximize
            };

            var expected = new Solution()
            {
                Decisions = new double[] { 0, 0, 30 },
                Quality = SolutionQuality.Optimal,
                AlternateSolutionsExist = false,
                OptimalValue = 120
            };

            Console.WriteLine("Simplex solver by Michael Hollman, Sawyer Jager, and Darren Johnson");
            Console.WriteLine();

            Console.WriteLine(string.Format("{0} the objective function with coefficients:", model.GoalKind));
            Console.WriteLine(string.Join(", ", goal.Coefficients));
            Console.WriteLine(string.Format("and a constant term of {0}", goal.ConstantTerm));
            Console.WriteLine("bound by the following constraints:");
            Console.WriteLine();

            foreach (var c in model.Constraints)
            {
                Console.WriteLine(string.Format("Coefficients:  {0}", string.Join(", ", c.Coefficients)));
                Console.WriteLine(string.Format("Relationship:  {0}", c.Relationship));
                Console.WriteLine(string.Format("RHS Value:     {0}", c.Value));
                Console.WriteLine();
            }

            // SOLVE IT
            var actual = solver.Solve(model);

            Console.WriteLine("Solution values    [ expected | actual ]");
            Console.WriteLine(string.Format("Quality:           [ {0} | {1} ]", expected.Quality, expected.Quality));
            Console.WriteLine(string.Format("Decisions:         [ {0} | {1} ]", string.Join(", ", expected.Decisions), string.Join(", ", actual.Decisions)));
            Console.WriteLine(string.Format("Value:             [ {0} | {1} ]", expected.OptimalValue, expected.OptimalValue));
            Console.WriteLine(string.Format("Alternates:        [ {0} | {1} ]", expected.AlternateSolutionsExist, expected.AlternateSolutionsExist));

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
