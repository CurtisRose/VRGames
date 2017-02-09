using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
	public int numBullets;
	protected Quaternion magazineRotation;
	protected Vector3 magazinePosition;
	public bool hasMagazine = false;
	public string gunName;
	private bool touchingHolster = false;
	private bool isHolstered = false;
	public Vector3 holsterPosition = Vector3.zero;
	public Quaternion holsterRotation = Quaternion.Euler(Vector3.zero);
	public Holster holster;
	protected WandController otherController;
	protected bool primaryWeapon = false;


	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	public virtual void Reload(Magazine magazineScript) {

	}

	public virtual void Unload(Magazine magazineScript) {

	}

	protected override void PickUp(WandController controller) {
		base.PickUp (controller);
		GetComponent<Collider> ().isTrigger = false;
		if (GetIsHeld ()) {
			controller.SetControllerVisible (false);
		} else {
			controller.SetControllerVisible (true);
		}
	}

	public void SetOtherController(WandController controller) {
		if (holdingController) {
			otherController = controller;
			controller.secondaryHoldObject = gameObject;
			otherController.SetControllerVisible (false);
		}
	}

	public void UnsetOtherController () {
		if (otherController) {
			if (otherController.secondaryHoldObject) {
				otherController.secondaryHoldObject = null;
			}
			otherController.SetCollidingObject (null);
			otherController.SetControllerVisible (true);
			otherController = null;
		}
	}

	public WandController GetOtherController () {
		return otherController;
	}

	public override void OnGripDown(WandController controller) {
		//Debug.Log ("Pressing grips on weapon");
		//Debug.Log (GetTouchingHolster());
		//Debug.Log (!GetIsHolstered());
		//Debug.Log (GetIsHeld ());
		if (GetTouchingHolster() && !GetIsHolstered () && GetIsHeld ()) {
			//Debug.Log ("Holstering Weapon");
			bool isLeft = controller.isLeftcontroller;
			PickUp (controller);
			if (!holster.HolsterWeapon (this, isLeft)) {
				PickUp (controller);
			}

		} else if (GetIsHolstered ()) {
			//Debug.Log ("Unholstering weapon");
			SetTouchingHolster (false);
			holster.UnholsterWeapon (this);
			PickUp (controller);
		} 	else if (!holdingController) {
			//Debug.Log ("Pick up Weapon");
			PickUp (controller);
		} else if (holdingController && controllerNumberHolding == controller.GetControllerNumber()) {
			//Debug.Log ("Drop Weapon");
			PickUp (controller);
			UnsetOtherController ();
		}else {
			if (!otherController) {
				//Debug.Log ("Aim Weapon");
				SetOtherController (controller);
				Highlight (false);
			} else {
				//Debug.Log ("Unaim Weapon");
				gameObject.transform.rotation = holdingController.transform.rotation * gripRotation;
				UnsetOtherController ();
			}
		}
	}

	public Quaternion GetGripRotation() {
		return gripRotation;
	}

	public bool GetTouchingHolster() {
		return touchingHolster;
	}

	public void SetTouchingHolster(bool setTouching) {
		touchingHolster = setTouching;
	}

	public bool GetIsHolstered() {
		return isHolstered;
	}

	public void SetIsHolstered(bool setHolstered) {
		isHolstered = setHolstered;
	}

	public bool GetIsPrimaryWeapon() {
		return primaryWeapon;
	}

	public void SetIsPrimaryWeapon(bool setPrimary) {
		primaryWeapon = setPrimary;
	}
}
