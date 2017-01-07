using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float speedH = 10.0f;
	public float speedV = 10.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse Left") < 0) {
			//Debug.Log("Looking Left");
			yaw += speedH * Input.GetAxis("Mouse Left") * Time.deltaTime;
		}
		else if (Input.GetAxis("Mouse Right") > 0) {
			//Debug.Log("Looking Right");
			yaw += speedH * Input.GetAxis("Mouse Right") * Time.deltaTime;
		}
		if (Input.GetAxis("Mouse Up") > 0) {
			//Debug.Log("Looking Up");
			if (pitch >= -90) {
				pitch -= speedV * Input.GetAxis ("Mouse Up") * Time.deltaTime;
			}
			if (pitch < -90) {
				pitch = -90;
			}
		}
		else if (Input.GetAxis("Mouse Down") < 0) {
			//Debug.Log("Looking Down");
			if (pitch <= 90 ) {
				pitch -= speedV * Input.GetAxis ("Mouse Down") * Time.deltaTime;
			}
			if (pitch > 90) {
				pitch = 90;
			}
		}
		//Debug.Log(pitch);
		transform.parent.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}
}
