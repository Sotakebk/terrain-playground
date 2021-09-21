using System.Linq;
using UnityEngine;

namespace Playground.IoC
{
    public class ServiceCollector : MonoBehaviour, IService
    {
        [SerializeField]
        private MonoBehaviour[] collectedServices;

        private void Awake()
        {
            collectedServices = FindObjectsOfType<MonoBehaviour>();
            collectedServices = collectedServices.Where((obj) => obj is IService).Distinct().ToArray();
        }

        public T GetService<T>() where T : MonoBehaviour, IService
        {
            return (T)collectedServices.First((obj) => obj is T);
        }
    }
}
