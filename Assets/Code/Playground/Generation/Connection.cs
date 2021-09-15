namespace Playground.Generation
{
    public struct ConnectionPoint
    {
        public int Block;
        public int Parameter;

        public ConnectionPoint(int block, int parameter)
        {
            Block = block;
            Parameter = parameter;
        }
    }

    public sealed class Connection
    {
        public ConnectionPoint Input;
        public ConnectionPoint Output;
    }
}
