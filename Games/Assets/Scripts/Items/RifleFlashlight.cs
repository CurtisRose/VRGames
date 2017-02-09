using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleFlashlight : Attachment {
	Light flashlight;

	// Use this for initialization
	protected override void Start () {
		highlightObject = transform.GetChild(0).gameObject;
		base.Start ();
		attachmentPosition = new Vector3 (0.0f, 23.0f ,-4.0f);
		attachmentType = "Other";
		flashlight = GetComponentInChildren<Light> ();
	}

	void TurnFlashlightOn () {
		if (!flashlight.enabled) {
			flashlight.enabled = true;
		} else {
			flashlight.enabled = false;
			
		}
	}

	public override void OnTriggerDown(WandController controller) {
		TurnFlashlightOn ();
	}
}
