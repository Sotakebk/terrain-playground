using UnityEngine;

namespace Playground.Generation
{
    public class GenerationManager : MonoBehaviour
    {
        public Graphics.GraphicsManager graphicsManager;

        public void Generate()
        {
            var h = new Heightmap(1024);
            Generators.DiamondSquare.Fill(h, 1);
            Helper.Normalize(h);

            graphicsManager.AddHeightmap(h, "default");
            graphicsManager.SetHeightmap("default");
        }
    }
}
