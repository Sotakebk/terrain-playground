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
            var srv = (T)collectedServices.First((obj) => obj is T);
            if (srv != null)
                return srv;

            // in case something was not caught on Awake...
            var obj = FindObjectsOfType<MonoBehaviour>().Where((obj) => obj is T).First();
            if (obj == null)
                return null;

            var old = collectedServices;
            collectedServices = new MonoBehaviour[collectedServices.Length + 1];
            old.CopyTo(collectedServices, 0);
            collectedServices[collectedServices.Length - 1] = obj;

            return (T)obj;
        }
    }
}
