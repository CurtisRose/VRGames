using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foregrip : Attachment {
	private bool aiming = false;

	protected override void Start() {
		base.Start ();
		attachmentType = "Other";
		attachmentPosition = new Vector3 (0.0f, 10.0f , 0);
	}

	void HoldGrip (WandController controller) {
		if (!aiming) {
			aiming = true;
			Highlight (false);
			GetComponentInParent<M4A1Controller> ().SetOtherController (controller);
		} else {
			aiming = false;
			Highlight (true);
			GetComponentInParent<M4A1Controller> ().UnsetOtherController ();
		}
	}

	public override void OnTriggerDown(WandController controller) {
		HoldGrip (controller);
	}

	public override void OnGripDown(WandController controller) {
		if (!attachmentPoint) {
			PickUp (controller);
		} else {
			if (!isAttached) {
				Attach (controller);
			} else {
				if (!aiming) {
					Detach (controller);
				}
			}
		}
	}
}
