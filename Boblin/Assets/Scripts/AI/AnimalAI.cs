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
    // typically used in debug statements
    protected string animalName;
    // _______MOVEMENT_______
    [Header("MOVEMENT")]
    public NavMeshAgent navAgent;
    // how animal can travel
    public TravelLocation travelType;
    // set movement speeds
    public float walkSpeed, runSpeed;
    // distance of wander sphere origin from player
    public float wanderDist;
    // timer for updating nav agent destination
    private float navAgentUpdateTimer;
    public float NavAgentUpdateTimer
    {
        get { return navAgentUpdateTimer; }
        // timer cannot be negative
        set { navAgentUpdateTimer = Mathf.Max(0, value); }
    }

    // _______HEALTH_______
    [Header("HEALTH")]
    // current health of animal
    [SerializeField] private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        // health cannot be negative
        set { currentHealth = Mathf.Max(0, value); }
    }
    // max health of animal
    public int maxHealth;
    // life status of animal
    [SerializeField] private bool isAlive = true;

    // _______AFFECTS BEHAVIOUR_______
    [Header("BEHAVIOUR")]
    // distance at which objects/player can be detected
    [SerializeField] private float detectDist;
    public float DetectDist
    {
        get { return detectDist; }
        set { detectDist = value; }
    }

    // whether animal is wild or tamed
    [SerializeField] private bool isTamed = false;
    public bool IsTamed
    {
        get { return isTamed; }
        set
        {
            isTamed = value;
            navAgent.ResetPath();
        }
    }

    // whether animal stays in place or follows player
    private bool isStay = false; // use an idle animation
    public bool IsStay
    {
        get { return isStay; }
        set
        {
            isStay = value;
            navAgent.ResetPath();
        }
    }

    // whether animal feels threatened or not
    [SerializeField] private bool isThreatened = false;
    public bool IsThreatened
    {
        get { return isThreatened; }
        set
        {
            isThreatened = value;
            Debug.Log("IsThreatened set to " + value);
            navAgent.ResetPath();
        }
    }

    // detects when player leaves or enters detectDist
    private SphereCollider detectCollider;

    // level of aggression towards player/animals
    public AggressionLevel aggressionLevel;
    // current reaction to food
    public FoodEffect foodEffect;
    // current food object to be eaten
    public Food foodTarget = null;
    // current food being eaten
    public Food foodEaten = null;

    // _______PERSONALITY_______
    [Header("PERSONALITY")]
    // food that can make the animal like you
    [SerializeField] private string foodBest;
    public string FoodBest
    {
        get { return foodBest; }
        // only set the string if it's a food that exists in the world
        set { if (Food.AllFoodNames.Contains(value)) foodBest = value; }
    }

    // food that can make animal aggressive
    [SerializeField] private string foodWorst;
    public string FoodWorst
    {
        get { return foodWorst; }
        // only set the string if it's a food that exists in the world
        set { if (Food.AllFoodNames.Contains(value)) foodWorst = value; }
    }

    // food that can kill the animal
    [SerializeField] private string foodPoisonous;
    public string FoodPoisonous
    {
        get { return foodPoisonous; }
        // only set the string if it's a food that exists in the world
        set { if (Food.AllFoodNames.Contains(value)) foodPoisonous = value; }
    }

    // _______PLAYER REFS_______
    [Header("PLAYER")]
    public GameObject player;

    // various levels of aggression that an animal could have
    // this defines how an animal behaves when feeling threatened
    public enum AggressionLevel
    {
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
    //public AnimalAI() { }
    private void Start() { }

    public void BaseInit()
    {
        // set current health to max possible health
        // needs to be changed in future to allow for saved data to affect
        // this, potentially search player prefs
        CurrentHealth = maxHealth;
        // set speed for nav agent
        navAgent.speed = walkSpeed;

        // assign sphere collider for detecting player
        if (!TryGetComponent(out detectCollider)) return;
    }

    // checks if player prefs exists already and assigns if so
    // only checks member variables that could change throughout gameplay
    public void CheckPlayerPrefs(AnimalAI animalInstance, string animalPrefix, int animalMaxHealth)
    {
        // currentHealth
        if (PlayerPrefs.HasKey(animalPrefix + "CurrentHealth"))
            animalInstance.CurrentHealth = PlayerPrefs.GetInt(animalPrefix + "CurrentHealth");
        else animalInstance.CurrentHealth = animalMaxHealth;
        animalInstance.isAlive = (animalInstance.CurrentHealth > 0);
    }


    // build the behaviour tree, passing in individual animal type
    protected Node BuildTree(AnimalAI animal)
    {
        // behaviour tree structure
        Node root = new Sequencer(new List<Node>
        {
            new Selector(false, new List<Node>
            {
                new Sequencer(new List<Node>
                {
                    new CheckIsTamed(animal),
                    new Selector(false, new List<Node>
                    {
                        new Sequencer(new List<Node> { new CheckIsStay(animal), new StayBehaviour(animal) }),
                        new FollowPlayer(animal)
                    })
                }),
                new Selector(false, new List<Node>
                {
                    new Sequencer(new List<Node>
                    {
                        new IsPlayerNear(animal),
                        new Selector(false, new List<Node>
                        {
                            new Sequencer(new List<Node>
                            {
                                new CheckIsThreatened(animal),
                                //new IsPlayerNear(animal),
                                new FightOrFlight(animal)
                            }),
                            new WaitForPlayer(animal)
                        })
                    }),
                    // random selector
                    new Selector(true, new List<Node> { new Wander(animal), new Rest(animal), new Search(animal) })
                })
            }),
            new Sequencer(new List<Node>
            {
                new CheckHasFood(animal),
                new EatFood(animal),
                new CheckFoodEffect(animal),
                new ReactToFood(animal)
            }),

        });
        Debug.Log("Tree built for " + animal.animalName);
        return root;
    }

    public void FightOrFlightLow()
    {
        //Debug.Log("Fight or Flight Low RUN");

        // move in direction opposite to player/threat
        // vector in opposite direction to player
        Vector3 vectorAway = GetVectorFromPlayer().normalized * DetectDist;
        //NavMesh.SamplePosition(vectorAway, out NavMeshHit hit, 10.0f, -1);
        //navAgent.SetDestination(hit.position);
        navAgent.SetDestination(vectorAway);
    }

    public void FightOrFlightMedium()
    {
        // look aggressive
        // if player threatens again, or does not move after an amount of time, attack
    }

    public void FightOrFlightHigh()
    {
        // attack as soon as threatened
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

    // animal has taken damage. Source could be player or bad food
    public virtual void TakeDamage(int damageNum)
    {
        // reduce current health by damage taken
        CurrentHealth -= damageNum;
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
    public Vector3 GetVectorFromPlayer()
    {
        return transform.position - player.transform.position;
    }

    // if player
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsThreatened = false;
        }
    }
}
