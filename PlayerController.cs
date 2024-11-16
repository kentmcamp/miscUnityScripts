using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float runSpeed = 10f;
    private CharacterController myCC;
    public float momentumDamping = 5f;

    private Vector3 inputVector;
    private Vector3 movementVector;
    private float myGravity = -10f;

    public float jumpHeight = 2f;
    public float fallMultiplier = 2.5f;
    private bool isJumping;
    private float verticalVelocity;

    public Animator camAnim;
    private bool isWalking;
    private bool isRunning;

    void Start()
    {
        myCC = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        MovePlayer();

        camAnim.SetBool("isWalking", isWalking);
        camAnim.SetBool("isRunning", isRunning);
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // if holding down wasd, then return -1, 0, 1
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);

            isWalking = true;

            // Running
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }
        else
        {
            // if not holding down wsad, then return whatever value inputVector was at when it was last checked and lerp it towards zero
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);

            isWalking = false;
            isRunning = false;
        }

        // Set the current speed based on whether the player is running or walking
        float currentSpeed = isRunning ? runSpeed : playerSpeed;

        // Jumping
        if (myCC.isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
                verticalVelocity = 0f; // Reset vertical velocity when grounded
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * myGravity); // Increase vertical velocity to jump
                isJumping = true;
            }
        }
        else
        {
            if (verticalVelocity < 0) // If falling...
            {
                verticalVelocity += myGravity * (fallMultiplier - 1) * Time.deltaTime; // ...increase the rate of falling
            }
            verticalVelocity += myGravity * Time.deltaTime; // Apply gravity
        }

        movementVector = (inputVector * currentSpeed) + (Vector3.up * verticalVelocity); // Apply vertical velocity to movement vector
    }

    void MovePlayer()
    {
        myCC.Move(movementVector * Time.deltaTime);
    }
}