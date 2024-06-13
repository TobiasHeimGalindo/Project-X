using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isSprinting;
    private bool jumpPressed;
    private bool isGliding;
    private Gamepad gamepad;
    private GlidingController glidingController;

    void Awake()
    {
        // Initialize the GlidingController
        glidingController = GetComponent<GlidingController>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Assign the second connected gamepad to Player 2
        if (Gamepad.all.Count > 1)
        {
            gamepad = Gamepad.all[1];
        }
        else
        {
            Debug.LogError("No second gamepad connected for Player 2");
        }
    }

    void Update()
    {
        if (gamepad == null) return;

        // Move input
        moveInput = gamepad.leftStick.ReadValue();

        // Sprint input
        if (gamepad.leftStickButton.wasPressedThisFrame)
        {
            isSprinting = !isSprinting;
        }

        // Jump input
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            jumpPressed = true;
        }

        // Toggle gliding with right bumper (buttonR1)
        if (gamepad.rightShoulder.wasPressedThisFrame)
        {
            isGliding = !isGliding;
            if (isGliding)
            {
                glidingController.StartGliding();
            }
            else
            {
                glidingController.StopGliding();
            }
        }

        // Adjust movement speed
        float currentMoveSpeed = isSprinting ? moveSpeed + sprintMultiplier : moveSpeed;

        // Calculate movement
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * currentMoveSpeed * Time.deltaTime;

        // Move player
        transform.Translate(move, Space.Self);

        // Handle jumping
        if (jumpPressed)
        {
            Jump();
            jumpPressed = false; // Reset jump
        }
    }

    void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is TerrainCollider)
        {
            StopGliding();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider is TerrainCollider)
        {
            StopGliding();
        }
    }

    private void StopGliding()
    {
        if (isGliding)
        {
            isGliding = false;
            glidingController.StopGliding();
        }
    }
}
