using System.Collections.Generic;

namespace Playground.Generation
{
    public class Graph
    {
        private readonly List<Connection> connections;
        private readonly List<BlockContainer> blocks;

        public string Name { get; set; }
        public IReadOnlyList<Connection> Connections => connections;
        public IReadOnlyList<BlockContainer> Blocks => blocks;

        public void AddBlock(Block block, BlockMetadata metadata)
        {
            blocks.Add(new BlockContainer(block, metadata));
        }

        public void RemoveBlock(int index)
        {
            if (index < 0 || index >= blocks.Count)
                return;

            blocks.RemoveAt(index);
        }

        public void AddConnection(int inBlock, int inParameter, int outBlock, int outParameter)
        {
            connections.Add(new Connection()
            {
                Input = new ConnectionPoint(inBlock, inParameter),
                Output = new ConnectionPoint(outBlock, outParameter)
            });
        }

        public void RemoveConnection(int index)
        {
            if (index < 0 || index >= connections.Count)
                return;

            connections.RemoveAt(index);
        }

        public void Move(int index, int new_index)
        {
            if (new_index < 0 || new_index >= blocks.Count)
                return;

            var block = blocks[index];
            var block2 = blocks[new_index];
            blocks[new_index] = block;
            blocks[index] = block2;
        }

        public Graph()
        {
            connections = new List<Connection>();
            blocks = new List<BlockContainer>();
        }
    }
}
