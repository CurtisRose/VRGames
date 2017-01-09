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
		objectInHand = collidingObject;
		collidingObject = null;

		objectInHand.transform.parent = gameObject.transform;
		objectInHand.GetComponent<Rigidbody> ().useGravity = false;
		objectInHand.GetComponent<Rigidbody> ().isKinematic = true;
	}

	private void GrabWeapon() {
		//Debug.Log("Grabbing weapon: " + collidingObject.name);
		objectInHand = collidingObject;
		collidingObject = null;

		objectInHand.transform.parent = gameObject.transform;
		objectInHand.GetComponent<Rigidbody> ().useGravity = false;
		objectInHand.GetComponent<Rigidbody> ().isKinematic = true;

		//Rotate the gun properly
		Quaternion newRotation = Quaternion.Euler (-75, 180, 0);
		objectInHand.transform.rotation = controller.transform.rot * newRotation;

		// Position the gun properly
		Vector3 newPosition = new Vector3(0.0f, -0.25f, 0.1f);
		objectInHand.transform.localPosition = newPosition;

		// Make controller dissapear
		SetControllerVisible(gameObject, false);
	}

	private void ReleaseObject() {
		// Make controller reappear
		SetControllerVisible(gameObject, true);
		if (objectInHand.transform.parent != null) {
			objectInHand.transform.parent = null;
		}
		objectInHand.GetComponent<Rigidbody> ().useGravity = true;
		objectInHand.GetComponent<Rigidbody> ().isKinematic = false;
		objectInHand.GetComponent<Rigidbody> ().velocity = controller.velocity;
		objectInHand.GetComponent<Rigidbody> ().angularVelocity = controller.angularVelocity;
		objectInHand = null;
	}

	private FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 20000; // Mathf.Infinity instead maybe
		fx.breakTorque = 20000;
		return fx;
	}

	// Update is called once per frame
	void Update () {
		if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip) && !objectInHand) {
			if (collidingObject && collidingObject.CompareTag("Item")) {
				GrabObject ();
			}
			if (collidingObject && collidingObject.CompareTag("Weapon")) {
				GrabWeapon ();
			}
		}
		else if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip) && objectInHand) {
			ReleaseObject ();
		}
	}

	void SetControllerVisible(GameObject controller, bool isVisible) {
		foreach (SteamVR_RenderModel model in controller.GetComponentsInChildren<SteamVR_RenderModel>()) {
			foreach (var child in model.GetComponentsInChildren<MeshRenderer>()) {
				child.enabled = isVisible;
			}
		}
	}
}
