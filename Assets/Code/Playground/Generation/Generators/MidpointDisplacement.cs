using System;

namespace Playground.Generation.Generators
{
    public static class MidpointDisplacement
    {
        /* \/ X          \/ MAXX
         *  xy1  midx    x1y1 <- MAXY
         *
         *
         * midy   MID    midy
         *
         *
         * xy    midx    x1y <- Y
         *
         */

        public static void Fill(Heightmap map, int seed, float noisiness = 0.5f, float noiseDamping = 0.5f, float MidpointStrength = 0.25f)
        {
            int s = map.size;
            int s1 = s + 1;
            float[] heights = new float[s1 * s1];

            heights[0] = GetHeight(0.5f, 0.5f, 0, 0);
            heights[s] = GetHeight(0.5f, 0.5f, s, 0);
            heights[s * s1] = GetHeight(0.5f, 0.5f, 0, s);
            heights[s + s * s1] = GetHeight(0.5f, 0.5f, s, s);

            int size = s;

            for (int i = 0; (s >> (i + 1)) > 0; i++)
            {
                int subs = 1 << i;
                for (int x = 0; x < subs; x++)
                {
                    for (int y = 0; y < subs; y++)
                    {
                        Subdivision(x * size, y * size, size);
                    }
                }
                noisiness *= noiseDamping;
                size >>= 1;
            }

            Helper.ResizeCopy(heights, map.data, s1, s);

            void Subdivision(int x, int y, int size)
            {
                // coordinates
                int maxx = x + size;
                int maxy = y + size;
                int midx = (x + maxx) / 2;
                int midy = (y + maxy) / 2;

                // height values
                float xy = heights[x + y * s1];
                float x1y = heights[maxx + y * s1];
                float xy1 = heights[x + maxy * s1];
                float x1y1 = heights[maxx + maxy * s1];

                // generate edge values
                float midx_y = GetHeight((xy + x1y) / 2f, noisiness, midx, y);
                float x_midy = GetHeight((xy + xy1) / 2f, noisiness, x, midy);
                float midx_maxy = GetHeight((xy1 + x1y1) / 2f, noisiness, midx, maxy);
                float maxx_midy = GetHeight((x1y + x1y1) / 2f, noisiness, maxx, midy);

                // apply edge values
                heights[midx + y * s1] = midx_y;
                heights[x + midy * s1] = x_midy;
                heights[midx + maxy * s1] = midx_maxy;
                heights[maxx + midy * s1] = maxx_midy;

                // generate midpoint value
                var avg = (midx_y + x_midy + midx_maxy + maxx_midy) / 4;
                heights[midx + midy * s1] = GetHeight(avg, noisiness * MidpointStrength, midx, midy);
            }

            float GetHeight(float average, float range, int x, int y)
            {
                float value = Helper.Range01ToNP(SimpleNoise.GetNoiseFloat(x, y, seed));
                return value * range + average;
            }
        }

        public static void FillSensible(Heightmap map, int seed, float noisiness = 0.5f, float noiseDamping = 0.5f, float MidpointStrength = 0.25f)
        {
            int s = map.size;
            int s1 = s + 1;
            float[] heights = new float[s1 * s1];

            heights[0] = GetHeight(0.5f, 0.5f, 0, 0);
            heights[s] = GetHeight(0.5f, 0.5f, s, 0);
            heights[s * s1] = GetHeight(0.5f, 0.5f, 0, s);
            heights[s + s * s1] = GetHeight(0.5f, 0.5f, s, s);

            int size = s;

            for (int i = 0; (s >> (i + 1)) > 0; i++)
            {
                int subs = 1 << i;
                for (int x = 0; x < subs; x++)
                {
                    for (int y = 0; y < subs; y++)
                    {
                        Subdivision(x * size, y * size, size);
                    }
                }
                noisiness *= noiseDamping;
                size >>= 1;
            }

            Helper.ResizeCopy(heights, map.data, s1, s);

            void Subdivision(int x, int y, int size)
            {
                // coordinates
                int maxx = x + size;
                int maxy = y + size;
                int midx = (x + maxx) / 2;
                int midy = (y + maxy) / 2;

                // height values
                float xy = heights[x + y * s1];
                float x1y = heights[maxx + y * s1];
                float xy1 = heights[x + maxy * s1];
                float x1y1 = heights[maxx + maxy * s1];

                // generate edge values
                float midx_y = GetHeight((xy + x1y) / 2f, Math.Abs(xy - x1y) / 2f * noisiness, midx, y);
                float x_midy = GetHeight((xy + xy1) / 2f, Math.Abs(xy - xy1) / 2f * noisiness, x, midy);
                float midx_maxy = GetHeight((xy1 + x1y1) / 2f, Math.Abs(xy1 - x1y1) / 2f * noisiness, midx, maxy);
                float maxx_midy = GetHeight((x1y + x1y1) / 2f, Math.Abs(x1y - x1y1) / 2f * noisiness, maxx, midy);

                // apply edge values
                heights[midx + y * s1] = midx_y;
                heights[x + midy * s1] = x_midy;
                heights[midx + maxy * s1] = midx_maxy;
                heights[maxx + midy * s1] = maxx_midy;

                // generate midpoint value
                var avg = (midx_y + x_midy + midx_maxy + maxx_midy) / 4;
                var min = Math.Min(Math.Min(midx_y, x_midy), Math.Min(midx_maxy, maxx_midy));
                var max = Math.Max(Math.Max(midx_y, x_midy), Math.Max(midx_maxy, maxx_midy));
                var range = Math.Abs(min - max);

                heights[midx + midy * s1] = GetHeight(avg, MidpointStrength * range * noisiness, midx, midy);
            }

            float GetHeight(float average, float range, int x, int y)
            {
                float value = Helper.Range01ToNP(SimpleNoise.GetNoiseFloat(x, y, seed));
                return value * range + average;
            }
        }

        public static void FillCoherent(Heightmap map, int seed)
        {
            int s = map.size;
            int s1 = s + 1;
            float[] heights = new float[s1 * s1];

            heights[0] = GetHeight(0.5f, 0.5f, 0, 0);
            heights[s] = GetHeight(0.5f, 0.5f, s, 0);
            heights[s * s1] = GetHeight(0.5f, 0.5f, 0, s);
            heights[s + s * s1] = GetHeight(0.5f, 0.5f, s, s);

            int size = s;

            for (int i = 0; (s >> i) > 0; i++)
            {
                int subs = 1 << i;
                for (int x = 0; x < subs; x++)
                {
                    for (int y = 0; y < subs; y++)
                    {
                        Subdivision(x * size, y * size, size);
                    }
                }
                size >>= 1;
            }

            Helper.ResizeCopy(heights, map.data, s1, s);

            void Subdivision(int x, int y, int size)
            {
                // coordinates
                int maxx = x + size;
                int maxy = y + size;
                int midx = (x + maxx) / 2;
                int midy = (y + maxy) / 2;

                // height values
                float xy = heights[x + y * s1];
                float x1y = heights[maxx + y * s1];
                float xy1 = heights[x + maxy * s1];
                float x1y1 = heights[maxx + maxy * s1];

                // generate edge values
                float midx_y = GetHeight(Math.Min(xy, x1y), Math.Abs(xy - x1y), midx, y);
                float x_midy = GetHeight(Math.Min(xy, xy1), Math.Abs(xy - xy1), x, midy);
                float midx_maxy = GetHeight(Math.Min(xy1, x1y1), Math.Abs(xy1 - x1y1), midx, maxy);
                float maxx_midy = GetHeight(Math.Min(x1y, x1y1), Math.Abs(x1y - x1y1), maxx, midy);

                // apply edge values
                heights[midx + y * s1] = midx_y;
                heights[x + midy * s1] = x_midy;
                heights[midx + maxy * s1] = midx_maxy;
                heights[maxx + midy * s1] = maxx_midy;

                // middle
                var min = Math.Min(Math.Min(midx_y, x_midy), Math.Min(midx_maxy, maxx_midy));
                var max = Math.Max(Math.Max(midx_y, x_midy), Math.Max(midx_maxy, maxx_midy));
                heights[midx + midy * s1] = GetHeight(min, Math.Abs(min - max), midx, midy);
            }

            float GetHeight(float min, float range, int x, int y)
            {
                return min + SimpleNoise.GetNoiseFloat(x, y, seed) * range;
            }
        }
    }
}
