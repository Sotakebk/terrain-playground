using UnityEngine;
using Playground.Generation;

namespace Playground
{
    public class HeightmapGenerator
    {
        public Heightmap GenerateHeightmap()
        {
            var h = new Heightmap(1024);

            MidpointDisplacement.FillWeirdMidpoint(h, 1);

            Helper.Normalize(h);

            return h;
        }
    }
}
