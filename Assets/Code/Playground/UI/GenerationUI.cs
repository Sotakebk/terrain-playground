using UnityEngine;
using UnityEngine.UIElements;
using Playground.Generation;

namespace Playground.UI
{
    public class GenerationUI : MonoBehaviour
    {
        public GenerationManager generationManager;

        public VisualTreeAsset GenerationLayout;

        public void Construct(VisualElement root)
        {
            GenerationLayout.CloneTree(root);

            var b = root.Q<Button>(name: "Button-generate");
            b.clicked += () =>
            {
                generationManager.Generate();
            };
        }
    }
}
