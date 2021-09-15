using System.Collections.Generic;
using UnityEngine;

namespace Playground.Generation
{
    public class GenerationManager : MonoBehaviour
    {
        private List<BasicGraph> graphs;

        public IReadOnlyList<BasicGraph> Graphs => graphs.AsReadOnly();

        public void CreateNewGraph(string Name)
        {
            graphs.Add(new BasicGraph()
            {
                Name = Name
            });
        }

        public void DeleteGraph(Graph graph)
        {
            graphs.RemoveAll((_graph) => ReferenceEquals(_graph, graph));
        }

        public void ExecuteGraph(string Name)
        {
            graphs.Find((graph) => graph.Name == Name).Execute();
        }

        // TODO serialize, deserialize?

        private void Awake()
        {
            graphs = new List<BasicGraph>();
        }
    }
}
