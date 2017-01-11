using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolController  : Item {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 1000;

	void Start () {
		oldMaterial = gameObject.transform.GetChild(1).GetComponent<Renderer> ().material;
	}

	void Fire() {
		GameObject bullet = Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
		Physics.IgnoreCollision (bullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
		bullet.GetComponent<Rigidbody> ().AddForce(bullet.transform.forward * bulletSpeed);
		Destroy(bullet, 8.0f);
	}

	public override void Highlight(bool highlight) {
		if (highlight) {
			if (!isHeld) {
				gameObject.transform.GetChild(0).GetComponent<Renderer> ().material = highlightMaterial;
			} else {
				gameObject.transform.GetChild(0).GetComponent<Renderer> ().material = oldMaterial;
			}
		} else {
			gameObject.transform.GetChild(0).GetComponent<Renderer> ().material = oldMaterial;
		}
	}

	protected override void PickUp(GameObject controller) {
		WandController controllerScript = controller.GetComponent (typeof(WandController)) as WandController;
		if (isHeld && controllerScript.controllerNumber == controllerNumberHolding) {
			//Debug.Log ("Dropping Gun");
			isHeld = false;
			controllerScript.holdingItem = false;
			controllerScript.objectInHand = null;
			controllerNumberHolding = 0;
			controllerScript.SetControllerVisible (controller, true);
			if (GetComponent<FixedJoint> ()) {
				GetComponent<FixedJoint> ().connectedBody = null;
				Destroy (GetComponent<FixedJoint> ());
			}
			gameObject.GetComponent<Rigidbody> ().velocity = controllerScript.getVelocity ();
			gameObject.GetComponent<Rigidbody> ().angularVelocity = controllerScript.getVelocity ();
			//Highlight ();
		} else if (!isHeld) {
			if (!controllerScript.holdingItem) {
				isHeld = true;
				controllerScript.holdingItem = true;
				controllerNumberHolding = controllerScript.controllerNumber;
				controllerScript.objectInHand = gameObject;
				controllerScript.SetControllerVisible (controller, false);
				gameObject.transform.parent = controller.transform;
				Quaternion newRotation = Quaternion.Euler (-80, 180, 0);
				gameObject.transform.rotation = controller.transform.rotation * newRotation;
				Vector3 newPosition = new Vector3 (0.0f, 0.0f, -0.1f);
				gameObject.transform.localPosition = newPosition;
				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				gameObject.transform.parent = null;
				Highlight (false);
			}
		}
	}

	public override void OnTriggerDown(GameObject controller) {
		//Debug.Log ("Trigger Pressed");
		Fire();
	}

	public override void OnTriggerUp(GameObject controller) {
		//Debug.Log ("Trigger Released");
	}

	public override void OnTriggerHeld(GameObject controller) {
		//Debug.Log ("Trigger Held");
	}

	public override void OnHairTriggerDown(GameObject controller) {
		//Debug.Log ("Hair Trigger Pressed");
	}

	public override void OnHairTriggerUp(GameObject controller) {
		//Debug.Log ("Hair Trigger Released");
	}

	public override void OnHairTriggerHeld(GameObject controller) {
		//Debug.Log ("Hair Trigger Held");
	}

	public override void OnGripDown(GameObject controller) {
		//Debug.Log ("Grip Pressed");
		PickUp(controller);
	}

	public override void OnGripUp(GameObject controller) {
		//Debug.Log ("Grip Released");
	}

	public override void OnGripHeld(GameObject controller) {
		//Debug.Log ("Grip Held");
	}

	public override void OnTouchpadDown(GameObject controller) {
		//Debug.Log ("Touchpad Pressed");
	}

	public override void OnTouchpadUp(GameObject controller) {
		//Debug.Log ("Touchpad Released");
	}

	public override void OnTouchpadHeld(GameObject controller) {
		//Debug.Log ("Touchpad Held");
	}

	public override void OnMenuDown(GameObject controller) {
		//Debug.Log ("Menu Pressed");
	}

	public override void OnMenuUp(GameObject controller) {
		//Debug.Log ("Menu Released");
	}

	public override void OnMenuHeld(GameObject controller) {
		//Debug.Log ("Menu Held");
	}
}
