using UnityEngine;
using System;
using Unity.Mathematics;

namespace BombThrower.Utilities
{
    public static class NumberProcess
    {
        private static readonly System.Random random = new System.Random();

        /// <summary>
        /// 値を0以上にクランプします。負の値、NaN、または負の無限大の場合は0を返します。
        /// </summary>
        /// <param name="value">クランプ対象の値</param>
        /// <returns>0以上の値</returns>
        public static float ClampToZeroOrPositive(this float value, float add = 0)
        {
            return float.IsNaN(value) || float.IsNegativeInfinity(value) ? 0 : Math.Max(value + add, 0);
        }

        /// <summary>
        /// 値を0以下にクランプします。正の値、NaN、または正の無限大の場合は0を返します。
        /// </summary>
        /// <param name="value">クランプ対象の値</param>
        /// <returns>0以下の値</returns>
        public static float ClampToZeroOrNegative(this float value, float add = 0)
        {
            return float.IsNaN(value) || float.IsPositiveInfinity(value) ? 0 : Math.Min(value + add, 0);
        }

        /// <summary>
        /// 値を0以上にクランプします。負の値の場合は0を返します。
        /// </summary>
        /// <param name="value">クランプ対象の値</param>
        /// <returns>0以上の値</returns>
        public static int ClampToZeroOrPositive(this int value, int add = 0) => Math.Max(value + add, 0);

        /// <summary>
        /// 値を0以下にクランプします。正の値の場合は0を返します。
        /// </summary>
        /// <param name="value">クランプ対象の値</param>
        /// <returns>0以下の値</returns>
        public static int ClampToZeroOrNegative(this int value, int add) => Math.Min(value + add, 0);

        public static bool Chance(float chance) => random.NextDouble() <= chance;

        public static float3 Vector3Tofloat3(this Vector3 vector)
        {
            float3 r = new float3()
            {
                x = vector.x,
                y = vector.y,
                z = vector.z,
            };
            return r;
        }

        public static Vector3 Float3ToVector3(this float3 float3)
        {
            Vector3 r = new Vector3()
            {
                x = float3.x,
                y = float3.y,
                z = float3.z,
            };
            return r;
        }
    }
}
