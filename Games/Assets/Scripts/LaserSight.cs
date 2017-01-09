using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour {
	private Vector3 hitPoint;
	private bool laserOn = false;
	private float maxLaserDistance = 100;

	void Start () {
		//Debug.Log(transform.GetChild (0).name);
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild(1).gameObject.SetActive (false);
	}

	private void ShowLaser(RaycastHit hit) {
		//Debug.Log ("hit distance: " + hit.distance);
		transform.GetChild(0).position = Vector3.Lerp (transform.position, hitPoint, 0.5f);
		transform.GetChild(0).LookAt (hitPoint);
		transform.GetChild(1).gameObject.SetActive (true);
		transform.GetChild (1).position = hitPoint;
		transform.GetChild(0).localScale = new Vector3 (
			transform.GetChild(0).localScale.x,
			transform.GetChild(0).localScale.y,
			hit.distance/(transform.localScale.z * transform.parent.localScale.z));
	}
	private void ShowLaserInfinite() {
		//Debug.Log (transform.forward);
		transform.GetChild(1).gameObject.SetActive (false);
		transform.GetChild(0).position = Vector3.Lerp (transform.position, transform.position + (-1 * transform.forward * maxLaserDistance), 0.5f);
		transform.GetChild (0).LookAt (transform.position);
		transform.GetChild(0).localScale = new Vector3 (
			transform.GetChild(0).localScale.x,
			transform.GetChild(0).localScale.y,
			maxLaserDistance/(transform.localScale.z * transform.parent.localScale.z));
	}

	void Update () {
		if (gameObject.transform.parent.parent != null) {
			if (SteamVR_Controller.Input ((int)gameObject.transform.parent.parent.gameObject.GetComponent<SteamVR_TrackedObject> ().index).GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
				if (laserOn) {
					//Debug.Log ("Turning off Laser");
					laserOn = false;
					transform.GetChild(0).gameObject.SetActive (false);
				} else {
					//Debug.Log ("Turning on Laser");
					laserOn = true;
					transform.GetChild(0).gameObject.SetActive (true);
				}
			}
		}
		if (laserOn) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, -1 * transform.forward, out hit, maxLaserDistance)) {
				hitPoint = hit.point;
				ShowLaser (hit);
			} else {
				ShowLaserInfinite ();
			}
		}
	}
}
