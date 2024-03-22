using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
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
    public static Vector3 GetWanderPos(Vector3 origin, float distance)
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

    public void Wander()
    {

    }

    public void Rest()
    {

    }

    public void Search()
    {

    }
}
