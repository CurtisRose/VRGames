using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : Item {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 1000;

	public Material onTarget;
	public Material offTarget;

	private bool isHeld = false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		// Shooting
		if (gameObject.transform.parent != null && 
			SteamVR_Controller.Input ((int)gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject> ().index).GetHairTriggerDown ()) {
			//Debug.Log("Attempting To Shoot");
			Fire();
		}

		// Raycasting to change sight colors
		Ray ray = new Ray(transform.GetChild (3).GetComponent<Transform>().position, 
			transform.GetChild (3).GetComponent<Transform>().forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			if (hit.transform.tag == "Target") {
				transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = onTarget;
				transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = onTarget;
				transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material = onTarget;
			}
			else if (hit.transform.tag == "Bullet") {

			}
			else {
				transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = offTarget;
				transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = offTarget;
				transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material = offTarget;
			}
		} else {
			transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = offTarget;
			transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = offTarget;
			transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material = offTarget;
		}
	}

	void Fire() {
		// Create the Bullet from the Bullet Prefab
		//Debug.Log("Firing");
		GameObject bullet = Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody> ().AddForce(bullet.transform.forward * bulletSpeed);
		// Destroy the bullet after 8 seconds
		Destroy(bullet, 8.0f);
	}

	public override void OnTriggerDown(GameObject controller) {
		if (isHeld) {
			Fire ();
		}
	}

	public override void OnTriggerUp(GameObject controller) {
		Debug.Log ("Trigger Released While Holding Gun");
	}

	public override void OnTriggerHeld(GameObject controller) {
		Debug.Log ("Trigger Held While Holding Gun");
	}

	public override void OnHairTriggerDown(GameObject controller) {
		Debug.Log ("Hair Trigger Pressed While Holding Gun");
	}

	public override void OnHairTriggerUp(GameObject controller) {
		Debug.Log ("Hair Trigger Released While Holding Gun");
	}

	public override void OnHairTriggerHeld(GameObject controller) {
		Debug.Log ("Hair Trigger Held While Holding Gun");
	}

	public override void OnGripDown(GameObject controller) {
		if (isHeld) {
			//Debug.Log ("Dropping Gun");
			isHeld = false;

			WandController controllerScipt = controller.GetComponent (typeof(WandController)) as WandController;
			controllerScipt.SetControllerVisible (controller, true);

			if (gameObject.transform.parent != null) {
				gameObject.transform.parent = null;
			}
			gameObject.GetComponent<Rigidbody> ().useGravity = true;
			gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			gameObject.GetComponent<Rigidbody> ().velocity = controller.GetComponent<Rigidbody>().velocity;
			gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.GetComponent<Rigidbody>().angularVelocity;

		} else {
			//Debug.Log ("Picking up Gun");
			isHeld = true;
			WandController controllerScipt = controller.GetComponent (typeof(WandController)) as WandController;
			controllerScipt.SetControllerVisible (controller, false);

			gameObject.transform.parent = controller.transform;
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		
			//Rotate the gun properly
			Quaternion newRotation = Quaternion.Euler (-75, 180, 0);
			gameObject.transform.rotation = controller.transform.rotation * newRotation;

			// Position the gun properly
			Vector3 newPosition = new Vector3(0.0f, -0.25f, 0.1f);
			gameObject.transform.localPosition = newPosition;
		}
	}

	public override void OnGripUp(GameObject controller) {
		Debug.Log ("Grip Released While Holding Gun");
	}

	public override void OnGripHeld(GameObject controller) {
		Debug.Log ("Grip Held While Holding Gun");
	}

	public override void OnTouchpadDown(GameObject controller) {
		Debug.Log ("Touchpad Pressed While Holding Gun");
	}

	public override void OnTouchpadUp(GameObject controller) {
		Debug.Log ("Touchpad Released While Holding Gun");
	}

	public override void OnTouchpadHeld(GameObject controller) {
		Debug.Log ("Touchpad Held While Holding Gun");
	}

	public override void OnMenuDown(GameObject controller) {
		Debug.Log ("Menu Pressed While Holding Gun");
	}

	public override void OnMenuUp(GameObject controller) {
		Debug.Log ("Menu Released While Holding Gun");
	}

	public override void OnMenuHeld(GameObject controller) {
		Debug.Log ("Menu Held While Holding Gun");
	}
}
