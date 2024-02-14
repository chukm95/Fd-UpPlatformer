using OpenTK.Mathematics;
using System;

namespace FuckedUpPlatformer.Util
{
    public static class MathUtil
    {
        private const float MULT_TO_RADIANS = MathF.PI / 180;

        public static Vector3 ToRadians(Vector3 vec)
        {
            return vec * MULT_TO_RADIANS;
        }

        public static Vector3 ToDegrees(Vector3 vec)
        {
            return vec / MULT_TO_RADIANS;
        }
    }
}
