using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
	public int numBullets;
	protected Quaternion magazineRotation;
	protected Vector3 magazinePosition;
	public bool hasMagazine = false;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public virtual void Reload(Magazine magazineScript) {

	}

	public virtual void Unload(Magazine magazineScript) {

	}
}
