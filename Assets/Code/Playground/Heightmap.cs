namespace Playground
{
    public class Heightmap
    {
        public float[] data;
        public readonly int size;

        public Heightmap(int size)
        {
            if ((size & (size - 1)) != 0)
                throw new System.ArgumentException("'size' not power of 2");

            this.size = size;
            data = new float[size * size];
        }

        public Heightmap Clone()
        {
            var h = new Heightmap(size);
            System.Array.Copy(data, h.data, size * size);
            return h;
        }

        public float this[int p]
        {
            get
            {
                return data[p];
            }
            set
            {
                data[p] = value;
            }
        }

        public float this[int x, int y]
        {
            get
            {
                return data[size * y + x];
            }
            set
            {
                data[size * y + x] = value;
            }
        }
    }
}
