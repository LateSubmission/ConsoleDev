using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // type of food e.g. sunflower seed, chocolate, etc.
    public string type;
    private static List<string> allFoodNames = new();
    public static List<string> AllFoodNames
    {
        get { return allFoodNames; }
        set { allFoodNames = value; }
    }

    // store position for distance testing
    public Vector3 position;
    // amount this food fills the animal up
    // maybe have a percentage of hunger, or larger animals having more hunger "points"
    public int hungerQuantity;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        WorldStates.foodItems.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BeEaten()
    {
        //player.hunger -= hungerQuantity;
        // maybe destroy this object in a separate function,
        // called by the player once this object is no longer needed.
        //Destroy(gameObject);
    }
}
