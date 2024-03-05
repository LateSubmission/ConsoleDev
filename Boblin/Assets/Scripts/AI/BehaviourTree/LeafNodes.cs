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

        // reference to player
        protected GameObject player;

        // constructor assigns animal instance
        public LeafNode(AnimalAI animalType)
        {
            animal = animalType;
            player = animal.player;
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

        // determines state of this node
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
        public CheckIsStay(AnimalAI animalType) : base (animalType) { }

        // determines state of this node
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

        // determines state of this node
        public override NodeState Execute()
        {
            NodeState state = NodeState.SUCCESS;
            return state;
        }
    }

    public class IsPlayerNear : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public IsPlayerNear(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            Vector3 playerPos = player.transform.position;
            if (Vector3.Distance(playerPos, animal.transform.position) > animal.safeDist)
            {
                // return true 
            }

            NodeState state = NodeState.SUCCESS;
            return state;
        }
    }

    public class CheckIsThreatened : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public CheckIsThreatened(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // if animal is staying, return success, else return failure
            NodeState state = animal.GetIsThreatened() ? NodeState.SUCCESS : NodeState.FAILURE;
            Debug.Log("Is Sparrow threatened? " + animal.GetIsThreatened());
            return state;
        }
    }


    public class FightOrFlight : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public FightOrFlight(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            animal.FightOrFlight(animal);

            return NodeState.SUCCESS;
        }
    }
}
