using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (Input.GetKey(KeyCode.E) && itemIsPicked && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
            isThrown = true;

        }

        if (PickUpDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && !itemIsPicked && PickUpPoint.childCount < 1)
            {
                rb.useGravity = false;
                rb.isKinematic = true; // Set to kinematic when picked up
                GetComponent<SphereCollider>().enabled = false;
                transform.position = PickUpPoint.position; // Align position with pick up point
                transform.parent = PickUpPoint;

                itemIsPicked = true;
                forceMulti = 0;

                if (player2Controller != null)
                {
                    player2Controller.enabled = false;
                }

            }
        }

        if (Input.GetKeyUp(KeyCode.E) && itemIsPicked)
        {
            readyToThrow = true;

            if (forceMulti > 10)
            {
                rb.isKinematic = false; // Set to non-kinematic when thrown
                rb.useGravity = true;
                transform.parent = null;
                GetComponent<SphereCollider>().enabled = true;

                // Apply the force in the player's forward direction
                rb.AddForce(player.forward * forceMulti);
                rb.AddForce(player.up * forceMulti);

                itemIsPicked = false;
                readyToThrow = false;
                forceMulti = 0;

            }

            forceMulti = 0;
        }

        //Spieler 2 (der aufgehoben wird) kann sich selber wieder lösen mit L -> muss auf Controller gestellt werden
        if (Input.GetKeyUp(KeyCode.L) && itemIsPicked)
        {
            // Directly drop the item
            rb.isKinematic = false;
            rb.useGravity = true;
            transform.parent = null;
            GetComponent<SphereCollider>().enabled = true;

            itemIsPicked = false;
            readyToThrow = false;
            forceMulti = 0;

            // Enable Player2Controller when thrown
            if (player2Controller != null)
            {
                player2Controller.enabled = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Enable Player2Controller when colliding with specified objects
        if (collidableObjects.Contains(collision.gameObject))
        {
            if (player2Controller != null)
            {
                player2Controller.enabled = true;
            }
        }
    }

}
