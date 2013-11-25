using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using Vector = MathNet.Numerics.LinearAlgebra.Generic.Vector<double>;

namespace RaikesSimplexService.HollmanJagerJohnson
{
    internal class PVector : DenseVector
    {
        public bool IsBasic { get; set; }
        public bool IsArtificial { get; set; }
        public bool IsDecision { get; set; }
        public double ZValue { get; set; }

        public PVector(Vector v, bool basic, bool isDecision, bool artificial)
            : base(v)
        {
            IsBasic = basic;
            IsDecision = isDecision;
            IsArtificial = artificial;
        }
    }
}
