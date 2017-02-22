using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1911Magazine : Magazine {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		numBullets = 7;
		holsterPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		holsterRotation = Quaternion.Euler (new Vector3 (90, 0, 0));
		gunName = "M1911";
	}

}
