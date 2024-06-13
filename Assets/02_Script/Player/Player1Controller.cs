using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isSprinting;
    private bool jumpPressed;
    private Gamepad gamepad;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Assign the first connected gamepad to Player 1
        if (Gamepad.all.Count > 0)
        {
            gamepad = Gamepad.all[0];
        }
        else
        {
            Debug.LogError("No gamepad connected for Player 1");
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
}
