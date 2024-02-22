using System.Collections.Generic;

// namespace to hold all behaviour tree-related code
namespace BehaviourTree
{
    // refers to the current state of a node, determines flow of behaviour tree
    public enum NodeState
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    // ABSTRACT CLASS
    public abstract class Node
    {
        // this node's current state
        private NodeState state;

        // this node's child nodes
        private List<Node> children = new();
        // this node's parent node
        public Node parent;

        // default constructor
        public Node()
        {
        }

        // constructor for a node with a parent
        public Node(Node parentNode)
        {
            // assigns parent of this node at construction
            parent = parentNode;
        }

        // abstract Execute function forces derived classes to implement this behaviour
        public abstract NodeState Execute();

        /// <summary>
        /// Assign node(s) as a child of this node
        /// </summary>
        /// <param name="childNodes">A list of nodes to be added as children</param>
        public void AssignChildren(List<Node> childNodes)
        {
            foreach (Node child in childNodes)
            {
                // make this node the parent of each of the provided nodes
                child.parent = this;
            }
            // add the child nodes to this node's list of children
            children.AddRange(childNodes);
        }
    }
}
