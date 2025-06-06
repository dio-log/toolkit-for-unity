using System.Collections.Generic;

namespace Features.Tree.Scripts
{
    public class Node
    {
        public int Depth => Parent == null ? 0 : Parent.Depth + 1;
        public Node Parent { get; private set; }
        
        private List<Node> _children = new List<Node>();
        
        public IReadOnlyList<Node> Children => _children.AsReadOnly();

        public void SetParent(Node parent)
        {
            if(Parent != null) Parent.RemoveChild(this);
            
            Parent = parent;

            if (Parent != null) Parent.AddChild(this);
        }

        private void AddChild(Node child)
        {
            _children.Add(child);
        }

        private void RemoveChild(Node node)
        {
            _children.Remove(node);
        }
    }

    public class Node<T> where T : Node<T>
    {
        public int Depth => Parent == null ? 0 : Parent.Depth + 1;
        public T Parent { get; private set; }
        
        private List<T> _children = new ();
        
        public IReadOnlyList<T> Children => _children.AsReadOnly();

        public void SetParent(T parent)
        {
            if(Parent != null) Parent.RemoveChild((T)this);
            
            Parent = parent;

            if (Parent != null) Parent.AddChild((T)this);
        }

        private void AddChild(T child)
        {
            _children.Add(child);
        }

        private void RemoveChild(T node)
        {
            _children.Remove(node);
        }
    }
}