using System;

namespace WotDossier.Applications
{
    public static class Ext
    {
        /// <summary>
        /// The base number for binary values
        /// </summary>
        private const int BinaryBaseNumber = 2;

        /// <summary>
        /// The number of binary digits used to represent the binary number for a double precision floating
        /// point value. i.e. there are this many digits used to represent the
        /// actual number, where in a number as: 0.134556 * 10^5 the digits are 0.134556 and the exponent is 5.
        /// </summary>
        private const int DoublePrecision = 53;

        /// <summary>
        /// The maximum relative precision of a double
        /// </summary>
        private static readonly double _doubleMachinePrecision = Math.Pow(BinaryBaseNumber, -DoublePrecision);

        /// <summary>Value representing 10 * 2^(-52)</summary>
        private static readonly double _defaultDoubleRelativeAccuracy = _doubleMachinePrecision * 10;

        /// <summary>
        /// Checks whether two real numbers are almost equal.
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>true if the two values differ by no more than 10 * 2^(-52); false otherwise.</returns>
        public static bool AlmostEqual(this double a, double b)
        {
            double diff = a - b;
            return AlmostEqualWithError(a, b, diff, _defaultDoubleRelativeAccuracy);
        }

        /// <summary>
        /// Compares two doubles and determines if they are equal within the specified
        /// maximum error.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="diff">
        /// The difference of the two values (according to some norm).
        /// </param>
        /// <param name="maximumError">
        /// The accuracy required for being almost equal.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if both doubles are almost equal up to the specified
        /// maximum error, <see langword="false" /> otherwise.
        /// </returns>
        public static bool AlmostEqualWithError(this double a, double b, double diff, double maximumError)
        {
            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            if (double.IsInfinity(a) || double.IsInfinity(b))
            {
                return a == b;
            }

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            if (double.IsNaN(a) || double.IsNaN(b))
            {
                return false;
            }

            if (Math.Abs(a) < _doubleMachinePrecision || Math.Abs(b) < _doubleMachinePrecision)
            {
                return AlmostEqualWithAbsoluteError(a, b, diff, maximumError);
            }

            return AlmostEqualWithRelativeError(a, b, diff, maximumError);
        }

        /// <summary>
        /// Compares two doubles and determines if they are equal within the specified
        /// maximum absolute error.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="diff">
        /// The difference of the two values (according to some norm).
        /// </param>
        /// <param name="maximumAbsoluteError">
        /// The absolute accuracy required for being almost equal.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if both doubles are almost equal up to the specified
        /// maximum absolute error, <see langword="false" /> otherwise.
        /// </returns>
        public static bool AlmostEqualWithAbsoluteError(this double a, double b, double diff, double maximumAbsoluteError)
        {
            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            if (double.IsInfinity(a) || double.IsInfinity(b))
            {
                return a == b;
            }

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            if (double.IsNaN(a) || double.IsNaN(b))
            {
                return false;
            }

            return Math.Abs(diff) < maximumAbsoluteError;
        }

        /// <summary>
        /// Compares two doubles and determines if they are equal within the specified
        /// maximum relative error.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="diff">The difference of the two values (according to some norm).
        /// </param>
        /// <param name="maximumRelativeError">The relative accuracy required for being
        /// almost equal.</param>
        /// <returns>
        /// <see langword="true" /> if both doubles are almost equal up to the specified
        /// maximum relative error, <see langword="false" /> otherwise.
        /// </returns>
        public static bool AlmostEqualWithRelativeError(this double a, double b, double diff, double maximumRelativeError)
        {
            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            if (double.IsInfinity(a) || double.IsInfinity(b))
            {
                return a == b;
            }

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            if (double.IsNaN(a) || double.IsNaN(b))
            {
                return false;
            }

            if ((a == 0 && Math.Abs(b) < maximumRelativeError)
                || (b == 0 && Math.Abs(a) < maximumRelativeError))
            {
                return true;
            }

            return Math.Abs(diff) < maximumRelativeError * Math.Max(Math.Abs(a), Math.Abs(b));
        }
    }
}