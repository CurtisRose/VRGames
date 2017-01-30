using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
	public int numBullets;
	protected Quaternion magazineRotation;
	protected Vector3 magazinePosition;
	public bool hasMagazine = false;
	public string gunName;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	public virtual void Reload(Magazine magazineScript) {

	}

	public virtual void Unload(Magazine magazineScript) {

	}

	public Quaternion GetGripRotation() {
		return gripRotation;
	}
}
