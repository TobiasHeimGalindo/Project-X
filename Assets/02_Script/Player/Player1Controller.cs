using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool isSprinting;
    private bool jumpPressed;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += OnMove;
        controls.Gameplay.Move.canceled += OnMove;
        controls.Gameplay.Sprint.performed += OnSprint;
        controls.Gameplay.Jump.performed += OnJump;
        controls.Gameplay.Jump.canceled += OnJump;
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.Gameplay.Move.performed -= OnMove;
        controls.Gameplay.Move.canceled -= OnMove;
        controls.Gameplay.Sprint.performed -= OnSprint;
        controls.Gameplay.Jump.performed -= OnJump;
        controls.Gameplay.Jump.canceled -= OnJump;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Adjust movement speed
        float currentMoveSpeed = isSprinting ? moveSpeed + sprintMultiplier : moveSpeed;

        // Calculate movement
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized * currentMoveSpeed * Time.deltaTime;

        // Move player
        transform.Translate(move, Space.Self);

        // Stop sprinting if player stops moving
        if (moveInput == Vector2.zero)
        {
            isSprinting = false;
        }

        // Handle jumping
        if (jumpPressed)
        {
            Jump();
            jumpPressed = false; // Reset jump
        }
    }

    void Jump()
    {
        // Check if player is on the ground before jumping
        if (Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = !isSprinting; // Toggle sprinting state
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }
}
