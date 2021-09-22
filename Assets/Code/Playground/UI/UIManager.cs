using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Playground.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private RectTransform ButtonPanel;

        [SerializeField]
        private RectTransform OuterContainer;

        [SerializeField]
        private GameObject TopButtonPrefab;

        [SerializeField]
        private GameObject[] Panels;

        private IUIGenerator[] InstantiatedPanels;

        private void Awake()
        {
            if (Panels == null || TopButtonPrefab == null ||
                ButtonPanel == null || OuterContainer == null)
                throw new System.NullReferenceException();
        }

        private void Start()
        {
            List<IUIGenerator> UIGenerators = new List<IUIGenerator>();

            foreach (GameObject go in Panels)
            {
                if (go.GetComponent<IUIGenerator>() == null)
                {
                    Debug.LogWarning($"Prefab {go.name} lacks a UIGenerator component!");
                    continue;
                }

                var obj = Instantiate(go, OuterContainer);

                UIGenerators.Add(obj.GetComponent<IUIGenerator>());
            }

            InstantiatedPanels = UIGenerators.ToArray();

            for (int i = 0; i < InstantiatedPanels.Length; i++)
            {
                var _i = i;
                InstantiatedPanels[i].Initialize();
                var obj = Instantiate(TopButtonPrefab, ButtonPanel);

                obj.GetComponent<Button>().onClick.AddListener(() => SetActive(_i));
                obj.GetComponentInChildren<Text>().text = InstantiatedPanels[i].Name;
            }

            SetActive(-1);
        }

        public void SetActive(int index)
        {
            Debug.Log($"SetActive({index})");
            foreach (var ui in InstantiatedPanels)
            {
                ui.Disable();
            }
            if (index >= 0 && index < InstantiatedPanels.Length)
                InstantiatedPanels[index].Enable();
        }
    }
}
