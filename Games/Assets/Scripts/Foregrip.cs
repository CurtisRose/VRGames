using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foregrip : Attachment {
	protected override void Start() {
		base.Start ();
		attachmentType = "Grip";
	}

	protected override void OnTriggerEnter(Collider col) {
		if (col.GetComponent<AttachmentPoint>()) {
			if (!col.GetComponent<AttachmentPoint> ().attachment) {
				if (attachmentType == col.GetComponent<AttachmentPoint> ().attachmentPointType) {
					if (!attachmentPoint) {
						attachmentPoint = col.GetComponent<AttachmentPoint> ();
						attachmentPoint.attachment = gameObject.GetComponent<Attachment> ();
						attachmentPoint.Highlight (true);
					}
				}
			}
		} else if (col.GetComponent<WandController>() && isAttached) {
			Debug.Log ("Grabbing foregrip");
			//Detach (col.GetComponent<WandController>());
			AddConfigurableJoint (col.GetComponent<WandController>());
		}
	}

	protected override void OnTriggerExit(Collider col) {
		if (!isAttached && col.GetComponent<AttachmentPoint>()) {
			if (col.GetComponent<AttachmentPoint> () == attachmentPoint) {
				Debug.Log ("Foregrip ontriggerexit");
				attachmentPoint.Highlight (false);
				attachmentPoint.attachment = null;
				//attachmentPoint = null;
			}
		} else if (col.GetComponent<WandController>()) {
			//Debug.Log ("Releasing foregrip");
			/*if (GetComponent<ConfigurableJoint> ()) {
				GetComponent<ConfigurableJoint> ().connectedBody = null;
				Destroy(GetComponent<ConfigurableJoint> ());
			}*/
		}
	}

	protected override void Attach(WandController controller) {
		controller.DropItem ();
		isHeld = false;
		isAttached = true;
		attachmentPoint.Highlight (false);
		GetComponent<Collider> ().isTrigger = false;

		if (gameObject.GetComponent<FixedJoint> ()) {
			controller.SetObjectInHand(null);
			controller.SetControllerVisible (true);
			gameObject.GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (gameObject.GetComponent<FixedJoint> ());
		}
		transform.rotation = attachmentPoint.transform.rotation;
		transform.Rotate (0, 0, 90);
		transform.parent = attachmentPoint.transform;
		transform.position = attachmentPoint.transform.position;
		Vector3 temp = transform.localPosition;
		temp.x -= 4.5f;
		transform.localPosition = temp;
		transform.parent = null;
		FixedJoint joint = AddFixedJoint();
		joint.connectedBody = attachmentPoint.transform.parent.GetComponent<Rigidbody> ();
		joint.anchor = attachmentPoint.transform.position;
		Physics.IgnoreCollision (GetComponent<Collider> (), attachmentPoint.transform.parent.GetComponent<Collider> ());
		Highlight(false);
	}
}
