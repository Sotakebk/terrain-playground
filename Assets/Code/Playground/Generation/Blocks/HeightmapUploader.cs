using Playground.Graphics;

namespace Playground.Generation.Blocks
{
    public class HeightmapUploader : Block
    {
        [BlockVariable]
        public string HeightmapName;

        [BlockMethod]
        public void UploadHeightmap(Heightmap input)
        {
            services.GetService<GraphicsManager>().AddHeightmap(input, HeightmapName);
        }
    }
}
