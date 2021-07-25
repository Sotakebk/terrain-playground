using UnityEngine;

namespace Playground
{
    public class HeightmapGenerator
    {
        public Heightmap GenerateHeightmap()
        {
            var h = new Heightmap(1024);
            for (int y = 0; y < 1024; y++)
                for (int x = 0; x < 1024; x++)
                {
                    float r = Mathf.Sqrt(x * x + y * y);
                    float height = Mathf.Sin((r * Mathf.PI) / 53f);
                    h[x, y] = (height + 1f) / 2f;
                }

            return h;
        }
    }
}
