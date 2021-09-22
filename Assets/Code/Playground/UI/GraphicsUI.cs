using Playground.Graphics;
using Playground.IoC;
using UnityEngine;

namespace Playground.UI
{
    public class GraphicsUI : MonoBehaviour, IUIGenerator
    {
        public bool IsEnabled => gameObject.activeSelf;

        public string Name => "Graphics Settings";

        private ServiceCollector ServiceCollector;
        private GraphicsManager GraphicsManager;

        public void Initialize()
        {
            ServiceCollector = FindObjectOfType<ServiceCollector>();
            GraphicsManager = ServiceCollector.GetService<GraphicsManager>();
        }

        public void Construct()
        {
        }

        public void Disable()
        {
            Debug.Log($"Disable this: {this.gameObject.name}");
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            Debug.Log($"Enable this: {this.gameObject.name}");
            gameObject.SetActive(true);
        }
    }
}
