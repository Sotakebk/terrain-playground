namespace Playground.Generation
{
    public sealed class BlockContainer
    {
        public Block Block { get; private set; }
        public BlockMetadata BlockMetadata { get; private set; }
        public int SelectedMethod { get; private set; }

        public BlockContainer(Block block, BlockMetadata metadata)
        {
            BlockMetadata = metadata;
            Block = block;
            SelectedMethod = 0;
        }
    }
}
