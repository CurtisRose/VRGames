using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item {
	public int numBullets;
	public Vector3 attachLocation;
	public bool attached = false;
	public Weapon attachedWeapon;
	public string gunName;
	protected bool touchingMagazinePouch = false;
	protected bool isHolstered = false;
	public Vector3 holsterPosition = Vector3.zero;
	public Quaternion holsterRotation = Quaternion.Euler(Vector3.zero);
	public MagazinePouch magazinePouch;
	public Collider magCollider;
	public Weapon weaponToAttach;

	protected override void Start() {
		base.Start();
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.GetComponentInChildren<Weapon>()) {
			//Debug.Log ("Attempting to reload");
			if (isHeld) {
				if (!attached) {
					if (!col.gameObject.GetComponentInChildren<Weapon> ().hasMagazine) {
						//Debug.Log ("Testing magazine collision enter");
						if (col.gameObject.GetComponentInChildren<Weapon> ().gunName == gunName) {
							//Attach (col.gameObject.GetComponentInChildren<Weapon> ());
							weaponToAttach = col.gameObject.GetComponentInChildren<Weapon> ();
							col.gameObject.GetComponentInChildren<Weapon> ().Highlight (true);
						}
					}
				}
			}
		}
	}

	/*void OnTriggerStay(Collider col) {
		if (col.gameObject.GetComponent<WandController> ()) {
			col.gameObject.GetComponent<WandController> ().SetCollidingObject (gameObject);
		}
	}*/

	void OnTriggerExit(Collider col) {
		if (col.gameObject.GetComponentInChildren<Weapon>()) {
			if (isHeld) {
				col.gameObject.GetComponentInChildren<Weapon> ().Highlight (false);
				if (weaponToAttach == col.gameObject.GetComponentInChildren<Weapon> ()) {
					weaponToAttach = null;
				}
			}
		}
	}
		
	void Attach(Weapon weapon) {
		attached = true;
		holdingController.doNotSetCollidingObject = true;
		weapon.Reload (gameObject.GetComponent<Magazine>());
		attachedWeapon = weapon;
		controllerNumberHolding = 0;
		holdingController.SetCollidingObject (null);
		holdingController = null;
		SetIsHeld (false);
		weapon.Highlight (false);
	}

	public virtual void Unattach(Weapon weapon) {
		//Debug.Log ("Unattaching magazine");
		weapon.Unload (gameObject.GetComponent<Magazine>());
		weaponToAttach = null;
		weapon.Highlight (false);
		attached = false;
	}

	public override void OnGripDown(WandController controller) {
		if (attached) {
			Unattach (attachedWeapon);
			PickUp (controller);
		} else if (weaponToAttach) {
			Attach (weaponToAttach);
		} else if (GetTouchingMagazinePouch() && !GetIsHolstered () && GetIsHeld ()) {
			//Debug.Log ("Holstering magazine");
			PickUp (controller);
			SetIsHolstered (true);
			if (!magazinePouch.AddMagazine (this)) {
				//Debug.Log ("Trying to repick up magazine");
				SetIsHolstered (false);
				PickUp (controller);
			}
		} else if (GetIsHolstered ()) {
			//Debug.Log ("Unholstering magazine");
			magazinePouch.RemoveMagazine (this);
			SetTouchingMagazinePouch (false, null);
			SetIsHolstered (false);
			PickUp (controller);
		} 	else {
			if (isHeld) {
				//Debug.Log ("Dropping Magazine");
				PickUp (controller);
			} else {
				//Debug.Log ("Picking up magazine");
				PickUp (controller);
			}
		}
	}

	public bool GetTouchingMagazinePouch() {
		return touchingMagazinePouch;
	}

	public void SetTouchingMagazinePouch(bool setTouching, MagazinePouch pouch) {
		magazinePouch = pouch;
		touchingMagazinePouch = setTouching;
	}

	public bool GetIsHolstered() {
		return isHolstered;
	}

	public void SetIsHolstered(bool setHolstered) {
		isHolstered = setHolstered;
	}
}
