namespace Playground
{
    public class Heightmap
    {
        public readonly float[] Data;
        public readonly int Size;

        public Heightmap(int size)
        {
            if (!Helper.IsPowerOf2(size))
                throw new System.ArgumentException("'size' not power of 2");

            this.Size = size;
            Data = new float[size * size];
        }

        public Heightmap Clone()
        {
            var h = new Heightmap(Size);
            System.Array.Copy(Data, h.Data, Size * Size);
            return h;
        }

        public float this[int p]
        {
            get
            {
                return Data[p];
            }
            set
            {
                Data[p] = value;
            }
        }

        public float this[int x, int y]
        {
            get
            {
                return Data[Size * y + x];
            }
            set
            {
                Data[Size * y + x] = value;
            }
        }
    }
}
