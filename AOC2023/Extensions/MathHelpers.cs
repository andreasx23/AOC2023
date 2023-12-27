using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Extensions
{
    public static class MathHelpers
    {
        /// <summary>
        /// min <= value <= max
        /// </summary>
        public static bool IsInsideRange<T>(this T value, T min, T max) where T : INumber<T>
        {
            return min <= value && value <= max;
        }

        public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
        {
            while (b != T.Zero)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
        {
            return a / GreatestCommonDivisor(a, b) * b;
        }

        public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        {
            return values.Aggregate(LeastCommonMultiple);
        }

        public static long Shoelace(this List<(int x, int y)> corners)
        {
            //  The area of a polygon bounded by vertices 'v' can be calculated using the Shoelace Formula:
            //  A = 1/2 * |Sum v[i] ^ v[i + 1]|

            corners.Add(corners[0]);

            long area = 0;
            for (int i = 0; i < corners.Count - 1; i++)
            {
                var a = corners[i];
                var b = corners[(i + 1) % corners.Count];
                area += ((long)b.x + a.x) * ((long)b.y - a.y);
            }

            return Math.Abs(area / 2L);
        }

        public static long PicksTheorem(this long a, long b)
        {
            //  The interior area of a polygon can be calculated using Pick's Theorem:
            //  I = A - B/2 + 1

            return a - b / 2 + 1;
        }
    }
}
