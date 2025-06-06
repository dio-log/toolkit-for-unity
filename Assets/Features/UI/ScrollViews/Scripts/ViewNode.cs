using System;
using Features.Tree.Scripts;

namespace Features.UI.ScrollViews.Scripts
{
    public class ViewNode : Node<ViewNode>
    {
        public string Id { get; }
        public bool IsExpanded { get; set; } = true;
        
        public Action<ViewNode> OnExpanded = delegate { };
        public ViewNode(string id)
        {
            Id = id;
        }
        
    }
}