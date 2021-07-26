using UnityEngine;

namespace Playground
{
    public class DisplayManager : MonoBehaviour
    {
        public Material HeightmapMaterial;

        public Mesh plane;

        private void Awake()
        {
            CreateMesh();
        }

        private const int cvert = 129; // number of vertices along an axis
        private const int cquad = 128; // number of 'quads' along an axis

        private void CreateMesh()
        {
            plane = new Mesh();
            Vector3[] verts = new Vector3[cvert * cvert];
            int[] indices = new int[cquad * cquad * 6];

            for (int z = 0; z < cvert; z++)
                for (int x = 0; x < cvert; x++)
                    verts[x + z * cvert] = new Vector3(x, 0, z);

            for (int z = 0; z < cquad; z++)
            {
                for (int x = 0; x < cquad; x++)
                {
                    int i = (z * cquad + x) * 6;

                    /* 0 1     1 1
                     * * - - - *
                     * | \     |
                     * |   \   |
                     * |     \ |
                     * * - - - *
                     * 0 0     1 0
                     */

                    indices[i] = (x + 1) + z * cvert;
                    indices[i + 1] = x + z * cvert;
                    indices[i + 2] = (x + 1) + (z + 1) * cvert;

                    indices[i + 3] = (x + 1) + (z + 1) * cvert;
                    indices[i + 4] = x + z * cvert;
                    indices[i + 5] = x + (z + 1) * cvert;
                }
            }

            plane.vertices = verts;
            plane.SetIndices(indices, MeshTopology.Triangles, 0, true);
            // 99999 - a big number
            plane.bounds = new Bounds(new Vector3(cvert / 2f, 0, cvert / 2f), new Vector3(cvert / 2f, 99999, cvert / 2f));
            plane.UploadMeshData(true);
        }

        public void ApplyHeightmap(Heightmap h)
        {
            ApplyToTexture(h);
            ApplyToMesh(h);
        }

        private Texture2D heightmapTexture;

        private void ApplyToTexture(Heightmap h)
        {
            if (heightmapTexture == null || h.size != heightmapTexture.width)
            {
                // need a new texture
                if (!SystemInfo.SupportsTextureFormat(TextureFormat.RFloat))
                {
                    throw new System.Exception("SystemInfo.SupportsTextureFormat(TextureFormat.RFloat) = FALSE");
                }

                Destroy(heightmapTexture);

                heightmapTexture = new Texture2D(1024, 1024, TextureFormat.RFloat, false, true);
                heightmapTexture.wrapMode = TextureWrapMode.Clamp;
                heightmapTexture.filterMode = FilterMode.Point;
                heightmapTexture.anisoLevel = 0;
            }

            heightmapTexture.SetPixelData(h.data, 0);
            heightmapTexture.Apply();
            HeightmapMaterial.SetTexture("_HeightTex", heightmapTexture);
        }

        private GameObject[] parts;

        private void ApplyToMesh(Heightmap h)
        {
            int d = (h.size / cquad);

            if (parts != null && parts.Length != d)
            {
                // need a different amount of parts
                foreach (GameObject go in parts)
                    Destroy(go);
                parts = null;
            }

            if (parts == null)
            {
                parts = new GameObject[d * d];
                for (int y = 0; y < d; y++)
                {
                    for (int x = 0; x < d; x++)
                    {
                        parts[x + y * d] = CreatePart(x * cquad, y * cquad);
                    }
                }
            }
        }

        private GameObject CreatePart(int x, int y)
        {
            System.Type[] comps = { typeof(MeshFilter), typeof(MeshRenderer) };

            var go = new GameObject($"{x}, {y}", comps);
            go.transform.parent = transform;
            go.transform.position = new Vector3(x, 0, y);

            go.GetComponent<MeshFilter>().sharedMesh = plane;
            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = HeightmapMaterial;
            var props = new MaterialPropertyBlock();
            props.SetVector("_Offset", new Vector4(x, y, 0, 0));
            mr.SetPropertyBlock(props);

            return go;
        }
    }
}
