namespace Playground.Generation
{
    public static class SimpleNoise
    {
        public static int GetNoise(int x, int y, int z)
        {
            int v = z + x * 374761393 + y * 668265263;
            v = (v ^ (v >> 13)) * 1274126177;
            return v ^ (v >> 16);
        }

        // returns a 0 to 1 value
        public static double GetNoiseDouble(int x, int y, int z)
        {
            return GetNoise(x, y, z) / (double)int.MaxValue;
        }

        public static float GetNoiseFloat(int x, int y, int z)
        {
            return (float)GetNoiseDouble(x, y, z);
        }
    }
}
