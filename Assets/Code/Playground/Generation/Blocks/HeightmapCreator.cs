namespace Playground.Generation.Blocks
{
    public class HeightmapCreator : Block
    {
        [BlockVariable]
        public int Size;

        [BlockMethod]
        public void CreateHeightmap(out Heightmap output)
        {
            output = new Heightmap(Size);
        }
    }
}
