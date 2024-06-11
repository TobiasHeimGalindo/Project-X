using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowingPlayer : MonoBehaviour
{
    private Transform PickUpPoint;
    private Transform player;
    private Player2Controller player2Controller;
    private Rigidbody rb;

    public float PickUpDistance;
    public float forceMulti;

    public bool readyToThrow;
    public bool itemIsPicked;
    public bool isThrown;

    public List<GameObject> collidableObjects;

    private PlayerControls controls;
    private bool throwPressed;
    private bool throwHeld;
    private bool canChargeThrow;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Throw.performed += OnThrowPerformed;
        controls.Gameplay.Throw.canceled += OnThrowCanceled;
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.Gameplay.Throw.performed -= OnThrowPerformed;
        controls.Gameplay.Throw.canceled -= OnThrowCanceled;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player 1").transform;
        PickUpPoint = GameObject.Find("PickUpPoint").transform;
        player2Controller = GetComponent<Player2Controller>();
    }

    void Update()
    {
        PickUpDistance = Vector3.Distance(player.position, transform.position);

        // Handle charging throw force while holding the throw button
        if (throwHeld && itemIsPicked && canChargeThrow)
        {
            forceMulti += 300 * Time.deltaTime;
            isThrown = true;
        }

        // Handle picking up the item
        if (PickUpDistance <= 2)
        {
            if (throwPressed && !itemIsPicked && PickUpPoint.childCount < 1)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                GetComponent<SphereCollider>().enabled = false;
                transform.position = PickUpPoint.position;
                transform.parent = PickUpPoint;

                itemIsPicked = true;
                forceMulti = 0;
                canChargeThrow = false; // Prevent charging throw immediately after picking up

                if (player2Controller != null)
                {
                    player2Controller.enabled = false;
                }
            }
        }

        // Handle throwing the item
        if (!throwHeld && itemIsPicked && isThrown)
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
            }
            forceMulti = 0;
        }

        // Handle resetting the throw charge capability
        if (!throwHeld && itemIsPicked)
        {
            canChargeThrow = true;
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
        }
    }

    void OnThrowPerformed(InputAction.CallbackContext context)
    {
        throwHeld = true;
        throwPressed = true;
    }

    void OnThrowCanceled(InputAction.CallbackContext context)
    {
        throwHeld = false;
        throwPressed = false;
    }
}
