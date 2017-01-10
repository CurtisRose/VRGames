using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : Item {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 1000;

	public Material onTarget;
	public Material offTarget;

	float time;

	float rateOfFire = 0.2f;

	bool twoHanded = false;

	// Use this for initialization
	void Start () {
		time = Time.realtimeSinceStartup;
		oldMaterial = gameObject.transform.GetChild(1).GetComponent<Renderer> ().material;
	}

	// Update is called once per frame
	void Update () {
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
		if (twoHanded) {
			//gameObject.transform.rot
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

	void AutomaticFire() {
		if (Time.realtimeSinceStartup - time > rateOfFire) {
			time = Time.realtimeSinceStartup;
			Fire ();
		}
	}

	public override void OnTriggerDown(GameObject controller) {
		//if (isHeld) {
		//	Fire ();
		//}
	}

	public override void OnTriggerUp(GameObject controller) {
		//Debug.Log ("Trigger Released While Holding Gun");
	}

	public override void OnTriggerHeld(GameObject controller) {
		//Debug.Log ("Trigger Held While Holding Gun");
		if (isHeld) {
			AutomaticFire ();
		}
	}

	public override void OnHairTriggerDown(GameObject controller) {
		//Debug.Log ("Hair Trigger Pressed While Holding Gun");
	}

	public override void OnHairTriggerUp(GameObject controller) {
		//Debug.Log ("Hair Trigger Released While Holding Gun");
	}

	public override void OnHairTriggerHeld(GameObject controller) {
		//Debug.Log ("Hair Trigger Held While Holding Gun");
	}

	public override void OnGripDown(GameObject controller) {
		PickUp (controller);
	}

	public override void OnGripUp(GameObject controller) {
		//Debug.Log ("Grip Released While Holding Gun");
	}

	public override void OnGripHeld(GameObject controller) {
		//Debug.Log ("Grip Held While Holding Gun");
	}

	public override void OnTouchpadDown(GameObject controller) {
		//Debug.Log ("Touchpad Pressed While Holding Gun");
	}

	public override void OnTouchpadUp(GameObject controller) {
		//Debug.Log ("Touchpad Released While Holding Gun");
	}

	public override void OnTouchpadHeld(GameObject controller) {
		//Debug.Log ("Touchpad Held While Holding Gun");
	}

	public override void OnMenuDown(GameObject controller) {
		//Debug.Log ("Menu Pressed While Holding Gun");
	}

	public override void OnMenuUp(GameObject controller) {
		//Debug.Log ("Menu Released While Holding Gun");
	}

	public override void OnMenuHeld(GameObject controller) {
		//Debug.Log ("Menu Held While Holding Gun");
	}

	protected override void PickUp(GameObject controller) {
		WandController controllerScipt = controller.GetComponent (typeof(WandController)) as WandController;
		if (isHeld && controllerScipt.controllerNumber == controllerNumberHolding) {
			//Debug.Log ("Dropping Gun");
			isHeld = false;
			controllerScipt.holdingItem = false;
			controllerNumberHolding = 0;

			controllerScipt.SetControllerVisible (controller, true);

			/*if (gameObject.transform.parent != null) {
				gameObject.transform.parent = null;
			}*/
			//gameObject.GetComponent<Rigidbody> ().useGravity = true;
			//gameObject.GetComponent<Rigidbody> ().isKinematic = false;
			if (GetComponent<FixedJoint> ()) {
				GetComponent<FixedJoint> ().connectedBody = null;
				Destroy (GetComponent<FixedJoint> ());
			}

			gameObject.GetComponent<Rigidbody> ().velocity = controllerScipt.getVelocity ();
			gameObject.GetComponent<Rigidbody> ().angularVelocity = controllerScipt.getVelocity ();

		} else if (!isHeld) {
			if (!controllerScipt.holdingItem) {
				Highlight (false);
				//Debug.Log ("Picking up Gun");
				isHeld = true;
				controllerScipt.holdingItem = true;
				controllerNumberHolding = controllerScipt.controllerNumber;

				controllerScipt.SetControllerVisible (controller, false);

				gameObject.transform.parent = controller.transform;
				//gameObject.GetComponent<Rigidbody> ().useGravity = false;
				//gameObject.GetComponent<Rigidbody> ().isKinematic = true;

				//Rotate the gun properly
				Quaternion newRotation = Quaternion.Euler (-75, 180, 0);
				gameObject.transform.rotation = controller.transform.rotation * newRotation;

				// Position the gun properly
				Vector3 newPosition = new Vector3 (0.0f, -0.25f, 0.1f);
				gameObject.transform.localPosition = newPosition;

				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				gameObject.transform.parent = null;
			}
		}
	}

	public override void Highlight(bool highlight) {
		//Debug.Log ("Highlighting Laser");
		if (highlight && !isHeld) {
			gameObject.transform.GetChild(1).GetComponent<Renderer> ().material = highlightMaterial;
		} else {
			gameObject.transform.GetChild(1).GetComponent<Renderer> ().material = oldMaterial;
		}
	}
}
