using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace Playground.UI
{
    [RequireComponent(typeof(GraphicsUI))]
    public class UIManager : MonoBehaviour
    {
        public UIDocument uidocument;

        [SerializeField]
        private Object[] UIGenerators;

        private IUIGenerator[] UI;

        private void Awake()
        {
            UI = UIGenerators.OfType<IUIGenerator>().Distinct().ToArray();

            if (UI == null)
                throw new System.NullReferenceException();
        }

        private void Start()
        {
            var root = uidocument.rootVisualElement;
            var container = root.Q<VisualElement>(name: "Container-window");
            var buttonContainer = root.Q<VisualElement>(name: "Container-buttons");

            root.Q<Button>(name: "Button-closeAll").clicked += () => ButtonPressClear();

            for (int x = 0; x < UI.Length; x++)
            {
                var _x = x;

                var button = new Button(() => ButtonPress(_x));
                button.text = UI[x].Name;
                button.AddToClassList("smallButton");
                buttonContainer.Add(button);

                UI[x].Construct(container);
            }

            for (int x = 0; x < UI.Length; x++)
                UI[x].Initialize();

            for (int x = 0; x < UI.Length; x++)
                UI[x].Disable();
        }

        private void ButtonPress(int index)
        {
            if (UI[index].IsEnabled)
                UI[index].Disable();
            else
                UI[index].Enable();
        }

        private void ButtonPressClear()
        {
            for (int x = 0; x < UI.Length; x++)
                UI[x].Disable();
        }
    }
}
