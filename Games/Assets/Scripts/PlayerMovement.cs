using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	float playerMovementSpeed = 3.0f;
	float sprintingSpeed = 3.0f;
	float aimingSpeed = 1.0f;
	float crouchingSpeed = 1.0f;
	float jumpVelocity = 300.0f;
	bool jumping = false;
	bool doubleJumping = false;
	bool sprinting = false;
	bool aiming = false;
	bool crouching = false;

	public GameObject crouchingPosition;
	public GameObject standingPosition;
	float crouchTime = 0.04f;

	// Use this for initialization
	void Start () {

	}

	void OnCollisionEnter(Collision col) {
		//Debug.Log ("Collision");
		if (col.collider.tag == "Ground") {
			jumping = false;
			doubleJumping = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Check for key presses [WASD and Arrow keys]
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			//Debug.Log("Moving Forward");
			transform.Translate(playerMovementSpeed * Vector3.forward * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			//Debug.Log("Moving Backward");
			transform.Translate(playerMovementSpeed * Vector3.back * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			//Debug.Log("Moving Left");
			transform.Translate(playerMovementSpeed * Vector3.left * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			//Debug.Log("Moving Right");
			transform.Translate(playerMovementSpeed * Vector3.right * Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			//Debug.Log (jumping);
			if (!jumping) {
				jumping = true;
				Jump ();
			} else if (jumping && !doubleJumping) {
				doubleJumping = true;
				Jump ();
			}
		}
		if (Input.GetMouseButton (1)) {
			if (!aiming) {
				aiming = true;
				playerMovementSpeed -= aimingSpeed;
			}
		} else {
			if (aiming) {
				aiming = false;
				playerMovementSpeed += aimingSpeed;
			}
		}
		if (Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(1)) {
			//Debug.Log("Sprinting");
			if (!sprinting) {
				sprinting = true;
				playerMovementSpeed += sprintingSpeed;
			}
		}
		else {
			//Debug.Log("Walking");
			if (sprinting) {
				sprinting = false;
				playerMovementSpeed -= sprintingSpeed;
			}
		}
		if (Input.GetKey (KeyCode.LeftControl)) {
			if (!crouching) {
				Debug.Log ("Crouching");
				crouching = true;
				playerMovementSpeed -= crouchingSpeed;
			}
			Vector3 velocity = Vector3.zero;
			transform.GetChild(0).position = Vector3.SmoothDamp (transform.GetChild(0).position, 
				crouchingPosition.transform.position, ref velocity, crouchTime);
		} else {
			if (crouching) {
				Debug.Log ("Walking");
				crouching = false;
				playerMovementSpeed += crouchingSpeed;
			}
			Vector3 velocity = Vector3.zero;
			transform.GetChild(0).position = Vector3.SmoothDamp (transform.GetChild(0).position, 
				standingPosition.transform.position, ref velocity, crouchTime);
		}
	}
	
	void Jump () {
		//Debug.Log ("Jumping");
		transform.GetComponent<Rigidbody> ().velocity = transform.up * Time.deltaTime * jumpVelocity;
	}
}
