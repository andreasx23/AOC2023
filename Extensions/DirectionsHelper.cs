using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class DirectionsHelper
    {
        public static readonly Dictionary<string, (int x, int y)> HexagonDirections = new()
        {
            { "n", (0, -1) },
            { "ne", (1, -1) },
            { "se", (1, 0) },
            { "s", (0, 1) },
            { "sw", (-1, 1) },
            { "nw", (-1, 0) },
        };

        public static readonly Dictionary<string, (int x, int y)> CardinalDirections = new()
        {
            { "n", (-1, 0) },
            { "s", (1, 0) },
            { "e", (0, 1) },
            { "w", (0, -1) },
        };

        public static readonly Dictionary<string, (int x, int y)> OrdinalDirections = new()
        {
            { "n", (-1, 0) },
            { "s", (1, 0) },
            { "e", (0, 1) },
            { "w", (0, -1) },
            { "nw", (-1, -1) },
            { "ne", (-1, 1) },
            { "se", (1, 1) },
            { "sw", (1, -1) },
        };

        public static List<(int x, int y)> GetValidDirections(IEnumerable<(int x, int y)> dirs, int height, int width, int x, int y)
        {
            List<(int x, int y)> validDirs = new();
            foreach (var item in dirs)
            {
                var dx = item.x + x;
                var dy = item.y + y;

                if (dx < 0 || dx >= height || dy < 0 || dy >= width)
                {
                    continue;
                }

                validDirs.Add((dx, dy));
            }

            return validDirs;
        }

        public static List<(int x, int y)> GetDirections(IEnumerable<(int x, int y)> dirs, int x, int y)
        {
            List<(int x, int y)> validDirs = new();
            foreach (var item in dirs)
            {
                var dx = item.x + x;
                var dy = item.y + y;
                validDirs.Add((dx, dy));
            }

            return validDirs;
        }
    }
}
