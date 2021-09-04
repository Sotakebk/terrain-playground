using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Playground.Graphics
{
    [RequireComponent(typeof(MeshGenerator))]
    public class GraphicsManager : MonoBehaviour
    {
        [SerializeField]
        private Material OriginalMaterial;

        public Material HeightmapMaterial { get; private set; }

        public Light Sun;

        public List<Texture2D> GradientTextures;
        public List<Texture2D> LineTextures;

        private MeshGenerator meshGenerator;
        private List<Texture2D> heightmapList;

        public ReadOnlyCollection<Texture2D> Heightmaps
        {
            get
            {
                return heightmapList.AsReadOnly();
            }
        }

        private void Awake()
        {
            HeightmapMaterial = new Material(OriginalMaterial);
            meshGenerator = transform.GetComponent<MeshGenerator>();
            heightmapList = new List<Texture2D>();

            if (meshGenerator == null)
                throw new System.NullReferenceException();

            if (!SystemInfo.SupportsTextureFormat(TextureFormat.RFloat))
                throw new System.Exception("SystemInfo.SupportsTextureFormat(TextureFormat.RFloat) = FALSE");

            meshGenerator.Initialize();
        }

        public void AddHeightmap(Heightmap heightmap, string submittedName)
        {
            Texture2D heightmapTexture = null;

            for (int x = 0; x < heightmapList.Count; x++)
            {
                if (heightmapList[x].name == submittedName)
                {
                    heightmapTexture = heightmapList[x];
                    break;
                }
            }

            if (heightmapTexture != null && heightmap.size != heightmapTexture.width)
                Destroy(heightmapTexture);

            if (heightmapTexture == null)
            {
                heightmapTexture = new Texture2D(heightmap.size, heightmap.size, TextureFormat.RFloat, false, true)
                {
                    name = submittedName,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Point,
                    anisoLevel = 0
                };
            }

            heightmapTexture.SetPixelData(heightmap.data, 0);
            heightmapTexture.Apply();

            heightmapList.Add(heightmapTexture);
        }

        public void SetHeightmap(string name)
        {
            for (int x = 0; x < heightmapList.Count; x++)
            {
                if (heightmapList[x].name == name)
                {
                    SetHeightTexture(heightmapList[x]);
                    break;
                }
            }
        }

        private void SetHeightTexture(Texture2D texture)
        {
            HeightmapMaterial.SetTexture("_HeightTex", texture);
            meshGenerator.ResizeMesh(texture.width);
        }

        public void RemoveHeightmap(string name)
        {
            for (int x = 0; x < heightmapList.Count; x++)
            {
                if (heightmapList[x].name == name)
                {
                    Destroy(heightmapList[x]);
                    heightmapList.RemoveAt(x);
                    break;
                }
            }
        }

        public void SetGradientTexture(int ID)
        {
            Debug.Log(ID);
            HeightmapMaterial.SetTexture("_GradientTex", GradientTextures[ID]);
        }

        public void SetLineTexture(int ID)
        {
            Debug.Log(ID);
            HeightmapMaterial.SetTexture("_LineTex", LineTextures[ID]);
        }

        public float DiffuseWeight
        {
            get
            {
                return HeightmapMaterial.GetFloat("_DiffWeight");
            }
            set
            {
                HeightmapMaterial.SetFloat("_DiffWeight", value);
            }
        }

        public float Brightness
        {
            get
            {
                return HeightmapMaterial.GetFloat("_Brightness");
            }
            set
            {
                HeightmapMaterial.SetFloat("_Brightness", value);
            }
        }

        public float TriplanarScale
        {
            get
            {
                return HeightmapMaterial.GetTextureScale("_MainTex").x;
            }
            set
            {
                HeightmapMaterial.SetTextureScale("_MainTex", new Vector2(value, value));
            }
        }

        public float HeightScale
        {
            get
            {
                return HeightmapMaterial.GetFloat("_HeightScale");
            }
            set
            {
                HeightmapMaterial.SetFloat("_HeightScale", value);
            }
        }

        public float LineCount
        {
            get
            {
                return HeightmapMaterial.GetFloat("_LineScale");
            }
            set
            {
                HeightmapMaterial.SetFloat("_LineScale", value);
            }
        }

        public float LineSlopeAdjustment
        {
            get
            {
                return HeightmapMaterial.GetFloat("_LineModifier");
            }
            set
            {
                HeightmapMaterial.SetFloat("_LineModifier", value);
            }
        }

        public float LightDirection
        {
            get
            {
                return Sun.transform.rotation.eulerAngles.y;
            }
            set
            {
                Vector3 euler = Sun.transform.rotation.eulerAngles;
                Sun.transform.rotation = Quaternion.Euler(new Vector3(euler.x, value, euler.z));
            }
        }

        public float LightAngle
        {
            get
            {
                return Sun.transform.rotation.eulerAngles.x;
            }
            set
            {
                Vector3 euler = Sun.transform.rotation.eulerAngles;
                Sun.transform.rotation = Quaternion.Euler(new Vector3(value, euler.y, euler.z));
            }
        }
    }
}
