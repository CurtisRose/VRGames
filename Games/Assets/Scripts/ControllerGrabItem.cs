using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabItem : MonoBehaviour {
	private SteamVR_TrackedObject trackedObj;
	private GameObject collidingObject; // Object being touched by controller.
	private GameObject objectInHand; // Object held by controller.
	private SteamVR_Controller.Device controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}


	void Awake() {
		//Debug.Log ("Testing ControllerGrabItem Awake()");
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	// Sets up other collider as potential grab target.
	public void OnTriggerEnter(Collider other) {
		//Debug.Log ("Touching an object");
		SetCollidingObject (other);
	}

	// Makes sure that the target is still set.
	public void OnTriggerStay(Collider other) {
		//Debug.Log ("Still Touching an object");
		SetCollidingObject (other);
	}

	// Removes other as a potential grab target.
	public void OnTriggerExit(Collider other) {
		//Debug.Log ("Stopped Touching an object");
		if (!collidingObject) {
			return;
		}
		collidingObject = null;
	}

	private void SetCollidingObject(Collider col) {
		if (collidingObject || !col.GetComponent<Rigidbody> ()) {
			return;
		}
		//Debug.Log ("Setting colliding object: " + col.gameObject);
		collidingObject = col.gameObject;
	}

	private void GrabObject() {
		//Debug.Log("Grabbing object: " + collidingObject.name);
		objectInHand = collidingObject;
		collidingObject = null;
		var joint = AddFixedJoint ();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody> ();
	}

	private void ReleaseObject() {
		if (GetComponent<FixedJoint> ()) {
			//Debug.Log ("Releasing object: " + objectInHand.name);
			GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());
			objectInHand.GetComponent<Rigidbody> ().velocity = controller.velocity;
			objectInHand.GetComponent<Rigidbody> ().angularVelocity = controller.angularVelocity;
			objectInHand = null;
		}
	}

	private FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	// Update is called once per frame
	void Update () {
		if (controller.GetHairTriggerDown ()) {
			if (collidingObject && collidingObject.CompareTag("Item")) {
				GrabObject ();
			}
		}
		if (controller.GetHairTriggerUp ()) {
			ReleaseObject ();
		}
	}
}
