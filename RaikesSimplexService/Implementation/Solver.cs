using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RaikesSimplexService.Contracts;
using RaikesSimplexService.DataModel;

namespace RaikesSimplexService.HollmanJagerJohnson
{
    public class Solver : ISolver
    {
        public Solution Solve(Model model)
        {
            var mod = InternalModel.FromModel(model);
            var solution = mod.Solve();
            if (!solution.WasOptimal)
            {
                return solution.PhaseSolution;
            }
            if (solution.WasTwoPhase)
            {
                solution = solution.GetNextPhaseModel().Solve();
            }
            return solution.PhaseSolution;
        }
    }
}
