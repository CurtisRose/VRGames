using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foregrip : Attachment {
	private bool held = false;
	WandController heldController;
	protected override void Start() {
		base.Start ();
		attachmentType = "Grip";
		attachmentPosition = new Vector3 (-4.0f, 0 , 0);
		attachmentRotation = new Vector3 (0, 0, 90);
	}


	private ConfigurableJoint AddConfigurableJointForeGrip() {
		Debug.Log ("Adding foregrip joint");
		ConfigurableJoint otherJoint = attachmentPoint.transform.parent.gameObject.AddComponent<ConfigurableJoint> ();
		otherJoint.angularXMotion = ConfigurableJointMotion.Free;
		otherJoint.angularYMotion = ConfigurableJointMotion.Free;
		otherJoint.angularZMotion = ConfigurableJointMotion.Free;
		ConfigurableJoint joint = attachmentPoint.transform.parent.gameObject.AddComponent<ConfigurableJoint> ();
		joint.axis = new Vector3 (1, 0, 0);
		joint.secondaryAxis = new Vector3 (0, 1, 0);
		joint.anchor = new Vector3 (0, 0.3f, 0);
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Limited;
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
		SoftJointLimit limit = new SoftJointLimit();
		limit.limit = .4f;
		joint.linearLimit = limit;
		limit.limit = 0f;
		joint.highAngularXLimit = limit;
		joint.angularYLimit = limit;
		joint.angularZLimit = limit;
		joint.breakForce = 1000f;
		return joint;
	}

	protected override void OnTriggerEnter(Collider col) {
		if (col.GetComponent<AttachmentPoint>()) {
			if (!col.GetComponent<AttachmentPoint> ().attachment) {
				if (attachmentType == col.GetComponent<AttachmentPoint> ().attachmentPointType) {
					if (!attachmentPoint) {
						attachmentPoint = col.GetComponent<AttachmentPoint> ();
						attachmentPoint.Highlight (true);
					}
				}
			}
		} else if (col.GetComponent<WandController>() && isAttached && !held) {
			Debug.Log ("Grabbing foregrip");
			held = true;
			attachmentPoint.GetComponentInParent<ConfigurableJoint> ().angularXMotion = ConfigurableJointMotion.Limited;
			attachmentPoint.GetComponentInParent<ConfigurableJoint> ().angularYMotion = ConfigurableJointMotion.Limited;
			attachmentPoint.transform.parent.LookAt (-col.GetComponent<WandController>().transform.position);
			//attachmentPoint.GetComponentInParent<ConfigurableJoint> ().swapBodies = true;
			ConfigurableJoint joint = AddConfigurableJointForeGrip ();
			joint.connectedBody = col.GetComponent<WandController> ().gameObject.GetComponent<Rigidbody> ();
			joint.swapBodies = true;
		}
	}

	protected override void OnTriggerExit(Collider col) {
		if (!isAttached && col.GetComponent<AttachmentPoint>()) {
			if (col.GetComponent<AttachmentPoint> () == attachmentPoint) {
				//Debug.Log ("Foregrip ontriggerexit");
				attachmentPoint.Highlight (false);
				attachmentPoint.attachment = null;
				attachmentPoint = null;
			}
		}
	}
}
