using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolController  : Weapon {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 1000;

	protected override void Start () {
		highlightObject = transform.GetChild (1).gameObject;
		base.Start ();
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
			if (controller.GetControllerNumber() == controllerNumberHolding) {
				//Debug.Log ("Dropping Gun");
				isHeld = false;
				controller.DropItem ();
				controllerNumberHolding = 0;
				controller.SetControllerVisible (true);
				if (GetComponent<FixedJoint> ()) {
					GetComponent<FixedJoint> ().connectedBody = null;
					Destroy (GetComponent<FixedJoint> ());
				}
				gameObject.GetComponent<Rigidbody> ().velocity = controller.GetVelocity;
				gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.GetVelocity;
			} else {
				//Debug.Log ("Gun Swapping Hands");
				controller.PickUpItem(gameObject);
				controllerNumberHolding = controller.GetControllerNumber();

				if (gameObject.GetComponent<FixedJoint> ()) {
					//otherControllerScript.holdingItem = false;
					WandController oldController = gameObject.GetComponent<FixedJoint> ().connectedBody.gameObject.GetComponent<WandController>() as WandController;
					oldController.SetControllerVisible (true);
					oldController.DropItem ();
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
			if (!controller.GetHoldingItem()) {
				isHeld = true;
				controller.PickUpItem (gameObject);
				controllerNumberHolding = controller.GetControllerNumber();
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
		if (controller.GetObjectInHand() == gameObject) { // If the hand that is holding the gun presses trigger, then shoot.
			Fire();
		}
	}
}
