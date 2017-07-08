using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMPMagazine : Magazine {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		numBullets = 30;
		holsterPosition = new Vector3 (0.0f, -0.076f, 0.082f);
		holsterRotation = Quaternion.Euler (new Vector3 (-90, 0, 0));
		gunName = "UMP";
	}
}
