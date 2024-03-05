using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInput playerInput;

    private PlayerInput.MovementActions movement;

    //reference player motor script
    private PlayerMotor motor;
    // Start is called before the first frame update
    void Awake()
    {
        //create a new instance
        playerInput = new PlayerInput();
        movement = playerInput.Movement;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //player motor moves moves using the value from our movement action
        motor.ProcessMove(movement.Move.ReadValue<Vector2>());
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
