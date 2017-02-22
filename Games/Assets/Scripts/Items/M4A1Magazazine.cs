using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4A1Magazazine : Magazine {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		numBullets = 30;
		holsterPosition = new Vector3 (0.0f, -0.0384f, 0.009f);
		holsterRotation = Quaternion.Euler (new Vector3 (90, 0, 0));
		gunName = "M4A1";
	}
}
