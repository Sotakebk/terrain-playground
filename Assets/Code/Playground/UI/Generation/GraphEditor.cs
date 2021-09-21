using Playground.Generation;
using Playground.IoC;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.UI.Generation
{
    public class GraphEditor : MonoBehaviour, IUIGenerator
    {
        public VisualTreeAsset GraphEditorLayout;

        private BlockFactory blockFactory;
        private TypeColorProvider typeColorProvider;

        private VisualElement graphRoot;
        private Label labelCurrentGraphName;
        private Label labelFeedback;
        private Button buttonClear;
        private VisualElement containerGraphEditor;

        private Graph graph;

        private bool addingConnection;
        private bool isInput;
        private int index;
        private int parameterIndex;

        private void Awake()
        {
            var sc = FindObjectOfType<ServiceCollector>();
            blockFactory = sc.GetService<BlockFactory>();
            typeColorProvider = sc.GetService<TypeColorProvider>();
        }

        public void Construct(VisualElement root)
        {
            GraphEditorLayout.CloneTree(root);
            graphRoot = root.Q<VisualElement>(name: "Container-graphRoot");
            labelCurrentGraphName = graphRoot.Q<Label>(name: "Label-activeGraph");
            labelFeedback = graphRoot.Q<Label>(name: "Label-feedback");
            containerGraphEditor = graphRoot.Q<VisualElement>(name: "Container-graphEditor");
            buttonClear = graphRoot.Q<Button>(name: "Button-clear");

            buttonClear.clicked += () => ButtonCancel();
            buttonClear.visible = false;
            labelFeedback.text = "";
        }

        public void Initialize()
        {
            EditGraph(null);
        }

        public void Disable()
        {
            graphRoot.style.display = DisplayStyle.None;
        }

        public void Enable()
        {
            graphRoot.style.display = DisplayStyle.Flex;
            RebuildGraphView();
        }

        public void EditGraph(Graph graph)
        {
            this.graph = graph;
            if (graph == null)
            {
                labelCurrentGraphName.text = "No graph open.";
            }
            else
            {
                labelCurrentGraphName.text = graph.Name;
            }
            RebuildGraphView();
        }

        public void AddBlock(BlockMetadata metadata)
        {
            if (graph == null)
                return;

            graph.AddBlock(blockFactory.CreateLogicBlock(metadata), metadata);
            RebuildGraphView();
        }

        public void RebuildGraphView()
        {
            containerGraphEditor.Clear();

            if (graph == null)
                return;

            var last = GenerateLine();
            containerGraphEditor.Add(last);

            last.Add(new VisualElement());

            for (int i = 0; i < graph.Blocks.Count; i++)
            {
                last.Add(GenerateBlock(i, true));
                last = GenerateLine();
                containerGraphEditor.Add(last);
                last.Add(GenerateBlock(i, false));
            }

            last.Add(new VisualElement());
        }

        private void ButtonCancel()
        {
        }

        private void CallEdit(int ID)
        {
        }

        private void CallUp(int ID)
        {
            graph.Move(ID, ID + 1);
            RebuildGraphView();
        }

        private void CallDown(int ID)
        {
            graph.Move(ID, ID - 1);
            RebuildGraphView();
        }

        private VisualElement GenerateLine()
        {
            var ve = new VisualElement();
            ve.AddToClassList("graphLine");
            return ve;
        }

        private VisualElement GenerateBlock(int ID, bool inputs)
        {
            var data = graph.Blocks[ID].BlockMetadata;

            var box = new VisualElement();
            box.AddToClassList("block");

            box.Add(GenerateBlockHeader(ID, data, inputs));
            box.Add(GenerateBlockContent(ID, data, inputs));

            return box;
        }

        private VisualElement GenerateBlockHeader(int ID, BlockMetadata data, bool inputs)
        {
            var header = new VisualElement();
            header.AddToClassList("blockHeader");
            var title = new Label(data.Name);
            header.Add(title);

            if (inputs == true)
            {
                var buttonBox = new VisualElement();
                header.Add(buttonBox);
                buttonBox.AddToClassList("blockHeaderButtonGroup");

                var buttonA = new Button();
                buttonA.clicked += () => CallEdit(ID);
                buttonA.text = @"E";
                buttonA.AddToClassList("blockMicroButton");
                buttonBox.Add(buttonA);

                var buttonB = new Button();
                buttonB.clicked += () => CallDown(ID);
                buttonB.text = @"/\";
                buttonB.AddToClassList("blockMicroButton");
                buttonBox.Add(buttonB);

                var buttonC = new Button();
                buttonC.clicked += () => CallUp(ID);
                buttonC.text = @"\/";
                buttonC.AddToClassList("blockMicroButton");
                buttonBox.Add(buttonC);
            }
            return header;
        }

        private VisualElement GenerateBlockContent(int ID, BlockMetadata data, bool inputs)
        {
            var body = new VisualElement();
            var MethodData = data.Methods[graph.Blocks[ID].SelectedMethod];

            ParameterMetadata[] arr;

            if (inputs)
                arr = MethodData.Inputs;
            else
                arr = MethodData.Outputs;

            for (int x = 0; x < arr.Length; x++)
            {
                body.Add(GenerateBlockLine(ID, x, arr[x], inputs));
            }

            if (arr.Length == 0)
            {
                body.Add(new Label("Empty"));
            }

            return body;
        }

        private VisualElement GenerateBlockLine(int ID, int paramID, ParameterMetadata data, bool input)
        {
            var line = new VisualElement();
            line.AddToClassList("blockElement");
            var textContainer = new VisualElement();
            textContainer.AddToClassList("blockElementTextGroup");
            line.Add(textContainer);
            var labelName = new Label(data.Name);
            labelName.AddToClassList("blockTextLabel");
            var labelType = new Label(data.Type.Name);
            labelType.AddToClassList("blockTextLabel");

            var typeColor = typeColorProvider.GetColor(data.Type);

            labelType.style.unityTextOutlineColor = typeColor;
            labelType.style.unityTextOutlineWidth = new StyleFloat(0.05f);

            textContainer.Add(labelName);
            textContainer.Add(labelType);
            line.Add(GenerateDot(ID, paramID, input, typeColor));

            if (input)
                line.style.flexDirection = FlexDirection.RowReverse;

            return line;
        }

        private VisualElement GenerateDot(int ID, int paramID, bool input, Color DotColor)
        {
            var outer = new Button(() => { });
            outer.AddToClassList("blockInOut");
            outer.AddToClassList("blockMicroButton");
            var inner = new VisualElement();
            inner.AddToClassList("blockInOutInner");
            inner.style.backgroundColor = new StyleColor(DotColor);
            outer.Add(inner);
            return outer;
        }
    }
}
