using Playground.Graphics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playground.UI
{
    public class GraphicsUI : MonoBehaviour
    {
        public GraphicsManager graphicsManager;

        public VisualTreeAsset GraphicsLayout;

        public void Construct(VisualElement root)
        {
            GraphicsLayout.CloneTree(root);

            Slider s = root.Q<Slider>(name: "Slider-sunDirection");
            s.value = graphicsManager.LightDirection;
            s.RegisterValueChangedCallback((state) => { graphicsManager.LightDirection = state.newValue; });
            s = root.Q<Slider>(name: "Slider-sunAngle");
            s.value = graphicsManager.LightAngle;
            s.RegisterValueChangedCallback((state) => { graphicsManager.LightAngle = state.newValue; });
            s = root.Q<Slider>(name: "Slider-diffuse");
            s.value = graphicsManager.DiffuseWeight;
            s.RegisterValueChangedCallback((state) => { graphicsManager.DiffuseWeight = state.newValue; });
            s = root.Q<Slider>(name: "Slider-brightness");
            s.value = graphicsManager.Brightness;
            s.RegisterValueChangedCallback((state) => { graphicsManager.Brightness = state.newValue; });
            s = root.Q<Slider>(name: "Slider-heightScale");
            s.value = graphicsManager.HeightScale;
            s.RegisterValueChangedCallback((state) => { graphicsManager.HeightScale = state.newValue; });
            s = root.Q<Slider>(name: "Slider-triplanarScale");
            s.value = Helper.InverseEXB(graphicsManager.TriplanarScale, -2);
            s.RegisterValueChangedCallback((state) =>
            {
                graphicsManager.TriplanarScale = Helper.EXBCurve(state.newValue, -2);
            });
            s = root.Q<Slider>(name: "Slider-lineCount");
            s.value = graphicsManager.LineCount;
            s.RegisterValueChangedCallback((state) => { graphicsManager.LineCount = state.newValue; });
            s = root.Q<Slider>(name: "Slider-lineSlope");
            s.value = graphicsManager.LineSlopeAdjustment;
            s.RegisterValueChangedCallback((state) => { graphicsManager.LineSlopeAdjustment = state.newValue; });

            var group = root.Q<VisualElement>(name: "Container-gradients");
            for (int i = 0; i < graphicsManager.GradientTextures.Count; i++)
            {
                // ...whaat
                int _i = i;
                var b = new Button(() => graphicsManager.SetGradientTexture(_i));
                b.contentContainer.style.backgroundImage = graphicsManager.GradientTextures[i];
                b.text = graphicsManager.GradientTextures[i].name;
                group.Add(b);
            }

            group = root.Q<VisualElement>(name: "Container-lines");
            for (int i = 0; i < graphicsManager.LineTextures.Count; i++)
            {
                // this thing scares me
                int _i = i;
                var b = new Button(() => graphicsManager.SetLineTexture(_i));
                b.contentContainer.style.backgroundImage = graphicsManager.LineTextures[i];
                b.text = graphicsManager.LineTextures[i].name;
                group.Add(b);
            }
        }
    }
}
