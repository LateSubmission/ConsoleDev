using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This file contains all leaf nodes for animal AI behaviour tree


namespace BehaviourTree
{
    /// <summary>
    /// Returns SUCCESS if animal is tamed, else FAILURE
    /// </summary>
    public class CheckIsTamed : Node
    {
        // reference to animal
        AnimalAI animal;

        public CheckIsTamed(AnimalAI animalType) 
        {
            // let the program know which animal to check
            animal = animalType;
        }

        public override NodeState Execute()
        {
            // if animal is tamed, return success, else return failure
            NodeState state = animal.GetIsTamed() ? NodeState.SUCCESS : NodeState.FAILURE;
            Debug.Log("Is Sparrow tamed? " + animal.GetIsTamed());
            return state;            
        }
    }


    //public class FightOrFlight : Node
    //{

    //}
}
