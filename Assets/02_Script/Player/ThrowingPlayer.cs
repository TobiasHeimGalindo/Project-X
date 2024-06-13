using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Für die Verwendung von UI-Elementen

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

    public Slider forceSlider; // UI-Slider zur Anzeige der Kraft

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player 1").transform;
        PickUpPoint = GameObject.Find("PickUpPoint").transform;
        player2Controller = GetComponent<Player2Controller>();
        glidingController = GetComponent<GlidingController>();

        // Initialisieren Sie den Slider
        if (forceSlider != null)
        {
            forceSlider.minValue = 0;
            forceSlider.maxValue = 300;
            forceSlider.value = forceMulti;
        }
    }

    void Update()
    {
        PickUpDistance = Vector3.Distance(player.position, transform.position);

        if (Input.GetKey(KeyCode.E) && itemIsPicked && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
            forceMulti = Mathf.Clamp(forceMulti, 0, 300); // Begrenzen der Kraft auf 300
            isThrown = true;

            // Aktualisieren Sie den Slider-Wert
            if (forceSlider != null)
            {
                forceSlider.value = forceMulti;
            }
        }

        if (PickUpDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && !itemIsPicked && PickUpPoint.childCount < 1)
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
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && itemIsPicked)
        {
            readyToThrow = true;

            if (forceMulti > 10)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                transform.parent = null;
                GetComponent<SphereCollider>().enabled = true;
                rb.AddForce(player.forward * forceMulti);
                rb.AddForce(player.up * forceMulti);

                itemIsPicked = false;
                readyToThrow = false;
                forceMulti = 0;

                if (glidingController != null)
                {
                    glidingController.enabled = true;
                    glidingController.StartGliding(); // Eine Methode zum Starten des Gleitens aufrufen
                }
            }

            forceMulti = 0;

            if (forceSlider != null)
            {
                forceSlider.value = forceMulti;
            }
        }

        if (Input.GetKeyUp(KeyCode.L) && itemIsPicked)
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
}
