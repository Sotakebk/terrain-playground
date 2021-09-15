using UnityEngine.UIElements;

namespace Playground.UI
{
    /// <summary>
    /// Interface of common methods for UI management.
    /// </summary>
    public interface IUIGenerator
    {
        /// <summary>
        /// May be called only once, initializes UI variables.
        /// </summary>
        /// <param name="root">Root VisualElement of this UI component.</param>
        public void Construct(VisualElement root);

        /// <summary>
        /// May be called only once, initializes other variables, might depend on other UI components.
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Called any time, is expected to enable or make visible any UI components
        /// </summary>
        public void Enable();

        /// <summary>
        /// Called any time, is expected to disable or hide any UI components
        /// </summary>
        public void Disable();
    }
}
