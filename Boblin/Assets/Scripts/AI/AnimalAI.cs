using UnityEngine;


public abstract class AnimalAI : BehaviourTree.Tree
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
    public AnimalAI()
    {
        // set current health to max possible health
        // needs to be changed in future to allow for saved data to affect
        // this, potentially search player prefs
        currentHealth = maxHealth;
    }

    // check if the animal is wild or tamed
    public virtual bool CheckIsTamed()
    {
        return isTamed;
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
    public virtual void StartAnimation()
    {

    }
}
