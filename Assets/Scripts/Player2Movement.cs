using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Movement : MonoBehaviour {

  [Header("Ship parameters")]
  [SerializeField] private float shipAcceleration = 10f;
  [SerializeField] private float shipMaxVelocity = 30f;
  [SerializeField] private float shipRotationSpeed = 180f;
  [SerializeField] public float runningSpeed = 100f;

  [Header("Object references")]

  private Rigidbody2D shipRigidbody;
  private bool isAccelerating = false;
  private bool checkChecked = false;
  private bool isRunning = false;
  private float Laps = 0f;
  public Text player2Lap;


  private void Start() {
    // Get a reference to the attached RigidBody2D.
    shipRigidbody = GetComponent<Rigidbody2D>();
  }

  private void Update() {
      HandleShipAcceleration();
      HandleShipRotation();
      HandleShipRunning();

      player2Lap.text = "P2 laps: " + Laps.ToString();
  }

  private void FixedUpdate() {
    if(isAccelerating) {
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
        if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightShift)) {
            isRunning = true;
        } else if(Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightShift)) {
            isRunning = false;
        }
    }

  private void HandleShipAcceleration() {
    // Are we accelerating?
    isAccelerating = Input.GetKey(KeyCode.UpArrow);
  }

  private void HandleShipRotation() {
    // Ship rotation.
    if (Input.GetKey(KeyCode.LeftArrow)) {
      transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
    } else if (Input.GetKey(KeyCode.RightArrow)) {
      transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("FinishLine") && checkChecked) {
        Laps++;
        checkChecked = false;
        Debug.Log("Player2 Laps: " + Laps);
    } else if(collision.CompareTag("Checkpoint")) {
        checkChecked = true;
    }
  }
}
