using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_PS4
using UnityEngine.PS4;
#endif


public abstract class AnimalAI : MonoBehaviour
{
    // _______INDIVIDUAL_______
    [Header("INDIVIDUAL")]
    public string animalName;

    // _______MOVEMENT_______
    [Header("MOVEMENT")]
    public NavMeshAgent navAgent;
    // how animal can travel
    public TravelLocation travelType;
    // movement speeds
    public float walkSpeed, runSpeed;
    // distance of wander sphere origin from player
    public float wanderDist;

    // _______HEALTH_______
    [Header("HEALTH")]
    // current health of animal
    public int currentHealth;
    // max health of animal
    public int maxHealth;
    // life status of animal
    private bool isAlive = true;

    // _______AFFECTS BEHAVIOUR_______
    [Header("BEHAVIOUR")]
    // distance at which objects/player can be detected
    public float safeDist;
    // whether animal is wild or tamed
    public bool isTamed = false;
    // whether animal stays in place or follows player
    public bool isStay = false; // use an idle animation
    // whether animal feels threatened or not
    public bool isThreatened = false;
    // level of aggression towards player/animals
    public AggressionLevel aggression;
    // current reaction to food
    public FoodEffect foodEffect;
    // current food object to be eaten
    public Food foodTarget = null;
    // current food being eaten
    public Food foodEaten = null;

    // _______PERSONALITY_______
    [Header("PERSONALITY")]
    // preferred food
    public string foodBest;
    // least favourite food
    public string foodWorst;
    // poisonous food
    public string foodPoisonous;

    // _______PLAYER REFS_______
    [Header("PLAYER")]
    public GameObject player;


    // Should this be in a namespace?
    // various levels of aggression that an animal could have
    public enum AggressionLevel
    {
        none,
        low,
        medium,
        high
    }

    // how food affects animal
    public enum FoodEffect
    {
        neutral,
        dislike,
        like,
        poisoned,
    }

    // what the animal can travel in/on
    public enum TravelLocation
    {
        land,
        water,
        air
    }

    // Start is called before the first frame update
    //public AnimalAI()
    private void Start()
    {
        
    }

    public void BaseInit()
    {
        // set current health to max possible health
        // needs to be changed in future to allow for saved data to affect
        // this, potentially search player prefs
        currentHealth = maxHealth;
        // set speed for nav agent
        navAgent.speed = walkSpeed;
    }

    // checks if player prefs exists already and assigns if so
    // only checks member variables that could change throughout gameplay
    public void CheckPlayerPrefs(AnimalAI animalInstance, string animalPrefix, int animalMaxHealth)
    {
        // currentHealth
        if (PlayerPrefs.HasKey(animalPrefix + "CurrentHealth"))
            animalInstance.currentHealth = PlayerPrefs.GetInt(animalPrefix + "CurrentHealth");
        else animalInstance.currentHealth = animalMaxHealth;
        animalInstance.isAlive = (animalInstance.currentHealth > 0);
    }


    // build the behaviour tree, passing in individual animal type
    protected Node BuildTree(AnimalAI animal)
    {
        // behaviour tree structure
        Node root = new Sequencer(new List<Node>
        {
            new Selector(new List<Node>
            {
                new Sequencer(new List<Node>
                {
                    new CheckIsTamed(animal),
                    new Selector(new List<Node>
                    {
                        new Sequencer(new List<Node> { new CheckIsStay(animal), new StayBehaviour(animal) })
                        // FollowPlayer();
                    })
                }),
                new Selector(new List<Node>
                {
                    new Sequencer(new List<Node>
                    {
                        new IsPlayerNear(animal),
                        new Selector(new List<Node>
                        {
                            new Sequencer(new List<Node>
                            {
                                // CheckIsThreatened(),
                                //new FightOrFlight(animal)
                            }),
                            // WaitForPlayer()
                        })
                    }),
                    // random selector
                    new Selector(/*true,*/ new List<Node> { new Wander(animal), new Rest(animal), new Search(animal) })
                })
            }),
            new Sequencer(new List<Node>
            {
                // CheckHasFood(),
                // EatFood(),
                // CheckFoodEffect(),
                // ReactToFood()
            }),

        });
        Debug.Log("Tree built for " + animal.animalName);
        return root;
    }

    // check if the animal is wild or tamed
    public virtual bool GetIsTamed()
    {
        return isTamed;
    }

    // check if the animal is staying or following
    public virtual bool GetIsStay()
    {
        return isStay;
    }

    // check if animal is feeling threatened or not
    public bool GetIsThreatened()
    {
        return isThreatened;
    }

    // determines behaviour in response to threat
    public void FightOrFlight(AnimalAI animal)
    {
        switch (animal)
        {
            case SparrowAI:
                //Debug.Log("I'm a sparrow");
                break;
            case ColobusAI:
                //Debug.Log("I'm a colobus");
                break;
            default:
                //Debug.Log("I am a different animal");
                break;
        }
    }

    // react happily to favourite food
    public void FoodMakeHappy()
    {

    }

    // react angrily to lesat favourite food
    public void FoodMakeAngry()
    {

    }

    // die as a result of poisonous food
    public void FoodMakeDie()
    {

    }

    // virtual - not all animals will attack
    public virtual void Attack()
    {

    }

    // should this be DealDamage()? Look into which would work best
    public virtual void TakeDamage(int damageNum)
    {
        // reduce current health by damage taken
        currentHealth -= damageNum;
    }

    // less aggressive animals may run away if threatened
    public virtual void Retreat()
    {

    }

    // return if animal is alive or not
    public virtual bool CheckIsAlive()
    {
        return isAlive;
    }

    // To start an animation, perhaps include functions for specific
    // animations such as idle, fight, move so these can be called in
    // the behaviour tree
    public virtual void SetAnimation()
    {
        // for resting behaviour, use sitting animation
        // for stay, use idle standing still
    }

    public Vector3 GetForwardDirection()
    {
        return transform.forward;
    }

    public void FacePlayer()
    {
        //transform.forward = player.transform.position - transform.position;
        //if (transform.forward.magnitude > 1) transform.forward = transform.forward.normalized;
        Vector3 vectorToPlayer = GetVectorToPlayer();
        Vector3 destinationToTurn = transform.position + (vectorToPlayer.normalized * 0.1f);
        navAgent.SetDestination(destinationToTurn);
    }

    public Vector3 GetVectorToPlayer()
    {
        return player.transform.position - transform.position;
    }
}
