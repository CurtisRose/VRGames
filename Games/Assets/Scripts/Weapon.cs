using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
	public int numBullets;
	protected Quaternion gripRotation;
	protected Vector3 gripPosition;
	protected Quaternion magazineRotation;
	protected Vector3 magazinePosition;
	public bool hasMagazine = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void Reload(Magazine magazineScript) {

	}

	public virtual void Unload(Magazine magazineScript) {

	}

	public virtual ConfigurableJoint AddConfigurableJoint() {
		ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint> ();

		return joint;
	}
}
