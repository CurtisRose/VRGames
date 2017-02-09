using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item {
	public int numBullets;
	public Vector3 attachLocation;
	public bool attached = false;
	public Weapon attachedWeapon;
	public string gunName;
	private bool touchingMagazinePouch = false;
	private bool isHolstered = false;
	public Vector3 holsterPosition = Vector3.zero;
	public Quaternion holsterRotation = Quaternion.Euler(Vector3.zero);
	public MagazinePouch magazinePouch;
	public Collider magCollider;

	protected override void Start() {
		base.Start();
		numBullets = 30;
		holsterPosition = new Vector3 (0.0f, -0.0384f, 0.009f);
		holsterRotation = Quaternion.Euler (new Vector3 (90, 0, 0));
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.GetComponentInChildren<Weapon>()) {
			//Debug.Log ("Attempting to reload");
			if (isHeld) {
				if (!attached) {
					if (!col.gameObject.GetComponentInChildren<Weapon> ().hasMagazine) {
						//Debug.Log ("Testing magazine collision enter");
						if (col.gameObject.GetComponentInChildren<Weapon> ().gunName == gunName) {
							Attach (col.gameObject.GetComponentInChildren<Weapon> ());
						}
					}
				}
			}
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.GetComponent<WandController> ()) {
			col.gameObject.GetComponent<WandController> ().SetCollidingObject (gameObject);
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.GetComponentInChildren<Weapon>()) {
			if (isHeld) {
				if (attached) {
					attached = false;
					transform.parent = holdingController.transform;
					attachedWeapon = null;
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
	}

	void Unattach(Weapon weapon) {
		weapon.Unload (gameObject.GetComponent<Magazine>());
		// If there are less than 6 bullets then check how many, then Destroy the correct bullets in the magzine.
		/*if (numBullets < 6) {
			if (numBullets == 5) {
				if (transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).GetChild (0).childCount > 0) {
					Destroy (transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).GetChild (0).GetChild (0).gameObject);
				}
			} else if (numBullets == 4) {
				if (transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).childCount > 0) {
					Destroy (transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).GetChild (0).gameObject);
				}
			} else if (numBullets == 3) {
				if (transform.GetChild (0).GetChild (0).GetChild (0).childCount > 0) {
					Destroy (transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).gameObject);
				}
			} else if (numBullets == 2) {
				if (transform.GetChild (0).GetChild (0).childCount > 0) {
					Destroy (transform.GetChild (0).GetChild (0).GetChild (0).gameObject);
				}
			} else if (numBullets == 1) {
				if (transform.GetChild (0).childCount > 0) {
					Destroy (transform.GetChild (0).GetChild (0).gameObject);
				}
			} else if (numBullets == 0) {
				if (transform.childCount > 0) {
					Destroy (transform.GetChild (0).gameObject);
				}
			} 
		}*/
	}

	public override void OnGripDown(WandController controller) {
		if (attached) {
			Unattach (attachedWeapon);
		}
		//Debug.Log (GetTouchingMagazinePouch ());
		//Debug.Log (!GetIsHolstered ());
		//Debug.Log (GetIsHeld ());
		if (GetTouchingMagazinePouch() && !GetIsHolstered () && GetIsHeld ()) {
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
			//Debug.Log ("Doing something else with magazine");
			PickUp (controller);
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
