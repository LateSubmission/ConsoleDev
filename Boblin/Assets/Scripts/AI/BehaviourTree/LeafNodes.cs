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
            NodeState state = animal.IsTamed ? NodeState.SUCCESS : NodeState.FAILURE;
            //Debug.Log("Is " + animal.animalName + " tamed? " + animal.GetIsTamed());
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
            NodeState state = animal.IsStay ? NodeState.SUCCESS : NodeState.FAILURE;
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
            // stay in place, use idle (standing) animation

            NodeState state = NodeState.SUCCESS;
            return state;
        }
    }

    /// <summary>
    /// Sets nav mesh agent to follow player
    /// </summary>
    public class FollowPlayer : LeafNode
    {
        // use constructor from LeafNode, passing in animalType
        public FollowPlayer(AnimalAI animalType) : base(animalType) { }

        // determines state of this node
        public override NodeState Execute()
        {
            bool isCloseToPlayer = animal.GetVectorToPlayer().magnitude < 2.0f;
            // want the animal to continue following unless they are close enough to the player
            if (isCloseToPlayer)
            {
                // allowed to have no path when close enough
                animal.navAgent.ResetPath();
                return NodeState.SUCCESS;
            }
            else
            {
                // only want to update position every few seconds to save performance
                animal.NavAgentUpdateTimer += Time.deltaTime;
                // after 3s, update destination
                if (animal.NavAgentUpdateTimer >= 3.0f)
                {
                    animal.navAgent.SetDestination(player.transform.position);
                    // reset timer so the cycle continues
                    animal.NavAgentUpdateTimer = 0f;
                    return NodeState.SUCCESS;
                }
            }

            // if they have no path at this point, the node fails
            NodeState state = animal.navAgent.hasPath ? NodeState.SUCCESS : NodeState.FAILURE;
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
            //Debug.Log("IsPlayerNear RUN");

            NodeState state;
            if (Vector3.Distance(player.transform.position, animal.transform.position) > animal.DetectDist)
            {
                //animal.IsThreatened = false;
                state = NodeState.FAILURE;
            }
            else
            {
                //Debug.Log("Player is near " + animal + "!");
                state = NodeState.SUCCESS;
            }
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
            // USED FOR TESTING ONLY!
            //animal.IsThreatened = true; // GUARANTEES THREATENED BEHAVIOUR
            //animal.IsThreatened = animal.GetVectorToPlayer().magnitude < animal.DangerDist;

            // if animal is staying, return success, else return failure
            NodeState state = animal.IsThreatened ? NodeState.SUCCESS : NodeState.FAILURE;
            //Debug.Log("Is Sparrow threatened? " + animal.IsThreatened);
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
            //Debug.Log("FightOrFlight LEAF NODE RUN");

            NodeState state;

            // only want to update position every few seconds to save performance
            //animal.NavAgentUpdateTimer += Time.deltaTime;

            // after 3s, update destination
            //if (animal.NavAgentUpdateTimer >= 3.0f || !animal.navAgent.hasPath)
            if (animal.navAgent.remainingDistance < 4.0f)
            {
                switch (animal.aggressionLevel)
                {
                    case AnimalAI.AggressionLevel.low:
                        // I will run/fly/swim away
                        animal.FightOrFlightLow();
                        state = NodeState.SUCCESS;
                        break;

                    case AnimalAI.AggressionLevel.medium:
                        // I'll give you a warning, don't threaten me more
                        animal.FightOrFlightMedium();
                        state = NodeState.SUCCESS;
                        break;

                    case AnimalAI.AggressionLevel.high:
                        // "I've been looking forward to this..." - Count Dooku
                        animal.FightOrFlightHigh();
                        state = NodeState.SUCCESS;
                        break;

                    default:
                        // Something went wrong if we got here, return false for FAILURE
                        state = NodeState.FAILURE;
                        break;
                }                

                // reset timer so the cycle continues
                //animal.NavAgentUpdateTimer = 0f;
                return state;
            }
            else
            {
                //Debug.Log("FightOrFlight already calculated recently, skipped.");
                return NodeState.SUCCESS;
            }
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
            // idle animation

            // already facing player so reset any existing paths
            if (animal.IsFacingPlayer()) animal.navAgent.ResetPath();
            // only turn if not already facing player
            else animal.FacePlayer();

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
            if (animal.navAgent.remainingDistance > 2f)
            {
                //Debug.Log("Wander not calculated, already has path");
                return NodeState.SUCCESS;
            }

            //Debug.Log("Wander executed");
            // Wander steering behaviour.
            // Could have an array of points on the map to seek
            // Could seek a point in a circle in front of AI

            // distance to sphere origin from player
            float sphereDist = animal.wanderDist;

            Vector3 nextVector = animal.GetForwardDirection();
            nextVector.x *= Random.Range(0.5f, 1f);
            nextVector.z *= Random.Range(0.5f, 1f);

            // create sphere origin position
            Vector3 sphereOrigin = animal.transform.position + nextVector * sphereDist;
            //Debug.Log("Sphere origin: " + sphereOrigin);

            animal.navAgent.SetDestination(AiMovement.NewWanderPos(sphereOrigin, sphereDist));

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
            foodStr = animalType.FoodBest;
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
            float closestFoodDist = animal.DetectDist;
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
                if (distToFood < animal.DetectDist)
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
            if (currentFood.type == animal.FoodBest)
            {
                // set the food's effect on the animal to "like"
                animal.foodEffect = AnimalAI.FoodEffect.like;
            }
            // if the current food is the animal's _least_ favourite
            else if (currentFood.type == animal.FoodWorst)
            {
                // set the food's effect on the animal to "dislike"
                animal.foodEffect = AnimalAI.FoodEffect.dislike;
            }
            // if the current food is poisonous to the animal
            else if (currentFood.type == animal.FoodPoisonous)
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
