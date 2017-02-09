using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinePouchHolder : Item {

	// Use this for initialization
	protected override void Start () {
		highlightObject = transform.GetChild (0).gameObject;
		base.Start ();
	}

	protected override void PickUp(WandController controller) {
		base.PickUp (controller);
		if (GetComponent<Rigidbody> ()) {
			Destroy (GetComponent<Rigidbody> ());
		}
	}
}
