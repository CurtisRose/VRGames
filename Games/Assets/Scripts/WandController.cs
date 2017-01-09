using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WandController : MonoBehaviour {
/*	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private Valve.VR.EVRButtonId dashboardButton = Valve.VR.EVRButtonId.k_EButton_Dashboard_Back;
	private Valve.VR.EVRButtonId downButton = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
	private Valve.VR.EVRButtonId upButton = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
	private Valve.VR.EVRButtonId LeftButton = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
	private Valve.VR.EVRButtonId RightButton = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
*/

	Valve.VR.EVRButtonId[] buttonIds = new Valve.VR.EVRButtonId[] {
		Valve.VR.EVRButtonId.k_EButton_ApplicationMenu,
		Valve.VR.EVRButtonId.k_EButton_Grip,
		Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad,
		Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger
	};

	Valve.VR.EVRButtonId[] axisIds = new Valve.VR.EVRButtonId[] {
		Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad,
		Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger
	};

	private SteamVR_Controller.Device controller;
	private SteamVR_TrackedObject trackedObj;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		controller =  SteamVR_Controller.Input((int) trackedObj.index);
	}
	
	// Update is called once per frame
	void Update () {
		/*if (controller.GetAxis () != Vector2.zero) {
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
	}

	void OnCollisionEnter (Collision col) {
		/*if (col.gameObject.CompareTag ("Item")) {
			Debug.Log ("Touching an item");
		} else {
			Debug.Log ("Touching something else");
		}*/
	}
}
