using UnityEngine;
using UnityEngine.InputSystem;

public class GlidingController : MonoBehaviour
{
    public float glideForce = 10f;
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Gamepad gamepad;
    private bool isGliding = false;

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

    void FixedUpdate()
    {
        if (isGliding)
        {
            if (gamepad == null) return;

            // Apply upward force to simulate gliding
            rb.AddForce(Vector3.up * glideForce);

            // Control the gliding direction using the left stick of the gamepad
            Vector2 moveInput = gamepad.leftStick.ReadValue();
            Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
            rb.AddForce(moveDirection * moveSpeed);
        }
    }

    public void StartGliding()
    {
        isGliding = true;
    }

    public void StopGliding()
    {
        isGliding = false;
    }
}
