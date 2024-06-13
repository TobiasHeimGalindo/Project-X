using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidingController : MonoBehaviour
{
    public float glideForce = 10f;
    public float moveSpeed = 5f;

    private Rigidbody rb;

    private bool isGliding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isGliding)
        {
            // Apply upward force to simulate gliding
            rb.AddForce(Vector3.up * glideForce);

            // Control the gliding direction using arrow keys
            float horizontalInput = Input.GetAxis("Horizontal 2");
            float verticalInput = Input.GetAxis("Vertical 2");
            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
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
