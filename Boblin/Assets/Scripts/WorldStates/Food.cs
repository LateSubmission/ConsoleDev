using UnityEngine;

public class Food : MonoBehaviour
{
    // type of food e.g. sunflower seed, chocolate, etc.
    public string type;
    // store position for distance testing
    public Vector3 position;

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
}
