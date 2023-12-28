using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class DistanceHelper
    {
        public static (int x, int y, int z) ToCubeCoordinates(int x, int y)
        {
            int z = -x - y;
            return (x, y, z);
        }

        public static int CalculateDistanceFor3DSpaceIn2DSpace((int x, int y) startPosition, (int x, int y) targetPosition)
        {
            var cubeStartPosition = ToCubeCoordinates(startPosition.x, startPosition.y);
            var cubeTargetPosition = ToCubeCoordinates(targetPosition.x, targetPosition.y);
            return (Math.Abs(cubeStartPosition.x - cubeTargetPosition.x)
                    + Math.Abs(cubeStartPosition.y - cubeTargetPosition.y)
                    + Math.Abs(cubeStartPosition.z - cubeTargetPosition.z)) / 2;
        }

        public static int ManhattenDistance(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(x - targetX) + Math.Abs(y - targetY);
        }

        public static double EuclideanDistance(int x, int y, int targetX, int targetY)
        {
            var dx = Math.Pow(x - targetX, 2);
            var dy = Math.Pow(y - targetY, 2);
            return Math.Sqrt(dx + dy);
        }
    }
}
