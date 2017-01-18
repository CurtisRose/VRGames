using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScope : Attachment {

	void Start () {
		isAttachment = true;
		isAttached = false;
		attachmentType = "Optics";

		oldMaterial = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer> ().material;
	}

	protected override void Attach(WandController controller) {
		controller.objectInHand = null;
		controller.holdingItem = false;
		isHeld = false;
		isAttached = true;
		attachmentPoint.Highlight (false);
		GetComponent<Collider> ().isTrigger = false;

		if (gameObject.GetComponent<FixedJoint> ()) {
			controller.objectInHand = null;
			controller.SetControllerVisible (true);
			gameObject.GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (gameObject.GetComponent<FixedJoint> ());
		}
		transform.rotation = attachmentPoint.transform.rotation;
		transform.parent = attachmentPoint.transform;
		transform.position = attachmentPoint.transform.position;
		Vector3 temp = transform.localPosition;
		// Needs to be fixed. 3 should not be hard coded. Couldn't figure it out.
		temp.x += 3;
		transform.localPosition = temp;
		transform.parent = null;
		FixedJoint joint = AddFixedJoint();
		joint.connectedBody = attachmentPoint.transform.parent.GetComponent<Rigidbody> ();
		joint.anchor = attachmentPoint.transform.position;
		Physics.IgnoreCollision (GetComponent<Collider> (), attachmentPoint.transform.parent.GetComponent<Collider> ());
		Highlight(false);
	}

	public override void Highlight(bool highlight) {
		//Debug.Log ("Highlighting Laser");
		if (highlight) {
			gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer> ().material = highlightMaterial;
		} else {
			gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer> ().material = oldMaterial;
		}
	}
}
