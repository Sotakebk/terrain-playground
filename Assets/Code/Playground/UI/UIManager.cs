using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.UI
{
    [RequireComponent(typeof(GraphicsUI))]
    [RequireComponent(typeof(GenerationUI))]
    public class UIManager : MonoBehaviour
    {
        public UIDocument uidocument;

        private GraphicsUI graphicsUI;
        private GenerationUI generationUI;
        private VisualElement container;

        private void Awake()
        {
            graphicsUI = transform.GetComponent<GraphicsUI>();
            generationUI = transform.GetComponent<GenerationUI>();
            if (graphicsUI == null || generationUI == null)
                throw new System.NullReferenceException();
        }

        private void Start()
        {
            var root = uidocument.rootVisualElement;
            container = root.Q<VisualElement>(name: "Container-window");

            Button b = root.Q<Button>(name: "Button-close");
            b.clicked += () =>
            {
                RemoveNestedUI();
            };

            b = root.Q<Button>(name: "Button-display");
            b.clicked += () =>
            {
                RemoveNestedUI();
                graphicsUI.Construct(container);
            };

            b = root.Q<Button>(name: "Button-generation");
            b.clicked += () =>
            {
                RemoveNestedUI();
                generationUI.Construct(container);
            };
        }

        private void RemoveNestedUI()
        {
            container.Clear();
        }
    }
}
