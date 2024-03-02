using System.Collections.Generic;

namespace BehaviourTree
{
    /// <summary>
    /// All children must return SUCCESS for this node to return SUCCESS (acts like a logical AND operator)
    /// </summary>
    public class Sequencer : Node
    {
        // construct with no list of children
        public Sequencer() : base() { }

        // construct with list of children
        public Sequencer(List<Node> children) : base(children) { }


        public override NodeState Execute()
        {
            // return success if all children successful
            // return failure if any child fails

            // to check if any child nodes are still running
            bool anyChildIsRunning = false;

            foreach (Node child in children)
            {
                switch (child.Execute())
                {
                    case NodeState.RUNNING:
                        // make a note that a child is running
                        anyChildIsRunning = true;
                        // continue to next child
                        continue;
                    case NodeState.SUCCESS:
                        // this node succeeded, so continue to next child
                        continue;
                    case NodeState.FAILURE:
                        // this node failed so must return failure immediately
                        state = NodeState.FAILURE;
                        return state;
                    default:
                        // assume success
                        continue;
                }
            }
            // if a child is still running, state is running, otherwise state is success
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            // return state
            return state;
        }
    }
}
