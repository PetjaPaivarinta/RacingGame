using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

  private float Laps = 0f; 

  private bool goalCheck = false;


  public AudioSource Deathsound;

  [SerializeField] private float shipAcceleration = 10f;
   [SerializeField] private float runningSpeed = 50f;

  private float normalSpeed;
  [SerializeField] private float shipMaxVelocity = 10f;
  [SerializeField] private float shipRotationSpeed = 180f;


  private Rigidbody2D shipRigidbody;
  public Text Player1lapCountText; 
    private bool checkChecked = false;
    private bool isRunning = false;
  private bool isAccelerating = false;

  private void Start() {
    // Get a reference to the attached RigidBody2D.
    shipRigidbody = GetComponent<Rigidbody2D>();

  }

  private void Update() {
      HandleShipAcceleration();
      HandleShipRotation();
      HandleShipRunning();

      Player1lapCountText.text = "P1: " + Laps.ToString();
  }

  private void FixedUpdate() {
    // Check if accelerating
    if (isAccelerating) {
        if (isRunning) {
            // Apply running speed force
            shipRigidbody.AddForce(runningSpeed * transform.up);
        } else {
            // Apply normal acceleration force
            shipRigidbody.AddForce(shipAcceleration * transform.up);
        }

        // Clamp the velocity to the maximum
        shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);

        // Remove any torque to prevent unwanted rotation
        shipRigidbody.angularVelocity = 0f;
    } else {
        // Decelerate by setting velocity to zero
        shipRigidbody.velocity = Vector2.zero;

        // Remove any torque to prevent unwanted rotation
        shipRigidbody.angularVelocity = 0f;
    }
}


  private void HandleShipRunning() {
    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) {
        Debug.Log("Running");
        isRunning = true;
    } else {
        isRunning = false;
        Debug.Log("Not Running");
    }
  }

  private void HandleShipAcceleration() {
    // Are we accelerating?
    isAccelerating = Input.GetKey(KeyCode.W);
  }

  private void HandleShipRotation() {
    // Ship rotation.
    if (Input.GetKey(KeyCode.A)) {
      transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
    } else if (Input.GetKey(KeyCode.D)) {
      transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
    }
  }


  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Finish") && checkChecked && goalCheck)
    {
      Laps++;
      shipAcceleration = 10f;
      runningSpeed = 100f;
      checkChecked = false;
      Debug.Log("Player 1 laps: " + Laps);
    }
    if (collision.CompareTag("Checkpoint"))
    {
        checkChecked = true;
    }
    if (collision.CompareTag("Oil"))
    {
      shipAcceleration = 3f;
      runningSpeed = 30f;
      collision.gameObject.SetActive(false);
    }
    if (collision.CompareTag("GoalCheck"))
    {
      goalCheck = true;
    }
  }
}
