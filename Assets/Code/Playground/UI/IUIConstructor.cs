namespace Playground.UI
{
    /// <summary>
    /// Interface of common methods for UI management.
    /// </summary>
    public interface IUIGenerator
    {
        public bool IsEnabled { get; }

        /// <summary>
        /// Name of the UI component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// May be called only once, initializes variables.
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
