using System;

namespace Playground.Generation
{
    public static class DiamondSquare
    {
        public static void Fill(Heightmap map, int seed, float noisiness = 0.9f, float noiseDamping = 0.9f, float MidpointStrength = 0.25f)
        {
            int s = map.size;
            int s1 = s + 1;
            float[] heights = new float[s1 * s1];

            heights[0] = GetHeight(0.5f, 0.5f, 0, 0);
            heights[s] = GetHeight(0.5f, 0.5f, s, 0);
            heights[s * s1] = GetHeight(0.5f, 0.5f, 0, s);
            heights[s + s * s1] = GetHeight(0.5f, 0.5f, s, s);

            int size = s;
            for (int i = 0; (s >> i) > 1; i++)
            {
                int subs = 1 << i;
                for (int x = 0; x < subs; x++)
                {
                    for (int y = 0; y < subs; y++)
                    {
                        Diamond(x * size, y * size, size);
                    }
                }

                for (int x = 0; x <= subs; x++)
                {
                    for (int y = 0; y <= subs; y++)
                    {
                        // X-edge
                        if (y * size + (size / 2) < s1)
                            Square(x * size, y * size + (size / 2), size / 2);

                        // Y-edge
                        if (x * size + (size / 2) < s1)
                            Square(x * size + (size / 2), y * size, size / 2);
                    }
                }
                size >>= 1;
                noisiness *= noiseDamping;
            }

            Helper.ResizeCopy(heights, map.data, s1, s);

            void Square(int x, int y, int size)
            {
                /*
                 *        YUP
                 *
                 *
                 * XLEFT  XY    XRIGHT
                 *
                 *
                 *       YDOWN
                 *
                 */

                int xleft = x - size;
                int xright = x + size;
                int yup = y + size;
                int ydown = y - size;

                float min = float.MaxValue;
                float max = float.MinValue;
                float sum = 0;
                float n = 0;

                if (xleft >= 0)
                {
                    var v = heights[xleft + y * s1];
                    min = Math.Min(min, v);
                    max = Math.Max(max, v);
                    sum += v;
                    n++;
                }
                if (xright < s1)
                {
                    var v = heights[xright + y * s1];
                    min = Math.Min(min, v);
                    max = Math.Max(max, v);
                    sum += v;
                    n++;
                }
                if (yup < s1)
                {
                    var v = heights[x + yup * s1];
                    min = Math.Min(min, v);
                    max = Math.Max(max, v);
                    sum += v;
                    n++;
                }
                if (ydown >= 0)
                {
                    var v = heights[x + ydown * s1];
                    min = Math.Min(min, v);
                    max = Math.Max(max, v);
                    sum += v;
                    n++;
                }

                float average = sum / n;
                float amplitude = noisiness * Math.Abs(min - max) * 0.5f;

                heights[x + y * s1] = GetHeight(average, amplitude, x, y);
            }

            void Diamond(int x, int y, int size)
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

                int maxx = x + size;
                int maxy = y + size;
                float xy = heights[x + y * s1];
                float x1y = heights[maxx + y * s1];
                float xy1 = heights[x + maxy * s1];
                float x1y1 = heights[maxx + maxy * s1];

                float avg = (xy + x1y + xy1 + x1y1) * 0.25f;
                float min = Math.Min(Math.Min(xy, x1y), Math.Min(xy1, x1y1));
                float max = Math.Max(Math.Max(xy, x1y), Math.Max(xy1, x1y1));
                float amplitude = Math.Abs(min - max) * MidpointStrength * noisiness;

                int midx = x + size / 2;
                int midy = y + size / 2;
                heights[midx + midy * s1] = GetHeight(avg, amplitude, midx, midy);
            }

            float GetHeight(float average, float amplitude, int x, int y)
            {
                return average + Helper.Range01ToNP(SimpleNoise.GetNoiseFloat(x, y, seed)) * amplitude;
            }
        }
    }
}
