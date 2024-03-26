using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    // potentially need a reference to the animal.
    // Either make functions static and have reference in each function, 
    // or maybe have an instance of this class on each animal? 

    public NavMeshAgent navAgent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Generates a random point in a sphere and returns the nearest point to it on the NavMeshSurface
    /// </summary>
    /// <param name="origin">The origin of a sphere in which a point will be generated</param>
    /// <param name="distance">The radius of the sphere</param>
    /// <returns></returns>
    public static Vector3 NewWanderPos(Vector3 origin, float distance)
    {
        Debug.Log("Getting wander pos");

        // find a random point in a unit sphere, multiply the value by a specified distance
        Vector3 randomPoint = Random.insideUnitSphere * distance;

        // make random position relative to origin
        randomPoint += origin;

        // Finds nearest point on navmesh to the randomPoint value, within distance, -1 represents all layers.
        NavMesh.SamplePosition(randomPoint, out NavMeshHit navHit, distance, -1);

        Debug.Log("navHit position: " +  navHit.position);

        // return the position on the nav mesh
        return navHit.position;
    }

    public void Rest()
    {
        // start sit animation, do not move for set amount of time or until provoked (e.g. isThreatened)
    }

    public void Search()
    {
        // searching for a particular food. Is this like wander behaviour but with a sensor?
        // potentially check specific places where favourite food may be (e.g. seeds under a tree?)
        
        // perhaps use nav mesh to find a specific location, then behaviour similar to wander 
        // to find food in this area.
    }

    // Stay?
    public void Stay()
    {
        // start idle animation. Sit is reserved for rest behaviour
    }
}
