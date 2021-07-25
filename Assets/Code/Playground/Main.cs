using UnityEngine;

namespace Playground
{
    public class Main : MonoBehaviour
    {
        public Transform MapParent;
        public DisplayManager displayManager;

        public void Start()
        {
            var hgen = new HeightmapGenerator();

            var h = hgen.GenerateHeightmap();

            displayManager.ApplyHeightmap(h);
        }
    }
}
