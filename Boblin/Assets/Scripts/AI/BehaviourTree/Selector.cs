using System.Collections.Generic;
using BehaviourTree;

/// <summary>
/// Only one child must return SUCCESS for this node to return SUCCESS
/// </summary>
public class Selector : Node
{
    // list of all child nodes of the sequencer nodes
    List<Node> children = new();

    public override NodeState Execute()
    {
        // return success if any children successful
        // return failure if all children fail

        return NodeState.FAILURE;
    }
}
