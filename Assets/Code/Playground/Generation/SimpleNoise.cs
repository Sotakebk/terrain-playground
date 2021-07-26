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

        // returns a -1 to 1 double value
        public static double GetNoiseDouble(int x, int y, int z)
        {
            return GetNoise(x, y, z) / (double)int.MaxValue;
        }

        public static double GetNoiseDoubleNormal(int x, int y, int z)
        {
            double value = (GetNoiseDouble(x, y, z) + 1) / 2.0;
            return (value < 0) ? 0 : ((value > 1) ? 1 : value);
        }
    }
}
