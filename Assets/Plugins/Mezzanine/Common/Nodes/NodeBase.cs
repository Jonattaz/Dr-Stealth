using System;
using System.Collections.Generic;
using Mz.Identifiers;

namespace Mz.Nodes
{
    public delegate void NodeAttachedEventHandler(object node);
    public delegate void NodeDetachedEventHandler(object node, object parentPrevious);
    
    public abstract class NodeBase<TNode, TRoot> : INode<TNode, TRoot>
        where TNode : NodeBase<TNode, TRoot>
        where TRoot : class, TNode
    {
        protected NodeBase(
            object key = null,
            bool isRoot = false
        )
        {
            Key = key ?? Identifier.Get();
            IsRoot = isRoot;
        }

        //===== Properties
        
        public object Key { get; private set; }
        public TNode Parent { get; private set; }

        protected void SetParent(TNode value)
        {
            var parentPrevious = Parent;
            Parent = value;

            if (Parent != default)
            {
                var root = Parent.Root;
                if (root != default) Attached?.Invoke((TNode) this);
                IsRoot = false;
            }
            else
            {
                Detached?.Invoke((TNode) this, parentPrevious);
            }
        }

        public TNode NextSibling { get; protected set; }
        public TNode PreviousSibling { get; protected set; }
        public TNode FirstChild { get; protected set; }
        public TNode LastChild { get; protected set; }

        public bool IsRoot { get; protected set; }

        public TRoot Root
        {
            get
            {
                if (IsRoot) return (TRoot)this;
                var parent = Parent;
                while (parent?.Parent != null) parent = parent.Parent;
                return parent != null && parent.IsRoot ? (TRoot) parent : default;
            }
        }

        public bool IsCollection => LastChild != null;

        public string Path
        {
            get
            {
                var path = "";
                const string pathSegmentDelimiter = "/";
                var node = Parent;

                while (node != null)
                {
                    if (node.IsRoot) path = node.Key + path;
                    else path = pathSegmentDelimiter + node.Key + path;
                    node = node.Parent;
                }

                return path;
            }
        }

        public List<TNode> Children
        {
            get
            {
                GetChildren(out var nodes);
                return nodes;
            }
        }

        //===== Events

        public event NodeAttachedEventHandler Attached;
        public event NodeDetachedEventHandler Detached;

        public void GetChildren(out List<TNode> nodes)
        {
            nodes = new List<TNode>();

            var nodeParent = (TNode) this;
            if (nodeParent.LastChild == null) return;
            var node = nodeParent.FirstChild;
            nodes.Add(node);
            do
            {
                node = node.NextSibling;
                if (node != null) nodes.Add(node);
            } while (node != null);
        }

        public void Walk(Func<TNode, bool> action)
        {
            foreach (var node in Children)
            {
                node.Walk(action);
                if (!action(node)) break;
            }
        }

        public TNode GetFirstSibling()
        {
            var node = this;
            while (node.PreviousSibling != null) node = node.PreviousSibling;
            return (TNode)node;
        }

        public List<TNode> Unravel(List<TNode> list = null)
        {
            if (list == null) list = new List<TNode>();

            var firstChild = FirstChild;
            var nextSibling = NextSibling;
            LastChild = null;
            NextSibling = null;

            list.Add((TNode) this);
            if (firstChild != null) list = firstChild.Unravel(list);
            if (nextSibling != null) list = nextSibling.Unravel(list);

            return list;
        }

        protected TNode _Add(TNode node)
        {
            if (node == null) return default;

            if (LastChild == null)
            {
                FirstChild = node;
                node.PreviousSibling = null;
            }
            else
            {
                LastChild.NextSibling = node;
                node.PreviousSibling = LastChild;
            }

            node.NextSibling = null;
            node.SetParent((TNode) this);
            LastChild = node;
            
            OnAdd(node);

            return node;
        }
        
        protected virtual void OnAdd(TNode node) {}

        public TNode RemoveByKey(object key)
        {
            var node = GetByKey(key);
            if (node == null) return default;

            if (node == FirstChild && node == LastChild)
            {
                FirstChild = null;
                LastChild = null;
            }
            else if (node == LastChild)
            {
                LastChild = node.PreviousSibling;
                if (LastChild != null) LastChild.NextSibling = null;
            }
            else if (node == FirstChild)
            {
                FirstChild = node.NextSibling;
                if (FirstChild != null) FirstChild.PreviousSibling = null;
            }
            else
            {
                var previous = node.PreviousSibling;
                var next = node.NextSibling;
                previous.NextSibling = next;
                next.PreviousSibling = previous;
            }

            node.PreviousSibling = null;
            node.NextSibling = null;

            return node;
        }

        public TNode GetByKey(object key)
        {
            var node = FirstChild;

            while (node != null && node.NextSibling != FirstChild)
            {
                // == comparison doesn't work here.
                if (Equals(node.Key, key)) break;
                node = node.NextSibling;
            }

            if (!Equals(node.Key, key)) node = default;

            return node;
        }

        public TNode Get(string path, char pathSegmentDelimiter = '/')
        {
            if (string.IsNullOrEmpty(path)) return default;

            var nodeParent = (TNode) this;
            var child = default(TNode);
            var segements = path.Split(pathSegmentDelimiter);

            foreach (var segment in segements)
            {
                if (nodeParent == default) break;
                child = nodeParent.GetByKey(segment);
                nodeParent = child;
            }

            return child;
        }

        /// <summary>
        /// The default join behaviour for two nodes is to chain them as siblings and return the right node.
        /// </summary>
        public virtual TNode Join(TNode rightNode)
        {
            rightNode.NextSibling = NextSibling;
            rightNode.PreviousSibling = (TNode) this;
            NextSibling = rightNode;
            if (Parent == null) return rightNode;
            if (this == Parent.LastChild) Parent.LastChild = rightNode;
            rightNode.SetParent(Parent);
            return rightNode;
        }

        // Operator overloads

        /// <summary>
        /// Join two nodes, the first node decides how by overriding Join
        /// </summary>
        public static TNode operator |(NodeBase<TNode, TRoot> leftNode, TNode rightNode)
        {
            return leftNode.Join(rightNode);
        }

        public TNode this[int index]
        {
            get
            {
                GetChildren(out var children);
                return children[index];
            }
        }

        public TNode this[string key] => GetByKey(key);
    } // End class
}