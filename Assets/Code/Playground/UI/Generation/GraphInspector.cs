using Playground.Generation;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.UI.Generation
{
    public class GraphInspector : MonoBehaviour, IUIGenerator
    {
        public VisualTreeAsset GraphInspectorLayout;
        public GenerationManager GenerationManager;

        private GenerationUI GenerationUI;

        private VisualElement graphRoot;
        private Label labelCurrentGraph;
        private VisualElement containerSettings;
        private TextField textFieldName;
        private Button buttonSetName;
        private Button buttonDelete;
        private Button buttonCreateNew;
        private VisualElement containerGraphs;

        private Graph selectedGraph;

        private void Awake()
        {
            GenerationUI = GetComponent<GenerationUI>();
        }

        public void Construct(VisualElement root)
        {
            GraphInspectorLayout.CloneTree(root);
            graphRoot = root.Q<VisualElement>(name: "Container-graphInspector");
            containerSettings = graphRoot.Q<VisualElement>(name: "Container-settings");
            containerGraphs = graphRoot.Q<VisualElement>(name: "Container-graphs");

            labelCurrentGraph = graphRoot.Q<Label>(name: "Label-currentGraph");
            textFieldName = graphRoot.Q<TextField>(name: "TextField-name");

            buttonSetName = graphRoot.Q<Button>(name: "Button-setName");
            buttonCreateNew = graphRoot.Q<Button>(name: "Button-addNew");
            buttonDelete = graphRoot.Q<Button>(name: "Button-delete");

            buttonSetName.clicked += () => SetName();
            buttonCreateNew.clicked += () => CreateNewGraph();
            buttonDelete.clicked += () => DeleteGraph();
        }

        public void Initialize()
        {
            RegenerateGraphList();
            SelectGraph(-1);
        }

        public void Disable()
        {
            graphRoot.style.display = DisplayStyle.None;
        }

        public void Enable()
        {
            graphRoot.style.display = DisplayStyle.Flex;
            RegenerateGraphList();
        }

        private void CreateNewGraph()
        {
            string name = "New graph";
            while (GenerationManager.Graphs.Any((graph) => graph.Name == name))
            {
                name += "+";
            }
            GenerationManager.CreateNewGraph(name);
            RegenerateGraphList();
        }

        private void DeleteGraph()
        {
            if (selectedGraph != null)
            {
                GenerationManager.DeleteGraph(selectedGraph);
                SelectGraph(-1);
                RegenerateGraphList();
            }
        }

        private void RegenerateGraphList()
        {
            containerGraphs.Clear();
            for (int x = 0; x < GenerationManager.Graphs.Count; x++)
            {
                int _x = x;
                containerGraphs.Add(new Button(() => SelectGraph(_x))
                {
                    text = GenerationManager.Graphs[x].Name
                });
            }
        }

        private void SetName()
        {
            if (selectedGraph != null)
            {
                selectedGraph.Name = textFieldName.value;
                labelCurrentGraph.text = selectedGraph.Name;
            }
            RegenerateGraphList();
            GenerationUI.SetGraphContext(selectedGraph);
        }

        private void SelectGraph(int ID)
        {
            if (ID == -1)
            {
                selectedGraph = null;
                containerSettings.style.display = DisplayStyle.None;
                labelCurrentGraph.text = "Nothing active.";
            }
            else
            {
                containerSettings.style.display = DisplayStyle.Flex;
                selectedGraph = GenerationManager.Graphs[ID];
                labelCurrentGraph.text = selectedGraph.Name;
                textFieldName.value = selectedGraph.Name;
            }

            GenerationUI.SetGraphContext(selectedGraph);
        }
    }
}
