using System.Collections.Generic;
using BehaviourTree;

/// <summary>
/// All children must return SUCCESS for this node to return SUCCESS
/// </summary>
public class Sequencer : Node
{
    // list of all child nodes of the sequencer nodes
    List<Node> children = new();

    public override NodeState Execute()
    {
        // return success if all children successful
        // return failure if any child fails

        return NodeState.FAILURE;
    }
}
