using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : Item {
	protected bool isAttachment;
	protected bool isAttached;
	protected string attachmentType;
	protected AttachmentPoint attachmentPoint;
	protected Vector3 attachmentPosition;

	protected override void Start() {
		base.Start ();
		isAttachment = true;
		isAttached = false;
		attachmentType = null;
		attachmentPoint = null;
		attachmentPosition = Vector3.zero;
	}

	protected virtual void OnTriggerEnter(Collider col) {
		if (col.GetComponent<AttachmentPoint>()) {
			if (!col.GetComponent<AttachmentPoint> ().attachment) {
				if (attachmentType == col.GetComponent<AttachmentPoint> ().attachmentPointType) {
					if (!attachmentPoint) {
						attachmentPoint = col.GetComponent<AttachmentPoint> ();
						//attachmentPoint.attachment = gameObject.GetComponent<Attachment> ();
						attachmentPoint.Highlight (true);
					}
				}
			}
		}
	}

	protected virtual void OnTriggerStay(Collider col) {
		if (col.GetComponent<AttachmentPoint>()) {
			if (!col.GetComponent<AttachmentPoint> ().attachment) {
				if (attachmentType == col.GetComponent<AttachmentPoint> ().attachmentPointType) {
					if (!attachmentPoint) {
						attachmentPoint = col.GetComponent<AttachmentPoint> ();
						//attachmentPoint.attachment = gameObject.GetComponent<Attachment> ();
						attachmentPoint.Highlight (true);
					}
				}
			}
		}
	}

	protected virtual void OnTriggerExit(Collider col) {
		if (!isAttached && col.GetComponent<AttachmentPoint>()) {
				if (col.GetComponent<AttachmentPoint> () == attachmentPoint) {
					attachmentPoint.Highlight (false);
					attachmentPoint.attachment = null;
					attachmentPoint = null;
				}
		}
	}
		
	protected virtual void Attach(WandController controller) {
		controller.DropItem ();
		isHeld = false;
		isAttached = true;
		attachmentPoint.Highlight (false);
		GetComponent<Collider> ().isTrigger = false;
		controllerNumberHolding = 0;
		attachmentPoint.attachment = GetComponent<Attachment>();
		
		if (gameObject.GetComponent<ConfigurableJoint> ()) {
			controller.SetObjectInHand(null);
			controller.SetControllerVisible (true);
			gameObject.GetComponent<ConfigurableJoint> ().connectedBody = null;
			Destroy (gameObject.GetComponent<ConfigurableJoint> ());
		}
		transform.rotation = attachmentPoint.transform.rotation;
		transform.parent = attachmentPoint.transform;
		transform.position = attachmentPoint.transform.position;
		transform.localPosition = transform.localPosition + attachmentPosition;
		transform.parent = null;
		ConfigurableJoint joint = AddConfigurableJoint(controller);
		joint.connectedBody = attachmentPoint.transform.parent.GetComponent<Rigidbody> ();
		joint.anchor = attachmentPoint.transform.position;
		Physics.IgnoreCollision (GetComponent<Collider> (), attachmentPoint.transform.parent.GetComponent<Collider> ());
		Highlight(false);
	}

	protected virtual void Detach(WandController controller) {
		//Debug.Log ("Testing Detach Attachment");
		isAttached = false;
		attachmentPoint.attachment = null;
		if (gameObject.GetComponent<ConfigurableJoint> ()) {
			gameObject.GetComponent<ConfigurableJoint> ().connectedBody = null;
			Destroy (gameObject.GetComponent<ConfigurableJoint> ());
		}
		gameObject.transform.parent = null;
		Physics.IgnoreCollision (GetComponent<Collider> (), attachmentPoint.transform.parent.GetComponent<Collider> (), false);
		PickUp (controller);
	}

	protected override void PickUp(WandController controller) {
		if (isHeld  && controller.GetControllerNumber() == controllerNumberHolding) {
			isHeld = false;
			GetComponent<Collider>().isTrigger = false;
			controller.DropItem ();
			controllerNumberHolding = 0;

			if (gameObject.GetComponent<ConfigurableJoint> ()) {
				gameObject.GetComponent<ConfigurableJoint> ().connectedBody = null;
				Destroy (gameObject.GetComponent<ConfigurableJoint> ());
			}

			gameObject.GetComponent<Rigidbody> ().velocity = controller.getVelocity ();
			gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.getAngularVelocity ();
		}
		else if (isHeld  && controller.GetControllerNumber() != controllerNumberHolding) {
			isHeld = true;
			controller.PickUpItem (gameObject);
			controllerNumberHolding = controller.GetControllerNumber();

			if (gameObject.GetComponent<ConfigurableJoint> ()) {
				controller.SetObjectInHand(null);
				controller.SetControllerVisible (true);
				gameObject.GetComponent<ConfigurableJoint> ().connectedBody = null;
				Destroy (gameObject.GetComponent<ConfigurableJoint> ());
			}

			ConfigurableJoint joint = AddConfigurableJoint(controller);
			joint.connectedBody = controller.GetComponent<Rigidbody> ();
			joint.anchor = controller.transform.position;
		} 
		else if (!isHeld) { // Picking Up
			if (!controller.GetHoldingItem()) {
				GetComponent<Collider>().isTrigger = true;
				isHeld = true;
				controller.PickUpItem (gameObject);
				controllerNumberHolding = controller.GetControllerNumber();

				ConfigurableJoint joint = AddConfigurableJoint(controller);
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				joint.anchor = controller.transform.position;
			}
		}
	}

	public override void OnGripDown(WandController controller) {
		if (!attachmentPoint) {
			PickUp (controller);
		} else {
			if (!isAttached) {
				Attach (controller);
			} else {
				Detach (controller);
			}
		}
	}
}
