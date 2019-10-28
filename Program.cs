using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FractionCalc
{
    /// <summary>
    /// Implements a simple fraction calculator.  
    /// 
    /// Design assumptions:
    /// 1) all improper fractions will output as a mixed fraction
    /// 2) For simplicity, apply operators in order rather than according to usual rules of operator precedence
    /// 3) Numbers will be small enough to be stored in long type
    /// 4) Mixed fraction input could contain improper fraction
    /// 5) Some error checking included, but not all possible checking due to time constraint
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // match 0+ whitespace, then mixed fraction or [im]proper fraction or whole number followed by space
            string fractionPattern = @"\s*((?<whole>\d+)_(?<num>\d+)/(?<denom>\d+)|(?<num>\d+)/(?<denom>\d+)|(?<whole>\d+\s*))";                         

            // match 0+ whitespace then any of 4 allowed operators
            string operatorPattern = @"\s*(?<oper>[\+\*-/])";

            Regex queryPattern = new Regex(@"
                \s*                   # match zero or more whitespace characters, then ...
                [\?]                  # match query operator, then ...
                (?<remainder>.*)      # rest of input
                ", RegexOptions.IgnorePatternWhitespace);

            long whole, num, denom;
            Fraction first;

            Console.WriteLine("Enter a math expression using whole numbers, fractions and operators.  Begin with '?' symbol.");
            Console.WriteLine("Valid operators are +, -, *, /.");
            Console.WriteLine("Valid operands are whole numbers (x), mixed fractions (x_x/x), and proper/improper fractions (x/x).");
            Console.WriteLine("If '?' symbol is omitted, or syntax error occurs, program ends.");

            while (true)
            {
                string input = Console.ReadLine();

                // verify query symbol included
                MatchCollection mc = queryPattern.Matches(input);
                if (mc.Count == 0)
                {
                    Console.WriteLine("Query symbol '?' missing.");
                    break;
                }

                // get first whole number or fraction
                input = mc[0].Groups["remainder"].Value;
                string firstFractionPattern = fractionPattern + @"(?<remainder>.*)";
                Regex firstRegex = new Regex(firstFractionPattern);
                mc = firstRegex.Matches(input);
                if (mc.Count == 0)
                {
                    Console.WriteLine("First operand incorrectly specified.");
                    break;
                }

                long.TryParse(mc[0].Groups["whole"].Value, out whole);
                long.TryParse(mc[0].Groups["num"].Value, out num);
                long.TryParse(mc[0].Groups["denom"].Value, out denom);
                first = new Fraction(whole, num, denom == 0 ? 1 : denom);
                input = mc[0].Groups["remainder"].Value;

                // Get rest of operators and whole number/fractions
                string opFractionPattern = operatorPattern + fractionPattern;
                Regex restRegex = new Regex(opFractionPattern);
                mc = restRegex.Matches(input);
                if (mc.Count == 0)
                {
                    Console.WriteLine("Invalid expression found.");
                    break;
                }

                //Calculate output result
                first.convert();
                foreach (Match m in mc)
                {
                    string oper = m.Groups["oper"].Value;
                    long.TryParse(m.Groups["whole"].Value, out whole);
                    long.TryParse(m.Groups["num"].Value, out num);
                    long.TryParse(m.Groups["denom"].Value, out denom);
                    Fraction next = new Fraction(whole, num, denom == 0 ? 1 : denom);
                    next.convert();
                    switch (oper)
                    {
                        case "+":
                            first.add(next);
                            break;
                        case "-":
                            first.subtract(next);
                            break;
                        case "*":
                            first.multiply(next);
                            break;
                        case "/":
                            first.divide(next);
                            break;
                    }
                }

                first.writeResult();
            }
        }
    }
}
