using Playground.Generation;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.UI.Generation
{
    public class AddInspector : MonoBehaviour, IUIGenerator
    {
        public VisualTreeAsset AddInspectorLayout;

        private BlockFactory BlockFactory;
        private GenerationUI GenerationUI;

        private VisualElement addRoot;
        private VisualElement containerBlocks;
        private Label labelName;
        private Label labelDescription;
        private Button buttonAdd;

        private BlockMetadata selectedBlock;

        private void Awake()
        {
            BlockFactory = FindObjectOfType<BlockFactory>();
            GenerationUI = FindObjectOfType<GenerationUI>();
        }

        public void Construct(VisualElement root)
        {
            AddInspectorLayout.CloneTree(root);
            addRoot = root.Q<VisualElement>(name: "Container-AddInspector");

            labelName = addRoot.Q<Label>(name: "Label-name");
            labelDescription = addRoot.Q<Label>(name: "Label-description");
            buttonAdd = addRoot.Q<Button>(name: "Button-add");
            buttonAdd.clicked += () => AddSelectedBlock();

            containerBlocks = addRoot.Q<VisualElement>(name: "Container-blocks");

            foreach (var pair in BlockFactory.BlockMetadata)
            {
                containerBlocks.Add(new Button(() => SetSelectedBlock(pair.Key))
                {
                    text = pair.Value.Name
                });
            }
        }

        public void Initialize()
        {
            SetSelectedBlock(null);
        }

        public void Disable()
        {
            addRoot.style.display = DisplayStyle.None;
            SetSelectedBlock(null);
        }

        public void Enable()
        {
            addRoot.style.display = DisplayStyle.Flex;
        }

        private void SetSelectedBlock(System.Type type)
        {
            if (type == null)
            {
                selectedBlock = null;
                labelName.text = "Nothing selected.";
                labelDescription.text = "Press the buttons below to select a logic block to add.";
                buttonAdd.style.visibility = Visibility.Hidden;
            }
            else
            {
                selectedBlock = BlockFactory.BlockMetadata[type];
                labelName.text = selectedBlock.Name;
                labelDescription.text = selectedBlock.Description;
                if (string.IsNullOrEmpty(labelDescription.text))
                {
                    labelDescription.text = "No description.";
                }
                buttonAdd.style.visibility = Visibility.Visible;
            }
        }

        private void AddSelectedBlock()
        {
            if (selectedBlock != null)
            {
                GenerationUI.AddBlockContext(selectedBlock);
            }
        }
    }
}
