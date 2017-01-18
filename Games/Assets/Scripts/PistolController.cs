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
			gameObject.transform.GetChild(0).GetComponent<Renderer> ().material = highlightMaterial;
		} else {
			gameObject.transform.GetChild(0).GetComponent<Renderer> ().material = oldMaterial;
		}
	}

	protected override void PickUp(WandController controller) {
		if (isHeld) {
			if (controller.controllerNumber == controllerNumberHolding) {
				//Debug.Log ("Dropping Gun");
				isHeld = false;
				controller.holdingItem = false;
				controller.objectInHand = null;
				controllerNumberHolding = 0;
				controller.SetControllerVisible (true);
				if (GetComponent<FixedJoint> ()) {
					GetComponent<FixedJoint> ().connectedBody = null;
					Destroy (GetComponent<FixedJoint> ());
				}
				gameObject.GetComponent<Rigidbody> ().velocity = controller.getVelocity ();
				gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.getVelocity ();
			} else {
				//Debug.Log ("Gun Swapping Hands");
				controller.holdingItem = true;
				controllerNumberHolding = controller.controllerNumber;
				controller.objectInHand = gameObject;

				if (gameObject.GetComponent<FixedJoint> ()) {
					//otherControllerScript.holdingItem = false;
					WandController oldController = gameObject.GetComponent<FixedJoint> ().connectedBody.gameObject.GetComponent<WandController>() as WandController;
					oldController.SetControllerVisible (true);
					oldController.holdingItem = false;
					oldController.objectInHand = null;
					gameObject.GetComponent<FixedJoint> ().connectedBody = null;
					Destroy (gameObject.GetComponent<FixedJoint> ());
				}

				controller.SetControllerVisible (false);
				gameObject.transform.parent = controller.transform;
				Quaternion newRotation = Quaternion.Euler (-80, 180, 0);
				gameObject.transform.rotation = controller.transform.rotation * newRotation;
				Vector3 newPosition = new Vector3 (0.0f, 0.0f, -0.1f);
				gameObject.transform.position = newPosition;
				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				gameObject.transform.parent = null;
				Highlight (false);
			}
			//Highlight ();
		} else if (!isHeld) {
			if (!controller.holdingItem) {
				isHeld = true;
				controller.holdingItem = true;
				controllerNumberHolding = controller.controllerNumber;
				controller.objectInHand = gameObject;
				controller.SetControllerVisible (false);
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

	public override void OnTriggerDown(WandController controller) {
		//Debug.Log ("Trigger Pressed");
		if (controller.objectInHand == gameObject) { // If the hand that is holding the gun presses trigger, then shoot.
			Fire();
		}
	}

	public override void OnTriggerUp(WandController controller) {
		//Debug.Log ("Trigger Released");
	}

	public override void OnTriggerHeld(WandController controller) {
		//Debug.Log ("Trigger Held");
	}

	public override void OnHairTriggerDown(WandController controller) {
		//Debug.Log ("Hair Trigger Pressed");
	}

	public override void OnHairTriggerUp(WandController controller) {
		//Debug.Log ("Hair Trigger Released");
	}

	public override void OnHairTriggerHeld(WandController controller) {
		//Debug.Log ("Hair Trigger Held");
	}

	public override void OnGripDown(WandController controller) {
		//Debug.Log ("Grip Pressed");
		PickUp(controller);
	}

	public override void OnGripUp(WandController controller) {
		//Debug.Log ("Grip Released");
	}

	public override void OnGripHeld(WandController controller) {
		//Debug.Log ("Grip Held");
	}

	public override void OnTouchpadDown(WandController controller) {
		//Debug.Log ("Touchpad Pressed");
	}

	public override void OnTouchpadUp(WandController controller) {
		//Debug.Log ("Touchpad Released");
	}

	public override void OnTouchpadHeld(WandController controller) {
		//Debug.Log ("Touchpad Held");
	}

	public override void OnMenuDown(WandController controller) {
		//Debug.Log ("Menu Pressed");
	}

	public override void OnMenuUp(WandController controller) {
		//Debug.Log ("Menu Released");
	}

	public override void OnMenuHeld(WandController controller) {
		//Debug.Log ("Menu Held");
	}
}
