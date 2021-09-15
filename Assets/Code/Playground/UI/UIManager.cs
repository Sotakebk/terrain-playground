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
            graphicsUI = GetComponent<GraphicsUI>();
            generationUI = GetComponent<GenerationUI>();
            if (graphicsUI == null || generationUI == null)
                throw new System.NullReferenceException();
        }

        private void Start()
        {
            var root = uidocument.rootVisualElement;
            container = root.Q<VisualElement>(name: "Container-window");

            graphicsUI.Construct(container);
            generationUI.Construct(container);
            graphicsUI.Disable();
            generationUI.Disable();

            Button b = root.Q<Button>(name: "Button-close");
            b.clicked += () =>
            {
                graphicsUI.Disable();
                generationUI.Disable();
            };

            b = root.Q<Button>(name: "Button-display");
            b.clicked += () =>
            {
                graphicsUI.Enable();
                generationUI.Disable();
            };

            b = root.Q<Button>(name: "Button-generation");
            b.clicked += () =>
            {
                graphicsUI.Disable();
                generationUI.Enable();
            };

            graphicsUI.Initialize();
            generationUI.Initialize();
        }
    }
}
