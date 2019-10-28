using System;

namespace FractionCalc
{
    /// <summary>
    /// Define a fraction class and operations required to support add, subtract, multiply, divide ops
    /// </summary>
    public class Fraction
    {
        long wholePart;  // For a mixed fraction or whole numeratorber
        long numerator;        
        long denominator;

        public Fraction (long whole, long n, long d)
        {
            wholePart = whole;
            numerator = n;
            denominator = d;
        }
        
        /// <summary>
        /// Return simplified version of a mixed/proper/improper fraction, if possible; or original, if not.
        /// Mixed fraction could contain improper fraction.
        /// </summary>
        public void simplify()
        {
            long gcd = GCD(abs(numerator), abs(denominator));
            if (gcd > 1)
            {
                numerator = numerator / gcd;
                denominator = denominator / gcd;
                if (denominator == 1)
                {
                    if (wholePart >= 0)
                    {
                        wholePart += numerator;
                    }
                    else
                    {
                        wholePart -= numerator;
                    }

                    numerator = 0;
                }
            }

            // handle improper case, which assumes fraction is not mixed.
            if (abs(numerator) > denominator)
            {
                long whole = numerator / denominator;
                numerator = abs(numerator % denominator);
                wholePart = whole;
            }
        }

        /// <summary>
        /// Convert mixed fraction to proper/improper fraction
        /// </summary>
        public void convert()
        {
            if (wholePart > 0)
            {
                numerator += denominator * wholePart;
            } else if (wholePart < 0) {
                numerator = denominator * wholePart - numerator;
            }

            wholePart = 0;
        }

        /// <summary>
        /// Multiple two non-mixed fractions
        /// </summary>
        public void multiply(Fraction f)
        {
            numerator *= f.numerator;
            denominator *= f.denominator;
        }

        /// <summary>
        /// Divide two non-mixed fractions
        /// </summary>
        public void divide(Fraction divisorF)
        {
            numerator *= divisorF.denominator;
            denominator *= divisorF.numerator;
        }

        /// <summary>
        /// Add two non-mixed fractions
        /// </summary>
        public void add(Fraction addendF)
        {
            long commonDenominator = denominator * addendF.denominator;
            numerator = (commonDenominator / denominator * numerator) + (commonDenominator / addendF.denominator * addendF.numerator);
            denominator = commonDenominator;
        }

        /// <summary>
        /// Subtract one non-mixed fraction from the other
        /// </summary>
        /// <param name="subtrahendF"></param>
        public void subtract(Fraction subtrahendF)
        {
            if (subtrahendF.wholePart != 0)
            {
                subtrahendF.wholePart *= -1;
            }

            else
            {
                subtrahendF.numerator *= -1;
            }

            add(subtrahendF);
        }

        /// <summary>
        /// Output fraction handling various special cases
        /// </summary>
        public void writeResult()
        {
            simplify();
            Console.Write("= ");
            if (numerator == 0)
            {
                Console.WriteLine(wholePart.ToString());
            }
            else if (wholePart == 0 && numerator != 0)
            {
                Console.WriteLine(numerator.ToString() + "/" + denominator.ToString());
            }
            else if (wholePart != 0 && numerator != 0)
            {
                Console.WriteLine(wholePart.ToString() + "_" + numerator.ToString() + "/" + denominator.ToString());
            }
        }

        /// <summary>
        /// Return absolute value
        /// </summary>
        private long abs(long a)
        {
            return a >= 0 ? a : -a;
        }

        /// <summary>
        /// return greatest common divisor of a and b
        /// </summary>
        private long GCD(long a, long b)
        {
            if (a == 0 || b == 0)
            {
                return 1;
            }

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }
    }
}
