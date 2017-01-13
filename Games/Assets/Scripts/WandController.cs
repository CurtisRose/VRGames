using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	private GameObject collidingObject; // Object being touched by controller.
	public GameObject objectInHand; // Object held by controller.

	public uint controllerNumber = 0;
	public bool holdingItem = false;

	WandController controllerScript;

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
		if (!collidingObject) {
			SetCollidingObject (other);
		}
	}

	// Makes sure that the target is still set.
	public void OnTriggerStay(Collider other) {
		//Debug.Log ("Still Touching an object");
		if (!collidingObject || !objectInHand) {
			SetCollidingObject (other);
		}
	}

	// Removes other as a potential grab target.
	public void OnTriggerExit(Collider other) {
		//Debug.Log ("Stopped Touching an object " + other.name);
		if (!collidingObject) {
			return;
		}
		if (other.CompareTag ("Item")) {
			Item itemScript = other.transform.GetComponent (typeof(Item)) as Item;
			itemScript.Highlight (false);
			collidingObject = null;
		}
	}

	private void SetCollidingObject(Collider col) {
		//Debug.Log ("Setting colliding object: " + col.gameObject);
		if (col.CompareTag ("Item")) {
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

	public Vector3 getVelocity() {
		return controller.velocity;
	}

	public Vector3 getAngularVelocity() {
		return controller.angularVelocity;
	}
}






////////////////////////////////////////////////////////////////////
/// Random things to keep for later maybe
/// 
/*	
private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
private Valve.VR.EVRButtonId dashboardButton = Valve.VR.EVRButtonId.k_EButton_Dashboard_Back;
private Valve.VR.EVRButtonId downButton = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
private Valve.VR.EVRButtonId upButton = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
private Valve.VR.EVRButtonId LeftButton = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
private Valve.VR.EVRButtonId RightButton = Valve.VR.EVRButtonId.k_EButton_DPad_Right;

Valve.VR.EVRButtonId[] buttonIds = new Valve.VR.EVRButtonId[] {
	Valve.VR.EVRButtonId.k_EButton_ApplicationMenu,
	Valve.VR.EVRButtonId.k_EButton_Grip,
	Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad,
	Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger};

Valve.VR.EVRButtonId[] axisIds = new Valve.VR.EVRButtonId[] {
	Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad,
	Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger
};

if (controller.GetAxis () != Vector2.zero) {
	//Debug.Log (gameObject.name + " " + controller.GetAxis ());
}
if (controller.GetHairTriggerDown ()) {
	//Debug.Log (gameObject.name + "Trigger Pressed");
}
if (controller.GetHairTriggerUp ()) {
	//Debug.Log (gameObject.name + "Trigger Released");
}
if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
	//Debug.Log (gameObject.name + "Grip Pressed");
}
if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Grip)) {
	//Debug.Log (gameObject.name + "Grip Released");
}
// Need to pull harder on the trigger to activate this.
if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
	//Debug.Log (gameObject.name + "New Trigger Pressed");
}
if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
	//Debug.Log (gameObject.name + "New Trigger Released");
}*/