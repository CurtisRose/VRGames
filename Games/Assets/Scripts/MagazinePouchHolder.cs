using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinePouchHolder : Item {
	Transform parent;
	// Use this for initialization
	protected override void Start () {
		parent = transform.parent;
		highlightObject = transform.GetChild (0).gameObject;
		base.Start ();
	}

	protected override void PickUp(WandController controller) {
		base.PickUp (controller);
		if (GetComponent<Rigidbody> ()) {
			Destroy (GetComponent<Rigidbody> ());
		}
		if (!GetIsHeld ()) {
			transform.parent = parent;
		}
	}
}
