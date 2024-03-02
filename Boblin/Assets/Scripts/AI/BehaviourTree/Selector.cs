using System.Collections.Generic;
using BehaviourTree;

/// <summary>
/// Only one child must return SUCCESS for this node to return SUCCESS
/// </summary>
public class Selector : Node
{
    public Selector() : base() { }

    public Selector(List<Node> children) : base(children) { }
    
    // allows for random selection of children
    public Selector(bool isRandom, List<Node> children) : base(children) { }

    public override NodeState Execute()
    {
        // return success if any children successful
        // return failure if all children fail
        
        foreach (Node child in children)
        {
            switch(child.Execute())
            {
                case NodeState.RUNNING:
                    state = NodeState.RUNNING;
                    return state;
                case NodeState.SUCCESS:
                    state = NodeState.SUCCESS;
                    return state;
                case NodeState.FAILURE:
                    continue;
                default:
                    continue;
            }
        }
        // has not already returned at running or success, therefore must have failed
        return NodeState.FAILURE;
    }
}
