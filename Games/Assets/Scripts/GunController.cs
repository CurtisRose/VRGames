using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
	private float maxFOV = 60;
	private float minFOV = 40;
	private float zoomSpeed = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			//Debug.Log("Shooting");

		}
		if (Input.GetMouseButtonDown (1)) {
			//Debug.Log ("Aiming Down Sights");
			transform.Translate (-.264f, .2f, 0.1f);
		} 
		else if (Input.GetMouseButtonUp (1)) {
			transform.Translate (.264f, -.2f, -0.1f);
		}
		if (Input.GetMouseButton(1)) {
			if (Camera.main.fieldOfView >= minFOV) {
				if (Camera.main.fieldOfView - zoomSpeed < minFOV) {
					Camera.main.fieldOfView = minFOV;
				} else {
					Camera.main.fieldOfView -= zoomSpeed;
				}
			}
		} else {
			if (Camera.main.fieldOfView <= maxFOV) {
				if (Camera.main.fieldOfView + zoomSpeed > maxFOV) {
					Camera.main.fieldOfView = maxFOV;
				} else {
					Camera.main.fieldOfView += zoomSpeed;
				}
			}

		}
	}
}
