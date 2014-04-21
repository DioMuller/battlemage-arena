using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattlemageArena.Core.Extension
{
    public static class Vector2Extension
    {
        /// <summary>
        /// Rotates a vector (in degrees).
        /// </summary>
        /// <param name="vector">Vector to be rotated.</param>
        /// <param name="degrees">Degrees to be rotated.</param>
        /// <returns>Rotated vector.</returns>
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            return RotateRadians(vector, MathHelper.ToRadians(degrees));
        }

        /// <summary>
        /// Rotates a vector (in radians).
        /// </summary>
        /// <param name="vector">Vector to be rotated.</param>
        /// <param name="radians">Radians to be rotated.</param>
        /// <returns>Rotated vector.</returns>
        public static Vector2 RotateRadians(this Vector2 vector, float radians)
        {
            Matrix rotation = Matrix.CreateRotationZ(radians);
            return Vector2.Transform(vector, rotation);
        }

        /// <summary>
        /// Gets vector angle.
        /// </summary>
        /// <param name="vector">Vector.</param>
        /// <returns></returns>
        public static float GetAngle(this Vector2 vector)
        {
            //Since Up is (0, -1)
            return (float)Math.Atan2(vector.X, -vector.Y);
            //return (float) Math.Atan2((double) vector.Y, (double) vector.X);
        }
    }
}
