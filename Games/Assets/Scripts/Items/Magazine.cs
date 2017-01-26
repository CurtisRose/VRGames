using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item {
	public int numBullets;
	public Vector3 attachLocation;
	public bool attached = false;
	public Weapon attachedWeapon;

	protected override void Start() {
		base.Start();
		numBullets = 30;
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.GetComponentInChildren<Weapon>()) {
			if (isHeld) {
				if (!attached) {
					if (!col.gameObject.GetComponentInChildren<Weapon> ().hasMagazine) {
						//Debug.Log ("Testing magazine collision enter");
						attached = true;
						Attach (col.gameObject.GetComponentInChildren<Weapon> ());
					}
				}
			}
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.gameObject.GetComponentInChildren<Weapon>()) {
			if (isHeld) {
				if (attached) {
					attached = false;
					transform.parent = null;
					attachedWeapon = null;
				}
			}
		}
	}
		
	void Attach(Weapon weapon) {
		weapon.Reload (gameObject.GetComponent<Magazine>());
		attachedWeapon = weapon;
		holdingController.SetCollidingObject(null);
		controllerNumberHolding = 0;
		holdingController = null;
	}

	void Unattach(Weapon weapon) {
		weapon.Unload (gameObject.GetComponent<Magazine>());
		// If there are less than 6 bullets then check how many, then Destroy the correct bullets in the magzine.
		if (numBullets < 6) {
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
		}
	}

	public override void OnGripDown(WandController controller) {
		if (attached) {
			Unattach (attachedWeapon);
		}
		base.PickUp (controller);
	}
}
