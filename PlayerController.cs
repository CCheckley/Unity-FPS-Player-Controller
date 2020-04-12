using UnityEngine;

// Ensures the required component is attached to the current object
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // This makes use of the character controller class for fast QUAKE/DOOM-like movement
    CharacterController characterController; // Character controller variable to store reference to attached script
    Vector3 movementDirection = Vector3.zero; // Vector3 variable to store current movement direction

    [SerializeField] protected GameObject characterHead; // Player head which is a child object of the current object this script is attached to

    [SerializeField] protected float lookSpeed = 30.0f;
    [SerializeField] protected float movementSpeed = 10.0f;
    [SerializeField] protected float jumpForce = 10.0f;
    [SerializeField] protected float gravity = 9.807f; // We have to manually define and apply gravity due to the use of the character controller unity class for movement

    [SerializeField] protected bool invertVerticalLookDirection; // Specify whether look should be inverted, this can be done in a nicer way if your familiar with unity's input system

    protected virtual void Start()
    {
        // Setup attached character controller
        characterController = GetComponent<CharacterController>();
        characterController.enableOverlapRecovery = true;
    }

    protected virtual void Update()
    {
        // Handle input
        float lookRight = Input.GetAxisRaw("Mouse X");
        float lookUp = Input.GetAxisRaw("Mouse Y");

        float moveRight = Input.GetAxisRaw("Horizontal");
        float moveForward = Input.GetAxisRaw("Vertical");

        bool isJumping = Input.GetButtonDown("Jump");

        // Call functions using input
        Move(moveRight, moveForward, isJumping);
        Look(lookRight, lookUp);

        movementDirection.y -= gravity * Time.deltaTime; // Apply gravity over time
        characterController.Move(movementDirection * Time.deltaTime); // Move using the CharacterController class
    }

    public void Move(float moveRight, float moveForward, bool isJumping)
    {
        if (characterController.isGrounded) // Check if toouching floor
        {
            movementDirection = new Vector3(moveRight, 0.0f, moveForward); // Apply movement from input
            movementDirection = transform.TransformDirection(movementDirection); // Make movement relative to rotation
            movementDirection *= movementSpeed; // Move at specified speed

            if (isJumping) { movementDirection.y = jumpForce; } // Apply jump force if input
        }
    }

    public void Look(float lookRight, float lookUp)
    {
        float verticalLookDelta = (lookUp * lookSpeed) * Time.deltaTime;
        float horizontalLookDelta = (lookRight * lookSpeed) * Time.deltaTime;

        float desiredVerticalLookDelta = (invertVerticalLookDirection) ? verticalLookDelta : -verticalLookDelta; // Inver look direction if specified

        transform.Rotate(new Vector3(0.0f, horizontalLookDelta, 0.0f)); // Rotate object around Y axis
        characterHead.transform.Rotate(new Vector3(desiredVerticalLookDelta, 0.0f, 0.0f)); // Rotate Head around X axis
    }
}