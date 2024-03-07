using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    private Transform mainCameraTransform; // Reference to the main camera's transform

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCameraTransform = Camera.main.transform; // Assign the main camera's transform
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        RotateWithCamera(); // Call RotateWithCamera in Update to rotate the player with the camera
    }

    // Rotate the player to match the camera's rotation
    private void RotateWithCamera()
    {
        // Get the camera's forward direction without the y-component (for horizontal rotation)
        Vector3 cameraForward = mainCameraTransform.forward;
        cameraForward.y = 0f;

        // Rotate the player to face the same direction as the camera
        if (cameraForward != Vector3.zero)
        {
            transform.forward = cameraForward.normalized;
        }
    }

    //receive inputs from InputManager and apply them to character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}


