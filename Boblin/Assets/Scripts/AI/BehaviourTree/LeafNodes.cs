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

    public class WaitForPlayer : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public WaitForPlayer(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // stay in place
                // animalType.Stay()?
            // wait for player to do something
                // check if food in radius? or just do nothing
            // idle animation

            return NodeState.SUCCESS;
        }
    }

    public class Wander : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public Wander(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // Wander steering behaviour.
                // Could have an array of points on the map to seek
                // Could seek a point in a circle in front of AI

            return NodeState.SUCCESS;
        }
    }

    public class Rest : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public Rest(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // sit animation, no other tasks

            return NodeState.SUCCESS;
        }
    }

    public class Search : LeafNode
    {
        string foodStr;

        // use constructor from LeafNode, passing in animalType
        public Search(AnimalAI animalType) : base(animalType)
        {
            foodStr = animalType.foodBest;
        }

        // determines state of this node
        public override NodeState Execute()
        {
            // searching for provided food

            return NodeState.SUCCESS;
        }
    }

    public class CheckHasFood : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public CheckHasFood(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // if animal already has a target object
            if (animal.foodTarget != null) return NodeState.SUCCESS;

            // to store distance to each food object in loop
            float distToFood;
            // store distance to closest food object
            float closestFoodDist = animal.safeDist;
            // store current nearest food object locally
            Food target = null;

            // check every food GO in the game world
            // make this more efficient rather than checking every food object in game world
            // could use BVH to find food within x square metres and add to list of nearby food?
            // Only run every x seconds to save on processing power?
            foreach (Food food in WorldStates.foodItems)
            {
                // distance between player and food
                distToFood = Vector3.Distance(animal.transform.position, food.position);

                // if the food GO is close enough, add it to the nearby list
                if (distToFood < animal.safeDist)
                {
                    // if target not already set or this food is closer than others
                    if (target == null || distToFood < closestFoodDist)
                    {
                        // set target to food obj in current iteration
                        target = food;
                        // set closest food to calculated dist in current iteration
                        closestFoodDist = distToFood;
                    }
                }
            }
            // assign this animal's food target
            animal.foodTarget = target;

            // if there is food nearby, return success
            if (target != null) return NodeState.SUCCESS;
            // else fail
            return NodeState.FAILURE;
        }
    }

    public class EatFood : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public EatFood(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // if still moving towards food
            if (animal.navAgent.hasPath) return NodeState.RUNNING;


            // when near enough to food
            if (animal.navAgent.remainingDistance <= animal.navAgent.stoppingDistance)
            {
                animal.navAgent.ResetPath();
            }
            else  // if doesn't have path and not close to food
            {
                // get local reference to food position
                Vector3 foodPos = animal.foodTarget.position;
                // set nav agent's destination to food position
                animal.navAgent.SetDestination(foodPos);
            }
            
            // if food eaten, unassign food target
            animal.foodTarget = null;
            // return success as food has been eaten
            return NodeState.SUCCESS;
        }
    }
}
