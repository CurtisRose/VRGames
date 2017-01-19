﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item {
	public int numBullets;
	public Vector3 attachLocation;
	public bool attached = false;
	public Weapon attachedWeapon;

	void Start() {
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
					//Debug.Log ("Testing magazine collision exit");
					attached = false;
					transform.parent = null;
					attachedWeapon = null;
					//Unattach (col.gameObject.GetComponentInChildren<Weapon> ());
				}
			}
		}
	}
		
	void Attach(Weapon weapon) {
		weapon.Reload (gameObject.GetComponent<Magazine>());
		attachedWeapon = weapon;
		holdingController.collidingObject = null;
		controllerNumberHolding = 0;
		holdingController = null;
	}

	void Unattach(Weapon weapon) {
		weapon.Unload (gameObject.GetComponent<Magazine>());
		if (numBullets == 3) {
			if (transform.GetChild (0).GetChild (0).GetChild (0).childCount > 0) {
				Destroy (transform.GetChild (0).GetChild (0).GetChild (0).GetChild (0).gameObject);
			}
		}
		else if (numBullets == 2) {
			if (transform.GetChild (0).GetChild (0).childCount > 0) {
				Destroy (transform.GetChild (0).GetChild (0).GetChild (0).gameObject);
			}
		} 
		else if (numBullets == 1) {
			if (transform.GetChild (0).childCount > 0) {
				Destroy (transform.GetChild (0).GetChild (0).gameObject);
			}
		} 
		else if (numBullets == 0) {
			if (transform.childCount > 0) {
				Destroy (transform.GetChild (0).gameObject);
			}
		} 
		//attachedWeapon = null;
	}

	public override void OnGripDown(WandController controller) {
		//Debug.Log ("Grip Pressed");
		if (attached) {
			Unattach (attachedWeapon);
		}
		PickUp(controller);

	}
}
