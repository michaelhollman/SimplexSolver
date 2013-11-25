using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using Matrix = MathNet.Numerics.LinearAlgebra.Generic.Matrix<double>;
using Vector = MathNet.Numerics.LinearAlgebra.Generic.Vector<double>;

namespace RaikesSimplexService.HollmanJagerJohnson
{
    internal class PVectorPrime
    {
        public PVector PColumn { get; set; }
        public DenseVector PColumnPrime { get; set; }
        public double CPrime { get; set; }

        public PVectorPrime(PVector PColumn, DenseVector PColumnPrime, double CPrime)
        {
            this.PColumn = PColumn;
            this.PColumnPrime = PColumnPrime;
            this.CPrime = CPrime;
        }
    }
}
