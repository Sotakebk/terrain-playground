using System.Threading.Tasks;
using UnityEngine;

namespace Playground
{
    public static class Helper
    {
        public static bool IsPowerOf2(int value)
        {
            return (value & (value - 1)) == 0;
        }

        public static void ResizeCopy<T>(T[] from, T[] to, int from_size, int to_size)
        {
            for (int y = 0; y < to_size; y++)
            {
                for (int x = 0; x < to_size; x++)
                {
                    to[x + to_size * y] = from[x + from_size * y];
                }
            }
        }

        public static float RangeNPTo01(float value)
        {
            return (value + 1f) / 2f;
        }

        public static float Range01ToNP(float value)
        {
            return (value * 2) - 1;
        }

        // E^(-x-b)
        public static float EXBCurve(float X, float B)
        {
            return Mathf.Exp(-X - B);
        }

        public static float InverseEXB(float Y, float B)
        {
            return -Mathf.Log(Y) - B;
        }

        public static void Normalize(Heightmap h)
        {
            int size = h.size;
            float min = float.MaxValue;
            float max = float.MinValue;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float v = h.data[x + y * size];
                    if (v < min) min = v;
                    if (v > max) max = v;
                }
            }

            float toZero = 0 - min;
            float toRange = 1f / (max - min);

            Parallel.For(0, size, x =>
            {
                for (int y = 0; y < size; y++)
                {
                    h.data[x + y * size] = (toZero + h.data[x + y * size]) * toRange;
                }
            });
        }
    }
}
