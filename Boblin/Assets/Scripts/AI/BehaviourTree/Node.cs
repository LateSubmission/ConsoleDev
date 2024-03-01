using System.Collections.Generic;

// THE CODE BELOW USES THIRD PARTY CODE - YouTube video: Create an AI with behaviour trees [Unity/C# tutorial] by Mina Pêcheux
// Available here: https://www.youtube.com/watch?v=aR6wt5BlE-E&t=21s

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
        protected NodeState state;

        // this node's child node(s)
        protected List<Node> children = new();
        // this node's parent node
        public Node parent;

        // default constructor
        public Node()
        {
            parent = null;
        }

        // constructor for a node with children
        public Node(List<Node> children)
        {
            AssignChildren(children);
        }

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

        // abstract Execute function forces derived classes to implement this behaviour
        public abstract NodeState Execute();
    }
}
