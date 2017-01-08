using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public float playerMovementSpeed = 2.0f;
	public float jumpVelocity = 300.0f;
	public bool jumping = false;
	public bool doubleJump = false;

	// Use this for initialization
	void Start () {

	}

	void OnCollisionEnter(Collision col) {
		//Debug.Log ("Collision");
		if (col.collider.tag == "Ground") {
			jumping = false;
			doubleJump = false;
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
			Debug.Log (jumping);
			if (!jumping) {
				jumping = true;
				Jump ();
			} else if (jumping && !doubleJump) {
				doubleJump = true;
				Jump ();
			}
		}
	}
	
	void Jump () {
		//Debug.Log ("Jumping");
		transform.GetComponent<Rigidbody> ().velocity = transform.up * Time.deltaTime * jumpVelocity;
	}
}
