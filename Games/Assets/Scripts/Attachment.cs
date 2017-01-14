using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : Item {
	public bool isAttachment = false;
	public bool isAttached = false;
	public string attachmentType = null;
	public AttachmentPoint attachmentPoint;

	protected virtual void OnTriggerEnter(Collider col) {
		if (col.GetComponent<AttachmentPoint>()) {
			attachmentPoint = col.GetComponent<AttachmentPoint> ();
			attachmentPoint.Highlight (true);
		}
	}
	
	protected virtual void OnTriggerExit(Collider col) {
		if (!isAttached && col.GetComponent<AttachmentPoint>()) {
				if (col.GetComponent<AttachmentPoint> () == attachmentPoint) {
					attachmentPoint.Highlight (false);
					attachmentPoint = null;
				}
		}
	}

	/*protected virtual void OnCollisionStay(Collision col) {

	}*/

	/*protected virtual void OnCollisionExit(Collision col) {
		if (col.gameObject.GetComponentInChildren<AttachmentPoint> ()) {
			//AttachmentPoint atachmentPoint = col.gameObject.GetComponent<AttachmentPoint> () as AttachmentPoint;
			if (!isAttached) {
				if (attachmentPoint) {
					attachmentPoint.Highlight (false);
					attachmentPoint = null;
				}
			}
		}
	}*/
		
	protected virtual void Attach(WandController controller) {
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
		transform.position = attachmentPoint.transform.position;
		transform.rotation = attachmentPoint.transform.rotation;
		FixedJoint joint = AddFixedJoint();
		joint.connectedBody = attachmentPoint.transform.parent.GetComponent<Rigidbody> ();
		joint.anchor = attachmentPoint.transform.position;
		Physics.IgnoreCollision (GetComponent<Collider> (), attachmentPoint.transform.parent.GetComponent<Collider> ());
		Highlight(false);
	}

	protected virtual void Detach(WandController controller) {
		//Debug.Log ("Testing Detach Attachment");
		isAttached = false;
		if (gameObject.GetComponent<FixedJoint> ()) {
			gameObject.GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (gameObject.GetComponent<FixedJoint> ());
		}
		Physics.IgnoreCollision (GetComponent<Collider> (), attachmentPoint.transform.parent.GetComponent<Collider> (), false);
		PickUp (controller);
	}

	protected override void PickUp(WandController controller) {
		if (isHeld  && controller.controllerNumber == controllerNumberHolding) {
			//Debug.Log ("Dropping Item");
			isHeld = false;
			GetComponent<Collider>().isTrigger = false;
			controller.holdingItem = false;
			controllerNumberHolding = 0;
			controller.objectInHand = null;

			//controllerScipt.SetControllerVisible (controller, true);

			if (gameObject.GetComponent<FixedJoint> ()) {
				gameObject.GetComponent<FixedJoint> ().connectedBody = null;
				Destroy (gameObject.GetComponent<FixedJoint> ());
			}

			gameObject.GetComponent<Rigidbody> ().velocity = controller.getVelocity ();
			gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.getAngularVelocity ();
			//Highlight ();
		}
		else if (isHeld  && controller.controllerNumber != controllerNumberHolding) {
			//Debug.Log ("Item Switching Hands");
			isHeld = true;
			controller.holdingItem = true;
			controllerNumberHolding = controller.controllerNumber;
			controller.objectInHand = gameObject;

			//controllerScipt.SetControllerVisible (controller, false);

			if (gameObject.GetComponent<FixedJoint> ()) {
				//otherControllerScript.holdingItem = false;
				controller.objectInHand = null;
				controller.SetControllerVisible (true);
				gameObject.GetComponent<FixedJoint> ().connectedBody = null;
				Destroy (gameObject.GetComponent<FixedJoint> ());
			}

			FixedJoint joint = AddFixedJoint();
			joint.connectedBody = controller.GetComponent<Rigidbody> ();
			joint.anchor = controller.transform.position;
			Highlight(false);
		} 
		else if (!isHeld) { // Picking Up
			if (!controller.holdingItem) {
				//Debug.Log ("Picking Up Item");
				GetComponent<Collider>().isTrigger = true;
				isHeld = true;
				controller.holdingItem = true;
				controllerNumberHolding = controller.controllerNumber;
				controller.objectInHand = gameObject;

				//controllerScipt.SetControllerVisible (controller, false);

				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				joint.anchor = controller.transform.position;
				Highlight(false);
			}
		}
	}

	public override void OnGripDown(WandController controller) {
		if (!attachmentPoint) {
			PickUp (controller);
		} else {
			//Debug.Log ("Trying to attach");
			if (!isAttached) {
				//Debug.Log ("Attaching");
				Attach (controller);
			} else {
				//Debug.Log ("Detaching");
				Detach (controller);
			}
		}
	}
}
