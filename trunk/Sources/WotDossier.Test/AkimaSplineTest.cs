﻿// <copyright file="AkimaSplineTest.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2002-2013 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using NUnit.Framework;
using WotDossier.Framework.Interpolation;

namespace WotDossier.Test
{
    /// <summary>
    /// AkimaSpline test case.
    /// </summary>
    [TestFixture]
    public class AkimaSplineTest
    {
        /// <summary>
        /// Sample points.
        /// </summary>
        private readonly double[] _t = new[] { -2.0, -1.0, 0.0, 1.0, 2.0 };

        /// <summary>
        /// Sample values.
        /// </summary>
        private readonly double[] _x = new[] { 1.0, 2.0, -1.0, 0.0, 1.0 };

        /// <summary>
        /// Verifies that the interpolation matches the given value at all the provided sample points.
        /// </summary>
        [Test]
        public void FitsAtSamplePoints()
        {
            IInterpolation interpolation = new AkimaSplineInterpolation(_t, _x);

            for (int i = 0; i < _x.Length; i++)
            {
                Assert.AreEqual(_x[i], interpolation.Interpolate(_t[i]), "A Exact Point " + i);

                var actual = interpolation.DifferentiateAll(_t[i]);
                Assert.AreEqual(_x[i], actual.Item1, "B Exact Point " + i);
            }
        }

        /// <summary>
        /// Verifies that at points other than the provided sample points, the interpolation matches the one computed by Maple as a reference.
        /// </summary>
        /// <param name="t">Sample point.</param>
        /// <param name="x">Sample value.</param>
        /// <param name="maxAbsoluteError">Maximum absolute error.</param>
        [TestCase(-2.4, -0.52, 1e-15)]
        [TestCase(-0.9, 1.826, 1e-15)]
        [TestCase(-0.5, 0.25, 1e-15)]
        [TestCase(-0.1, -1.006, 1e-15)]
        [TestCase(0.1, -0.9, 1e-15)]
        [TestCase(0.4, -0.6, 1e-15)]
        [TestCase(1.2, 0.2, 1e-15)]
        [TestCase(10.0, 9, 1e-15)]
        [TestCase(-10.0, -151, 1e-15)]
        public void FitsAtArbitraryPointsWithMaple(double t, double x, double maxAbsoluteError)
        {
            IInterpolation interpolation = new AkimaSplineInterpolation(_t, _x);

            // TODO: Verify the expected values (that they are really the expected ones)
            Assert.AreEqual(x, interpolation.Interpolate(t), maxAbsoluteError, "Interpolation at {0}", t);

            var actual = interpolation.DifferentiateAll(t);
            Assert.AreEqual(x, actual.Item1, maxAbsoluteError, "Interpolation as by-product of differentiation at {0}", t);
        }

        /// <summary>
        /// Verifies that the interpolation supports the linear case appropriately
        /// </summary>
        /// <param name="samples">Samples array.</param>
        //[TestCase(5)]
        //[TestCase(7)]
        //[TestCase(15)]
        //public void SupportsLinearCase(int samples)
        //{
        //    double[] x, y, xtest, ytest;
        //    LinearInterpolationCase.Build(out x, out y, out xtest, out ytest, samples);
        //    IInterpolation interpolation = new AkimaSplineInterpolation(x, y);
        //    for (int i = 0; i < xtest.Length; i++)
        //    {
        //        Assert.AreEqual(ytest[i], interpolation.Interpolate(xtest[i]), 1e-15, "Linear with {0} samples, sample {1}", samples, i);
        //    }
        //}
    }
}
