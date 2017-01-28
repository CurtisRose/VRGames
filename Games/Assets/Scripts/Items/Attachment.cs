using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : Item {
	protected bool isAttachment;
	protected bool isAttached;
	protected string attachmentType;
	protected AttachmentPoint attachmentPoint;
	protected Vector3 attachmentPosition;
	protected Quaternion attachmentRotation;
	private AudioSource[] attachmentSounds;

	protected override void Start() {
		base.Start ();
		isAttachment = true;
		isAttached = false;
		attachmentType = null;
		attachmentPoint = null;
		attachmentPosition = Vector3.zero;
		attachmentRotation = Quaternion.Euler(Vector3.zero);
		attachmentSounds = GetComponents<AudioSource>();
	}

	protected virtual void OnTriggerEnter(Collider col) {
		if (col.GetComponent<AttachmentPoint>()) {
			if (!col.GetComponent<AttachmentPoint> ().attachment) {
				if (attachmentType == col.GetComponent<AttachmentPoint> ().attachmentPointType) {
					if (!attachmentPoint) {
						attachmentPoint = col.GetComponent<AttachmentPoint> ();
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
		attachmentSounds [0].Play ();
		controller.DropItem ();
		isHeld = false;
		isAttached = true;
		attachmentPoint.Highlight (false);
		//attachmentPoint.ToggleCollider ();
		GetComponent<Collider> ().isTrigger = false;
		controllerNumberHolding = 0;
		attachmentPoint.attachment = GetComponent<Attachment>();
		attachmentPoint.MakeClear ();
		
		if (gameObject.GetComponent<ConfigurableJoint> ()) {
			controller.SetObjectInHand(null);
			controller.SetControllerVisible (true);
			gameObject.GetComponent<ConfigurableJoint> ().connectedBody = null;
			Destroy (gameObject.GetComponent<ConfigurableJoint> ());
		}
			
		transform.rotation = attachmentPoint.transform.rotation;
		transform.position = attachmentPoint.transform.position;
		transform.parent = attachmentPoint.transform;
		Vector3 tempPosition = attachmentPoint.transform.localPosition + attachmentPosition;
		transform.localPosition = tempPosition;
		transform.parent = null;

		FixedJoint joint = AddFixedJoint();
		joint.connectedBody = attachmentPoint.transform.parent.GetComponent<Rigidbody> ();
		joint.anchor = attachmentPoint.transform.localPosition;
		Highlight(false);
	}

	protected virtual void Detach(WandController controller) {
		//Debug.Log ("Testing Detach Attachment");
		attachmentSounds [0].Play ();
		isAttached = false;
		//attachmentPoint.ToggleCollider ();
		attachmentPoint.MakeUnclear ();
		attachmentPoint.attachment = null;
		if (GetComponent<ConfigurableJoint> ()) {
			GetComponent<ConfigurableJoint> ().connectedBody = null;
			Destroy (GetComponent<ConfigurableJoint> ());
		}
		if (GetComponent<FixedJoint> ()) {
			GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (GetComponent<FixedJoint> ());
		}
		transform.parent = null;
		//Physics.IgnoreCollision (GetComponent<Collider> (), attachmentPoint.transform.parent.GetComponent<Collider> (), false);
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

			//gameObject.GetComponent<Rigidbody> ().velocity = controller.GetVelocity;
			//gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.GetAngularVelocity;
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

			ConfigurableJoint joint = AddConfigurableJoint();
			joint.connectedBody = controller.GetComponent<Rigidbody> ();
			joint.anchor = controller.transform.position;
		} 
		else if (!isHeld) { // Picking Up
			if (!controller.GetHoldingItem()) {
				GetComponent<Collider>().isTrigger = true;
				isHeld = true;
				controller.PickUpItem (gameObject);
				controllerNumberHolding = controller.GetControllerNumber();

				ConfigurableJoint joint = AddConfigurableJoint();
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
