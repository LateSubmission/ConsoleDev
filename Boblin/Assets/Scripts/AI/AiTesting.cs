using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTesting : MonoBehaviour
{
    public AnimalAI sparrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            sparrow.isTamed = !sparrow.isTamed;
        }
    }
}
