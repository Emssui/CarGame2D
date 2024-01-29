using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

  [Header("Ship parameters")]
  [SerializeField] private float shipAcceleration = 10f;
  [SerializeField] private float shipMaxVelocity = 30f;
  [SerializeField] private float shipRotationSpeed = 180f;
  [SerializeField] public float runningSpeed = 100f;

  [Header("Object references")]

  private Rigidbody2D shipRigidbody;
  private bool isAccelerating = false;
  private bool isRunning = false;
  private float Laps = 0f;
  private bool checkChecked = false;
  public Text player1Lap;

  private void Start() {
    // Get a reference to the attached RigidBody2D.
    shipRigidbody = GetComponent<Rigidbody2D>();
  }

  private void Update() {
      HandleShipAcceleration();
      HandleShipRotation();
      HandleShipRunning();

      player1Lap.text = "P1 Laps: " + Laps.ToString();
  }

  private void FixedUpdate() {
    if (isAccelerating) {
       if(isRunning) {
            shipRigidbody.AddForce(runningSpeed * transform.up);
        } else {
            shipRigidbody.AddForce(shipAcceleration * transform.up);
        }
      // Increase velocity upto a maximum.
      shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
    } else {
      shipRigidbody.velocity = Vector2.zero;
      shipRigidbody.angularVelocity = 0f;
    }
  } 

  private void HandleShipRunning() {
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) {
            isRunning = true;
        } else if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift)) {
            isRunning = false;
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
    if (collision.CompareTag("FinishLine") && checkChecked) {
        Laps++;
        Debug.Log("Player1 Laps: " + Laps);
        checkChecked = false;
    } else if(collision.CompareTag("Checkpoint")) {
        checkChecked = true;
    }
  }
}
