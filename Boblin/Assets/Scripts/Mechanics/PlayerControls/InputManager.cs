using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInput playerInput;

    private PlayerInput.MovementActions movement;
    // Start is called before the first frame update
    void Awake()
    {
        //create a new instance
        playerInput = new PlayerInput();
        movement = playerInput.Movement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        movement.Enable();

    }

    private void OnDisable()
    {
        movement.Disable();
    }
}
