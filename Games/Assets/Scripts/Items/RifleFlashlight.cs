using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleFlashlight : Attachment {
	Light light;

	// Use this for initialization
	protected override void Start () {
		highlightObject = transform.GetChild(0).gameObject;
		base.Start ();
		attachmentPosition = new Vector3 (0.0f, 23.0f ,-4.0f);
		attachmentType = "Other";
		light = GetComponentInChildren<Light> ();
	}

	void TurnFlashlightOn () {
		if (!light.enabled) {
			light.enabled = true;
		} else {
			light.enabled = false;
			
		}
	}

	public override void OnTriggerDown(WandController controller) {
		TurnFlashlightOn ();
	}
}
