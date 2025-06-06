using System.Collections.Generic;
using Features.UI.ScrollViews.Scripts;
using UnityEngine;

namespace Features.UI.ScrollViews.Tests
{
    public class HierarchyScrollViewTester : MonoBehaviour
    {
        [SerializeField] private VirtualHierarchy _virtualHierarchy;
        private void Start()
        {
            var nodes = GetNodes();
            
            foreach (var viewNode in nodes)
            {
                _virtualHierarchy.InsertNode(viewNode);
            }
            
        }

        private IEnumerable<ViewNode> GetNodes()
        {
            var idx = 0;
            while (idx < 50)
            {
                Debug.Log(idx);
                var id = $"node { idx++ }";
                var parent = new ViewNode(id);

                id = $"node { idx++ }";
                var child1 = new ViewNode(id);
                child1.SetParent(parent);
                
                id = $"node { idx++ }";
                var child2 = new ViewNode(id);
                child2.SetParent(child1);
                
                yield return parent;
            }

            yield return new ViewNode($"node { idx }");
        }
    }
}