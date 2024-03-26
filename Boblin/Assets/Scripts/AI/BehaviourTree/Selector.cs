using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

/// <summary>
/// Only one child must return SUCCESS for this node to return SUCCESS
/// </summary>
public class Selector : Node
{
    private readonly bool isRandom;

    public Selector() : base() { }

    //public Selector(List<Node> children) : base(children) { }

    // allows for random selection of children
    public Selector(bool _isRandom, List<Node> children) : base(children) 
    {
        isRandom = _isRandom;
    }

    // return success if any children successful
    // return failure if all children fail
    public override NodeState Execute()
    {
        // randomise order of children every time the selector is executed
        if (isRandom) RandomiseList(children);

        foreach (Node child in children)
        {
            switch (child.Execute())
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

    public void RandomiseList(List<Node> children)
    {
        // cache original list
        List<Node> temp = new();
        temp.AddRange(children);

        for (int i = 0; i < children.Count; i++)
        {
            // want a random child in cached list
            int index = Random.Range(0, temp.Count);
            // update next node in children with randomised child
            children[i] = temp[index];
            // child cannot be used again in another iteration
            temp.RemoveAt(index);
        }
    }
}
