using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	private GameObject collidingObject; // Object being touched by controller.
	private GameObject objectInHand; // Object held by controller.

	private uint controllerNumber = 0;
	private bool holdingItem = false;

	private WandController controllerScript;

	void Awake() {
		//Debug.Log ("Testing ControllerGrabItem Awake()");
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		controllerScript = gameObject.GetComponent<WandController> () as WandController;
	}

	void Start() {
		controllerNumber = controller.index;
	}

	// Sets up other collider as potential grab target.
	public void OnTriggerEnter(Collider other) {
		//Debug.Log ("Touching an object: " + other.name);
		if (!collidingObject && !objectInHand) {
			SetCollidingObject (other);
		}
	}

	// Makes sure that the target is still set.
	public void OnTriggerStay(Collider other) {
		//Debug.Log ("Still Touching an object");
		if (!collidingObject && !objectInHand) {
			SetCollidingObject (other);
		}
	}

	// Removes other as a potential grab target.
	public void OnTriggerExit(Collider other) {
		//Debug.Log ("Stopped Touching an object " + other.name);
		if (!collidingObject) {
			//Debug.Log ("I don't know when this OnTriggerExit should happen for the controllers");
			return;
		}
		else if (other == collidingObject.GetComponent<Collider>()) {
			if (other.transform.GetComponent (typeof(Item))) {
				Item itemScript = other.transform.GetComponent (typeof(Item)) as Item;
				itemScript.Highlight (false);
				collidingObject = null;
			}
		}
	}

	private void SetCollidingObject(Collider col) {
		//Debug.Log ("Setting colliding object: " + col.gameObject);
		if (col.transform.GetComponent (typeof(Item))) {
			//Debug.Log ("testing collision");
			Item itemScript = col.transform.GetComponent (typeof(Item)) as Item;
			itemScript.Highlight (true);
			collidingObject = col.gameObject;
		}
	}
		
	// Update is called once per frame
	void Update () {
		if (objectInHand) {
			//Debug.Log ("Object is in hand");
			Item objectInHandScript = objectInHand.GetComponent (typeof(Item)) as Item;
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
				//Debug.Log ("Interacting with an item with grip down");
				objectInHandScript.OnGripDown (controllerScript);
			}
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
				//Debug.Log ("Interacting with an item with trigger down");
				objectInHandScript.OnTriggerDown (controllerScript);
			}
			if (controller.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
				//Debug.Log ("Interacting with an item with trigger held");
				objectInHandScript.OnTriggerHeld (controllerScript);
			}
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
				objectInHandScript.OnTouchpadDown (controllerScript);
			}
		}
		else if (collidingObject) {
			Item objectInHandScript = collidingObject.GetComponent (typeof(Item)) as Item;
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
				//Debug.Log ("Interacting with an item with grip down");
				objectInHandScript.OnGripDown (controllerScript);
			}
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
				//Debug.Log ("Interacting with an item with trigger down");
				objectInHandScript.OnTriggerDown (controllerScript);
			}
			if (controller.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
				//Debug.Log ("Interacting with an item with trigger down");
				objectInHandScript.OnTriggerHeld (controllerScript);
			}
		}
		else {
			
		}
		if (controller.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
			Debug.Log ("Controller: " + controllerNumber);
		}
	}

	public void SetControllerVisible(bool isVisible) {
		foreach (SteamVR_RenderModel model in gameObject.GetComponentsInChildren<SteamVR_RenderModel>()) {
			foreach (var child in model.GetComponentsInChildren<MeshRenderer>()) {
				child.enabled = isVisible;
			}
		}
	}

	public void PickUpItem(GameObject item) {
		SetObjectInHand (item);
		SetHoldingItem (true);
	}

	public void DropItem() {
		SetObjectInHand (null);
		SetHoldingItem (false);
	}

	public Vector3 getVelocity() {
		return controller.velocity;
	}

	public Vector3 getAngularVelocity() {
		return controller.angularVelocity;
	}

	public void VibrateController(int time) {
		controller.TriggerHapticPulse ((ushort)time);
	}

	public GameObject GetCollidingObject() {
		return collidingObject;
	}

	public void SetCollidingObject(GameObject set) {
		collidingObject = set;
	}

	public GameObject GetObjectInHand() {
		return objectInHand;
	}

	public void SetObjectInHand(GameObject set) {
		objectInHand = set;
	}

	public uint GetControllerNumber() {
		return controllerNumber;
	}

	public bool GetHoldingItem() {
		return holdingItem;
	}

	public void SetHoldingItem(bool set) {
		holdingItem = set;
	}

	public WandController GetControllerScript() {
		return controllerScript;
	}

}