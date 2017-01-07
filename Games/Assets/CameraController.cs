using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float cameraMovementSpeed = 2.0f;
	public float cameraRotationSpeed = 2.0f;

	public float speedH = 10.0f;
	public float speedV = 10.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Check for key presses [WASD and Arrow keys]
		if (Input.GetButton("Forward")) {
			Debug.Log("Moving Forward");
			transform.Translate(cameraMovementSpeed * Vector3.forward * Time.deltaTime);
		}
		if (Input.GetButton("Backward")) {
			Debug.Log("Moving Backward");
			transform.Translate(cameraMovementSpeed * Vector3.back * Time.deltaTime);
		}
		if (Input.GetButton("Left")) {
			Debug.Log("Moving Left");
			transform.Translate(cameraMovementSpeed * Vector3.left * Time.deltaTime);
		}
		if (Input.GetButton("Right")) {
			Debug.Log("Moving Right");
			transform.Translate(cameraMovementSpeed * Vector3.right * Time.deltaTime);
		}
		if (Input.GetAxis("Mouse Left") < 0) {
			Debug.Log("Looking Left");
			yaw += speedH * Input.GetAxis("Mouse Left") * Time.deltaTime;
		}
		else if (Input.GetAxis("Mouse Right") > 0) {
			Debug.Log("Looking Right");
			yaw += speedH * Input.GetAxis("Mouse Right") * Time.deltaTime;
		}
		if (Input.GetAxis("Mouse Up") > 0) {
			Debug.Log("Looking Up");
			pitch -= speedV * Input.GetAxis("Mouse Up") * Time.deltaTime;
		}
		else if (Input.GetAxis("Mouse Down") < 0) {
			Debug.Log("Looking Down");
			pitch -= speedV * Input.GetAxis("Mouse Down") * Time.deltaTime;
		}
		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}
}
