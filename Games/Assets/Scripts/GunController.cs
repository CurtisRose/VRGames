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
		Physics.IgnoreCollision (bullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
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

	public override void OnTriggerDown(WandController controller) {
		//if (isHeld) {
		//	Fire ();
		//}
	}

	public override void OnTriggerUp(WandController controller) {
		//Debug.Log ("Trigger Released While Holding Gun");
	}

	public override void OnTriggerHeld(WandController controller) {
		//Debug.Log ("Trigger Held While Holding Gun");
		if (isHeld) {
			AutomaticFire ();
		}
	}

	public override void OnHairTriggerDown(WandController controller) {
		//Debug.Log ("Hair Trigger Pressed While Holding Gun");
	}

	public override void OnHairTriggerUp(WandController controller) {
		//Debug.Log ("Hair Trigger Released While Holding Gun");
	}

	public override void OnHairTriggerHeld(WandController controller) {
		//Debug.Log ("Hair Trigger Held While Holding Gun");
	}

	public override void OnGripDown(WandController controller) {
		PickUp (controller);
	}

	public override void OnGripUp(WandController controller) {
		//Debug.Log ("Grip Released While Holding Gun");
	}

	public override void OnGripHeld(WandController controller) {
		//Debug.Log ("Grip Held While Holding Gun");
	}

	public override void OnTouchpadDown(WandController controller) {
		//Debug.Log ("Touchpad Pressed While Holding Gun");
	}

	public override void OnTouchpadUp(WandController controller) {
		//Debug.Log ("Touchpad Released While Holding Gun");
	}

	public override void OnTouchpadHeld(WandController controller) {
		//Debug.Log ("Touchpad Held While Holding Gun");
	}

	public override void OnMenuDown(WandController controller) {
		//Debug.Log ("Menu Pressed While Holding Gun");
	}

	public override void OnMenuUp(WandController controller) {
		//Debug.Log ("Menu Released While Holding Gun");
	}

	public override void OnMenuHeld(WandController controller) {
		//Debug.Log ("Menu Held While Holding Gun");
	}

	protected override void PickUp(WandController controller) {
		if (isHeld) {
			if (controller.controllerNumber == controllerNumberHolding) {
				//Debug.Log ("Dropping Gun");
				isHeld = false;
				controller.holdingItem = false;
				controllerNumberHolding = 0;
				controller.objectInHand = null;

				controller.SetControllerVisible (true);

				if (GetComponent<FixedJoint> ()) {
					GetComponent<FixedJoint> ().connectedBody = null;
					Destroy (GetComponent<FixedJoint> ());
				}

				gameObject.GetComponent<Rigidbody> ().velocity = controller.getVelocity ();
				gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.getVelocity ();
				//Highlight ();
			}
			else {
				//Debug.Log ("Gun Swapping Hands");
				controller.holdingItem = true;
				controllerNumberHolding = controller.controllerNumber;
				controller.objectInHand = gameObject;

				if (gameObject.GetComponent<FixedJoint> ()) {
					WandController oldController = gameObject.GetComponent<FixedJoint> ().connectedBody.gameObject.GetComponent<WandController>() as WandController;
					oldController.SetControllerVisible (true);
					oldController.holdingItem = false;
					oldController.objectInHand = null;
					gameObject.GetComponent<FixedJoint> ().connectedBody = null;
					Destroy (gameObject.GetComponent<FixedJoint> ());
				}

				controller.SetControllerVisible (false);
				gameObject.transform.parent = controller.transform;
				Quaternion newRotation = Quaternion.Euler (-75, 180, 0);
				gameObject.transform.rotation = controller.transform.rotation * newRotation;
				Vector3 newPosition = new Vector3 (0.0f, -0.25f, 0.1f);
				gameObject.transform.localPosition = newPosition;
				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				gameObject.transform.parent = null;
				Highlight (false);
			}
		} else if (!isHeld) {
			if (!controller.holdingItem) {
				//Debug.Log ("Picking up Gun");
				isHeld = true;
				controller.holdingItem = true;
				controllerNumberHolding = controller.controllerNumber;
				controller.objectInHand = gameObject;

				controller.SetControllerVisible (false);

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
				Highlight (false);
			}
		}
	}

	public override void Highlight(bool highlight) {
		if (highlight) {
			gameObject.transform.GetChild(1).GetComponent<Renderer> ().material = highlightMaterial;
		} else {
			gameObject.transform.GetChild(1).GetComponent<Renderer> ().material = oldMaterial;
		}
	}
}
