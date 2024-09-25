using UnityEngine;

namespace Omedya.ChessLib.Extensions
{
    internal static class MathExtensions
    {
        public static (int x, int y) Sign(this (int x, int y) vector)
        {
            var (x, y) = vector;

            return (x == 0 ? 0 : x / Mathf.Abs(x), y == 0 ? 0 : y / Mathf.Abs(y));
        }
    }
}