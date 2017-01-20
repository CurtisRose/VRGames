using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScope : Attachment {

	protected override void Start () {
		highlightObject = transform.GetChild (0).GetChild (0).gameObject;
		base.Start ();
		attachmentType = "Optics";
		attachmentPosition = new Vector3 (4, 0 ,0);
	}
}
