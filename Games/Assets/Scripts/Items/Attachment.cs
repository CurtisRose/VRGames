using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : Item {
	protected bool isAttachment;
	public bool isAttached;
	protected string attachmentType;
	public AttachmentPoint attachmentPoint;
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
		controllerNumberHolding = 0;
		attachmentPoint.attachment = GetComponent<Attachment>();
		attachmentPoint.MakeClear ();
			
		transform.rotation = attachmentPoint.transform.rotation;
		transform.position = attachmentPoint.transform.position;
		transform.parent = attachmentPoint.transform;
		Vector3 tempPosition = attachmentPoint.transform.localPosition + attachmentPosition;
		transform.localPosition = tempPosition;
		transform.parent = attachmentPoint.transform.parent;
		if (GetComponent<Rigidbody>()) {
			Destroy (GetComponent<Rigidbody> ());
		}
		Highlight(false);
	}

	protected virtual void Detach(WandController controller) {
		//Debug.Log ("Testing Detach Attachment");
		attachmentSounds [0].Play ();
		isAttached = false;
		attachmentPoint.MakeUnclear ();
		attachmentPoint.attachment = null;
		gameObject.AddComponent<Rigidbody> ();
		transform.parent = null;
		PickUp (controller);
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
