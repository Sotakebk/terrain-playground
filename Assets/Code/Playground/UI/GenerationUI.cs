using Playground.Generation;
using Playground.UI.Generation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.UI
{
    [RequireComponent(typeof(GraphInspector))]
    [RequireComponent(typeof(AddInspector))]
    [RequireComponent(typeof(BlockInspector))]
    [RequireComponent(typeof(GraphEditor))]
    public class GenerationUI : MonoBehaviour, IUIGenerator
    {
        public VisualTreeAsset GenerationLayout;
        public GenerationManager GenerationManager;

        private GraphInspector GraphInspector;
        private AddInspector AddInspector;
        private BlockInspector BlockInspector;
        private GraphEditor GraphEditor;

        private VisualElement generationRoot;
        private VisualElement inspectorRoot;
        private VisualElement graphRoot;

        public void Construct(VisualElement root)
        {
            GenerationLayout.CloneTree(root);
            generationRoot = root.Q<VisualElement>(name: "Container-generation");
            graphRoot = root.Q<VisualElement>(name: "Container-graph");
            inspectorRoot = root.Q<VisualElement>(name: "Container-inspector");

            GraphInspector = GetComponent<GraphInspector>();
            AddInspector = GetComponent<AddInspector>();
            BlockInspector = GetComponent<BlockInspector>();
            GraphEditor = GetComponent<GraphEditor>();

            GraphInspector.Construct(inspectorRoot);
            AddInspector.Construct(inspectorRoot);
            BlockInspector.Construct(inspectorRoot);
            GraphEditor.Construct(graphRoot);

            // register basic buttons
            var b = root.Q<Button>(name: "Button-add");
            b.clicked += () =>
            {
                GraphInspector.Disable();
                AddInspector.Enable();
                BlockInspector.Disable();
            };

            b = root.Q<Button>(name: "Button-graphs");
            b.clicked += () =>
            {
                GraphInspector.Enable();
                AddInspector.Disable();
                BlockInspector.Disable();
            };

            b = root.Q<Button>(name: "Button-execute");
            b.clicked += () => { };
        }

        public void Disable()
        {
            generationRoot.style.display = DisplayStyle.None;
        }

        public void Enable()
        {
            generationRoot.style.display = DisplayStyle.Flex;
        }

        public void Initialize()
        {
            GraphInspector.Disable();
            AddInspector.Disable();
            BlockInspector.Disable();
            GraphEditor.Disable();

            GraphInspector.Initialize();
            AddInspector.Initialize();
            BlockInspector.Initialize();
            GraphEditor.Initialize();

            GraphEditor.Enable();
        }

        public void SetGraphContext(Graph graph)
        {
            GraphEditor.EditGraph(graph);
        }

        public void SetBlockContext(BlockContainer block)
        {
        }

        public void AddBlockContext(BlockMetadata metadata)
        {
            GraphEditor.AddBlock(metadata);
        }
    }
}
