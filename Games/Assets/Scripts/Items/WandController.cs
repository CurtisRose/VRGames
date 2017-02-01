﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {
	private SteamVR_TrackedObject trackedObj;
	public SteamVR_Controller.Device controller {
		get { return SteamVR_Controller.Input ((int)trackedObj.index); }
	}
	public Vector3 GetVelocity { get { return controller.velocity; } }
	public Vector3 GetAngularVelocity { get { return controller.angularVelocity; } }

	public GameObject collidingObject; // Object being touched by controller.
	public GameObject objectInHand; // Object held by controller.
	public GameObject secondaryHoldObject;

	private uint controllerNumber = 0;
	private bool holdingItem = false;

	private WandController controllerScript;

	private PlayerController playerController;

	private GameController gameController;

	public bool isReady = false;

	// Sketchy hack, originally used for magazines.
	// When you load a magazine it makes sure your colliding object isn't immediately the gun.
	public bool doNotSetCollidingObject = false;

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		controllerScript = gameObject.GetComponent<WandController> () as WandController;
		gameController = GameController.GetInstance ();
	}

	void Start() {
		controllerNumber = controller.index;
		playerController = GetComponentInParent<PlayerController> ();
		isReady = true;
	}

	// Sets up other collider as potential grab target.
	public void OnTriggerEnter(Collider other) {
		if (other.transform.GetComponent (typeof(Item))) {
			if (!collidingObject && !objectInHand && !secondaryHoldObject) {
				if (doNotSetCollidingObject) {
					//Debug.Log ("Testing");
					doNotSetCollidingObject = false;
					SetCollidingObject (null);
				} else {
					SetCollidingObject (other.gameObject);
				}
			}
		}
	}

	// Makes sure that the target is still set.
	public void OnTriggerStay(Collider other) {
		if (other.transform.GetComponent<Magazine> ()) {
			if (!objectInHand && !secondaryHoldObject) {
				if (collidingObject) {
					collidingObject.GetComponent<Item> ().Highlight (false);
				}
				SetCollidingObject (other.gameObject);
			}
		} else if (other.transform.GetComponent<Item> ()) {
			if (!collidingObject && !objectInHand && !secondaryHoldObject) {
				SetCollidingObject (other.gameObject);
			} else if (collidingObject && !objectInHand && !secondaryHoldObject) {
				collidingObject.GetComponent<Item>().Highlight(true);
			}
		}
	}

	// Removes other as a potential grab target.
	public void OnTriggerExit(Collider other) {
		if (!collidingObject) {
			//Debug.Log ("Error");
			return;
		}
		else if (other.transform.GetComponent (typeof(Item))) {
			if (other.gameObject == collidingObject) {
				Item itemScript = other.transform.GetComponent (typeof(Item)) as Item;
				itemScript.collidingController = null;
				itemScript.Highlight (false);
				collidingObject = null;
			}
		}
	}

	public bool IsAvailable() {
		return GetObjectInHand ();
	}

	public void SetCollidingObject(GameObject gameObject) {
		if (gameObject) {
			if (gameObject.transform.GetComponent<Item> ()) {
				Item itemScript = gameObject.transform.GetComponent (typeof(Item)) as Item;
				itemScript.Highlight (true);
				itemScript.collidingController = this;
				collidingObject = gameObject;
			}
		} else {
			if (collidingObject) {
				if (collidingObject.GetComponent<Item>()) {
					collidingObject.GetComponent<Item> ().collidingController = null;
					collidingObject = null;
				}
			}
			collidingObject = null;
		}
	}
		
	// Update is called once per frame
	void Update () {
		//transform.parent.transform.position = transform.position;
		//transform.parent.transform.rotation = transform.rotation;
		if (objectInHand) {
			Item objectInHandScript = objectInHand.GetComponent (typeof(Item)) as Item;
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
				objectInHandScript.OnGripDown (controllerScript);
			}
			if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Grip)) {
				objectInHandScript.OnGripUp (controllerScript);
			}
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
				objectInHandScript.OnTriggerDown (controllerScript);
			}
			if (controller.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
				objectInHandScript.OnTriggerHeld (controllerScript);
			}
			if (controller.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
				objectInHandScript.OnTriggerUp (controllerScript);
			}
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
				objectInHandScript.OnTouchpadDown (controllerScript);
			}
		}
		else if (collidingObject) {
			Item objectInHandScript = collidingObject.GetComponent (typeof(Item)) as Item;
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
				objectInHandScript.OnGripDown (controllerScript);
			}
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
				objectInHandScript.OnTriggerDown (controllerScript);
			}
			if (controller.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
				objectInHandScript.OnTriggerHeld (controllerScript);
			}
		}
		else if(secondaryHoldObject) {
			Item secondaryHoldObjectScript = secondaryHoldObject.GetComponent (typeof(Item)) as Item;
			if (controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
				secondaryHoldObjectScript.OnGripDown (controllerScript);
			}
		}
		if (controller.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
			//Debug.Log ("Controller: " + controllerNumber);
			GameController.SwitchHandMovement ();
		}
	}

	public void SetControllerVisible(bool isVisible) {
		foreach (SteamVR_RenderModel model in gameObject.GetComponentsInChildren<SteamVR_RenderModel>()) {
			foreach (var child in model.GetComponentsInChildren<MeshRenderer>()) {
				child.enabled = isVisible;
			}
		}
		GetComponent<Collider> ().enabled = isVisible;
	}

	public void PickUpItem(GameObject item) {
		SetObjectInHand (item);
		SetHoldingItem (true);
	}

	public void DropItem() {
		SetObjectInHand (null);
		SetHoldingItem (false);
		SetCollidingObject (null);
	}

	public void VibrateController(int time) {
		controller.TriggerHapticPulse ((ushort)time);
	}

	public GameObject GetCollidingObject() {
		return collidingObject;
	}

	/*public void SetCollidingObject(GameObject set) {
		collidingObject = set;
	}*/

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