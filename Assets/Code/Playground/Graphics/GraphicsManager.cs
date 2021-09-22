using Playground.IoC;
using System.Collections.Generic;
using UnityEngine;

namespace Playground.Graphics
{
    [RequireComponent(typeof(MeshGenerator))]
    public class GraphicsManager : MonoBehaviour, IService
    {
        [SerializeField]
        private Material OriginalMaterial;

        [SerializeField]
        private Light Sun;

        [SerializeField]
        private List<Texture2D> gradientTextures;

        [SerializeField]
        private List<Texture2D> lineTextures;

        private List<Texture2D> heightmapTextures;

        public Material HeightmapMaterial { get; private set; }
        public IReadOnlyList<Texture2D> GradientTextures => gradientTextures;
        public IReadOnlyList<Texture2D> LineTextures => lineTextures;
        public IReadOnlyList<Texture2D> HeightmapTextures => heightmapTextures;

        private MeshGenerator meshGenerator;

        private void Awake()
        {
            meshGenerator = transform.GetComponent<MeshGenerator>();

            HeightmapMaterial = new Material(OriginalMaterial);
            heightmapTextures = new List<Texture2D>();

            if (!SystemInfo.SupportsTextureFormat(TextureFormat.RFloat))
                throw new System.Exception("SystemInfo.SupportsTextureFormat(TextureFormat.RFloat) = FALSE");
            if (meshGenerator == null)
                throw new System.NullReferenceException();

            meshGenerator.Initialize();
        }

        public void AddHeightmap(Heightmap heightmap, string submittedName)
        {
            Texture2D heightmapTexture = heightmapTextures.Find((tex) => tex.name == submittedName);
            bool exists = (heightmap != null);

            if (exists && heightmap.Size != heightmapTexture.width)
            {
                Destroy(heightmapTexture);
                exists = false;
            }

            if (heightmapTexture == null)
            {
                heightmapTexture = new Texture2D(heightmap.Size, heightmap.Size, TextureFormat.RFloat, false, true)
                {
                    name = submittedName,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Point,
                    anisoLevel = 0
                };
            }

            heightmapTexture.SetPixelData(heightmap.Data, 0);
            heightmapTexture.Apply();

            if (!exists)
                heightmapTextures.Add(heightmapTexture);
        }

        // material settings

        public void SetActiveHeightmap(string name)
        {
            var texture = heightmapTextures.Find((tex) => tex.name == name);
            if (texture != null)
                SetHeightTexture(texture);
        }

        private void SetHeightTexture(Texture2D texture)
        {
            HeightmapMaterial.SetTexture("_HeightTex", texture);
            meshGenerator.ResizeMesh(texture.width);
        }

        public void RemoveHeightmap(string name)
        {
            var tex = heightmapTextures.Find((tex) => tex.name == name);
            heightmapTextures.Remove(tex);
            Destroy(tex);
        }

        public void SetGradientTexture(int index)
        {
            HeightmapMaterial.SetTexture("_GradientTex", GradientTextures[index]);
        }

        public void SetLineTexture(int index)
        {
            HeightmapMaterial.SetTexture("_LineTex", LineTextures[index]);
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
