using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This file contains all leaf nodes for animal AI behaviour tree


namespace BehaviourTree
{
    public abstract class LeafNode : Node
    {
        // leaf node reference to individual animal instance
        protected AnimalAI animal;

        // constructor assigns animal instance
        public LeafNode(AnimalAI animalType)
        {
            animal = animalType;
        }

        // overrides Execute() from node but requires definition in derived classes
        public override abstract NodeState Execute();

    }

    /// <summary>
    /// Returns SUCCESS if animal is tamed, else FAILURE
    /// </summary>
    public class CheckIsTamed : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public CheckIsTamed(AnimalAI animalType) : base(animalType) { }

        // define execute logic for each leaf node
        public override NodeState Execute()
        {
            // if animal is tamed, return success, else return failure
            NodeState state = animal.GetIsTamed() ? NodeState.SUCCESS : NodeState.FAILURE;
            Debug.Log("Is Sparrow tamed? " + animal.GetIsTamed());
            return state;
        }
    }

    public class CheckIsStay : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public CheckIsStay(AnimalAI animalType) : base(animalType) { }

        // define execute logic for each leaf node
        public override NodeState Execute()
        {
            // if animal is staying, return success, else return failure
            NodeState state = animal.GetIsStay() ? NodeState.SUCCESS : NodeState.FAILURE;
            Debug.Log("Is Sparrow staying? " + animal.GetIsStay());
            return state;
        }
    }

    public class StayBehaviour : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public StayBehaviour(AnimalAI animalType) : base(animalType) { }

        // define execute logic for each leaf node
        public override NodeState Execute()
        {
            NodeState state = NodeState.SUCCESS;
            return state;
        }
    }


    //public class FightOrFlight : Node
    //{

    //}
}
