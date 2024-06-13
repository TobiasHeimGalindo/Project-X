using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Eingaben erfassen - Laufen links und rechts
        float moveX = Input.GetAxisRaw("Horizontal 1"); // A/D oder Pfeiltasten links/rechts (-1, 0, 1)
        float moveZ = Input.GetAxisRaw("Vertical 1");   // W/S oder Pfeiltasten hoch/runter (-1, 0, 1)

        // Bewegungsgeschwindigkeit anpassen

        float currentMoveSpeed = moveSpeed; // Aktuelle Bewegungsgeschwindigkeit

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed += sprintMultiplier; // Wenn Shift gedrückt und der Spieler sich nach vorne bewegt, erhöhe die Geschwindigkeit
        }

        // Bewegung berechnen
        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * currentMoveSpeed * Time.deltaTime;

        // Spieler bewegen
        transform.Translate(move, Space.Self);

        // Springen, wenn Leertaste gedrückt wird
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }

    void Jump()
    {
        // Überprüfen, ob der Spieler auf dem Boden ist, bevor er springt
        if (Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}


