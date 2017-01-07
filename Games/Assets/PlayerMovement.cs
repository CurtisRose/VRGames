using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public float playerMovementSpeed = 2.0f;

	// Use this for initialization
	void Start () {
		
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
	}
}
