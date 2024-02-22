using UnityEngine;

public abstract class AnimalAI : MonoBehaviour
{
    // _______HEALTH_______
    // current health of animal
    public int currentHealth;
    // max health of animal
    public int maxHealth;
    // life status of animal
    private bool isAlive = true;

    // _______AFFECTS BEHAVIOUR_______
    // level of aggression towards player/animals
    public AggressionLevel aggression;
    // whether animal is wild or tamed
    public bool isTamed = false;


    // Should this be in a namespace?
    // various levels of aggression that an animal could have
    public enum AggressionLevel
    {
        none,
        low,
        medium,
        high
    }

    // Start is called before the first frame update
    void Start()
    {
        // set current health to max possible health
        // needs to be changed in future to allow for saved data to affect this, potentially search player prefs
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
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

    // return if animal is alive or not
    public virtual bool CheckIsAlive()
    {
        return isAlive;
    }
}
