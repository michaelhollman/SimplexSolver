using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaikesSimplexService.DataModel;


namespace RaikesSimplexService.HollmanJagerJohnson
{
    class SolutionHelper
    {
        public static Solution Optimal(double[] decisions, double value, bool alternateSolutions)
        {
            return new Solution()
            {
                Quality = SolutionQuality.Optimal,
                Decisions = decisions,
                OptimalValue = value,
                AlternateSolutionsExist = alternateSolutions
            };
        }

        public static Solution Unbounded()
        {
            return new Solution()
            {
                Quality = SolutionQuality.Unbounded
            };
        }

        public static Solution Infeasible()
        {
            return new Solution()
            {
                Quality = SolutionQuality.Infeasible
            };
        }

        public static Solution TimedOut()
        {
            return new Solution()
            {
                Quality = SolutionQuality.TimedOut
            };
        }

    }
}
