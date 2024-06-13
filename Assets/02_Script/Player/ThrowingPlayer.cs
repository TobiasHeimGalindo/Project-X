using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // For using UI elements

public class ThrowingPlayer : MonoBehaviour
{
    private Transform PickUpPoint;
    private Transform player;
    private Player2Controller player2Controller;
    private GlidingController glidingController;
    private Rigidbody rb;

    public float PickUpDistance;
    public float forceMulti;
    public bool readyToThrow;
    public bool itemIsPicked;
    public bool isThrown;

    public List<GameObject> collidableObjects;
    public Slider forceSlider; // UI-Slider to display force

    private Gamepad gamepad;
    private bool throwPressed;
    private bool throwHeld;
    private bool canChargeThrow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player 1").transform;
        PickUpPoint = GameObject.Find("PickUpPoint").transform;
        player2Controller = GetComponent<Player2Controller>();
        glidingController = GetComponent<GlidingController>();

        // Assign the first connected gamepad to this player
        if (Gamepad.all.Count > 0)
        {
            gamepad = Gamepad.all[0];
        }
        else
        {
            Debug.LogError("No gamepad connected for Player 1");
        }

        // Initialize the slider
        if (forceSlider != null)
        {
            forceSlider.minValue = 0;
            forceSlider.maxValue = 300;
            forceSlider.value = forceMulti;
        }
    }

    void Update()
    {
        if (gamepad == null) return;

        PickUpDistance = Vector3.Distance(player.position, transform.position);

        // Charge throw force while holding the throw button
        if (throwHeld && itemIsPicked && canChargeThrow)
        {
            forceMulti += 300 * Time.deltaTime;
            forceMulti = Mathf.Clamp(forceMulti, 0, 300); // Clamp force to 300
            isThrown = true;

            // Update slider value
            if (forceSlider != null)
            {
                forceSlider.value = forceMulti;
            }
        }

        // Handle picking up the item
        if (PickUpDistance <= 2 && !itemIsPicked && PickUpPoint.childCount < 1 && throwPressed)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            GetComponent<SphereCollider>().enabled = false;
            transform.position = PickUpPoint.position;
            transform.parent = PickUpPoint;

            itemIsPicked = true;
            forceMulti = 0;

            if (forceSlider != null)
            {
                forceSlider.value = forceMulti;
            }

            if (player2Controller != null)
            {
                player2Controller.enabled = false;
            }

            throwPressed = false; // Reset throwPressed after picking up the item
        }

        // Handle throwing the item
        if (itemIsPicked && isThrown && !throwHeld)
        {
            if (forceMulti > 10)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                transform.parent = null;
                GetComponent<SphereCollider>().enabled = true;

                rb.AddForce(player.forward * forceMulti);
                rb.AddForce(player.up * forceMulti);

                itemIsPicked = false;
                isThrown = false;
                forceMulti = 0;

                if (forceSlider != null)
                {
                    forceSlider.value = forceMulti;
                }

                if (glidingController != null)
                {
                    glidingController.enabled = true;
                    glidingController.StartGliding(); // Call a method to start gliding
                }
            }

            forceMulti = 0;

            if (forceSlider != null)
            {
                forceSlider.value = forceMulti;
            }
        }

        // Reset the throw charge capability when the throw button is released
        if (!throwHeld && itemIsPicked)
        {
            canChargeThrow = true;
        }

        // Handle dropping the item
        if (gamepad.buttonWest.wasPressedThisFrame && itemIsPicked)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            transform.parent = null;
            GetComponent<SphereCollider>().enabled = true;

            itemIsPicked = false;
            readyToThrow = false;
            forceMulti = 0;

            if (forceSlider != null)
            {
                forceSlider.value = forceMulti;
            }

            if (player2Controller != null)
            {
                player2Controller.enabled = true;
            }

            if (glidingController != null)
            {
                glidingController.enabled = false;
            }
        }

        // Check for throw button press
        if (gamepad.buttonNorth.wasPressedThisFrame)
        {
            OnThrowPerformed();
        }

        if (!gamepad.buttonNorth.isPressed)
        {
            OnThrowCanceled();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collidableObjects.Contains(collision.gameObject))
        {
            if (player2Controller != null)
            {
                player2Controller.enabled = true;
            }

            if (glidingController != null)
            {
                glidingController.enabled = false;
            }
        }
    }

    void OnThrowPerformed()
    {
        if (itemIsPicked && canChargeThrow)
        {
            throwHeld = true;
        }
        else if (!itemIsPicked)
        {
            throwPressed = true;
        }
    }

    void OnThrowCanceled()
    {
        throwHeld = false;

        if (itemIsPicked)
        {
            isThrown = true;
        }
    }
}
