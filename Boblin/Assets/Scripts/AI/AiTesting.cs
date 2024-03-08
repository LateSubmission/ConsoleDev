using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTesting : MonoBehaviour
{
    public AnimalAI sparrow;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            sparrow.isTamed = !sparrow.isTamed;
        }
    }
}
