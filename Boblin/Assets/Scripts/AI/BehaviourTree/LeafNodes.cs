using Unity.VisualScripting;
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
            //Debug.Log("Is Sparrow tamed? " + animal.GetIsTamed());
            return state;
        }
    }

    /// <summary>
    /// Returns SUCCESS if animal has been commanded to stay
    /// </summary>
    public class CheckIsStay : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public CheckIsStay(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // if animal is staying, return success, else return failure
            NodeState state = animal.GetIsStay() ? NodeState.SUCCESS : NodeState.FAILURE;
            //Debug.Log("Is Sparrow staying? " + animal.GetIsStay());
            return state;
        }
    }

    /// <summary>
    /// Animal performs its defined stay behaviour
    /// </summary>
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

    /// <summary>
    /// Returns SUCCESS if player is within this animal's safe distance
    /// </summary>
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

    /// <summary>
    /// Returns SUCCESS if animal has been threatened
    /// </summary>
    public class CheckIsThreatened : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public CheckIsThreatened(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            // if animal is staying, return success, else return failure
            NodeState state = animal.GetIsThreatened() ? NodeState.SUCCESS : NodeState.FAILURE;
            //Debug.Log("Is Sparrow threatened? " + animal.GetIsThreatened());
            return state;
        }
    }

    /// <summary>
    /// Animal either becomes aggressive or flees depending on their aggression level
    /// </summary>
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

    /// <summary>
    /// Defines behaviour of animal while waiting for player to trigger another behaviour
    /// </summary>
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

    /// <summary>
    /// Wander steering behaviour with no specific aim
    /// </summary>
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

    /// <summary>
    /// Animal behaviour while resting, different to stay behaviour
    /// </summary>
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

    /// <summary>
    /// Behaviour to search for food
    /// </summary>
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

    /// <summary>
    /// Returns SUCCESS if animal has a piece of food they are moving towards, otherwise there is no food nearby
    /// </summary>
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

    /// <summary>
    /// Behaviour for animal to eat food
    /// </summary>
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

            animal.foodEaten = animal.foodTarget;
            // if food eaten, unassign food target
            animal.foodTarget = null;
            // return success as food has been eaten
            return NodeState.SUCCESS;
        }
    }

    /// <summary>
    /// Check what effect the food should have on the animal (positive, negative, death, etc.)
    /// </summary>
    public class CheckFoodEffect : LeafNode
    {
        Food currentFood;

        // use constructor from LeafNode, passing in animalType
        public CheckFoodEffect(AnimalAI animalType) : base(animalType)
        {
            // store current food locally
            currentFood = animal.foodEaten;
        }

        // determines state of this node
        public override NodeState Execute()
        {
            // if the current food is the animal's favourite
            if (currentFood.type == animal.foodBest)
            {
                // set the food's effect on the animal to "like"
                animal.foodEffect = AnimalAI.FoodEffect.like;
            }
            // if the current food is the animal's _least_ favourite
            else if (currentFood.type == animal.foodWorst)
            {
                // set the food's effect on the animal to "dislike"
                animal.foodEffect = AnimalAI.FoodEffect.dislike;
            }
            // if the current food is poisonous to the animal
            else if (currentFood.type == animal.foodPoisonous)
            {
                // set the food's effect on the animal to "poisoned"
                animal.foodEffect = AnimalAI.FoodEffect.poisoned;
            }
            // if food is _not_ in any above category, it has no specific effect on the animal
            else
            {
                // set the food's effect on the animal to "neutral"
                animal.foodEffect = AnimalAI.FoodEffect.neutral;
            }

            // effect on animal has been adjusted, return success
            return NodeState.SUCCESS;
        }
    }

    /// <summary>
    /// Affect animal based on result of CheckFoodEffect node
    /// </summary>
    public class ReactToFood : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public ReactToFood(AnimalAI animalType) : base(animalType)
        {

        }

        // determines state of this node
        public override NodeState Execute()
        {
            // CONSIDER IF THIS COULD BE INCORPORATED IN THE CHECKFOODEFFECT NODE

            // if animal likes the current food
            if (animal.foodEffect == AnimalAI.FoodEffect.like)
            {
                animal.FoodMakeHappy();
            }
            // if animal dislikes current food
            else if (animal.foodEffect == AnimalAI.FoodEffect.dislike)
            {
                animal.FoodMakeAngry();
            }
            // if current food is poisonous
            else if (animal.foodEffect == AnimalAI.FoodEffect.poisoned)
            {
                animal.FoodMakeDie();
            }

            return NodeState.SUCCESS;
        }
    }
}
