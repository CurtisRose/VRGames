using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {
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
		
	// Update is called once per frame
	void Update () {
		if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip) && collidingObject) {
			Item objectInHandScript = collidingObject.GetComponent (typeof(Item)) as Item;
			objectInHandScript.OnGripDown (gameObject);
		}
	}

	public void SetControllerVisible(GameObject controller, bool isVisible) {
		foreach (SteamVR_RenderModel model in controller.GetComponentsInChildren<SteamVR_RenderModel>()) {
			foreach (var child in model.GetComponentsInChildren<MeshRenderer>()) {
				child.enabled = isVisible;
			}
		}
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