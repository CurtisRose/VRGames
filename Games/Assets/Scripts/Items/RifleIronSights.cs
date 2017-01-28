using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleIronSights : Attachment {

	protected override void Start () {
		highlightObject = transform.GetChild(0).gameObject;
		base.Start ();
		attachmentPosition = new Vector3 (0.0f, 0.0f, -0.3f);
		attachmentType = "Optics";
	}
}
