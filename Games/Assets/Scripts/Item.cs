using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	/* What does an item need to do....
	 * 		It must have it's own "snap-to" position and rotation defined.
	 * 		Contain action functions for button events
	 * 
	 * 
	 */

	public Material highlightMaterial;
	protected Material oldMaterial;

	protected bool isHeld = false;
	protected uint controllerNumberHolding = 0;

	public Item() {
		//Debug.Log ("Item Created");
	}

	void Awake() {
		
	}

	void Start() {
		oldMaterial = gameObject.GetComponent<Renderer> ().material;
	}

	public virtual SpringJoint AddSpringJoint() {
		SpringJoint fx = gameObject.AddComponent<SpringJoint> ();
		fx.breakForce = Mathf.Infinity;
		fx.breakTorque = Mathf.Infinity;
		fx.spring = 50.0f;
		fx.damper = 50.0f;
		fx.maxDistance = 0;
		return fx;
	}

	public virtual HingeJoint AddHingeJoint() {
		HingeJoint fx = gameObject.AddComponent<HingeJoint> ();
		fx.breakForce = 10000;
		fx.breakTorque = 10000;
		fx.axis = gameObject.transform.up;
		fx.breakForce = Mathf.Infinity;
		fx.breakForce = Mathf.Infinity;

		JointSpring hingeSpring = fx.spring;
		hingeSpring.spring = 100.0f;
		hingeSpring.damper = 50.0f;
		fx.spring = hingeSpring;
		fx.useSpring = true;
		return fx;
	}

	public virtual FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = 50000;
		fx.breakTorque = 50000;
		return fx;
	}

	public virtual void OnTriggerDown(WandController controller) {
		//Debug.Log ("Trigger Pressed");
	}

	public virtual void OnTriggerUp(WandController controller) {
		//Debug.Log ("Trigger Released");
	}

	public virtual void OnTriggerHeld(WandController controller) {
		//Debug.Log ("Trigger Held");
	}

	public virtual void OnHairTriggerDown(WandController controller) {
		//Debug.Log ("Hair Trigger Pressed");
	}

	public virtual void OnHairTriggerUp(WandController controller) {
		//Debug.Log ("Hair Trigger Released");
	}

	public virtual void OnHairTriggerHeld(WandController controller) {
		//Debug.Log ("Hair Trigger Held");
	}

	public virtual void OnGripDown(WandController controller) {
		//Debug.Log ("Grip Pressed");
		PickUp(controller);
	}

	public virtual void OnGripUp(WandController controller) {
		//Debug.Log ("Grip Released");
	}

	public virtual void OnGripHeld(WandController controller) {
		//Debug.Log ("Grip Held");
	}

	public virtual void OnTouchpadDown(WandController controller) {
		//Debug.Log ("Touchpad Pressed");
	}

	public virtual void OnTouchpadUp(WandController controller) {
		//Debug.Log ("Touchpad Released");
	}

	public virtual void OnTouchpadHeld(WandController controller) {
		//Debug.Log ("Touchpad Held");
	}

	public virtual void OnMenuDown(WandController controller) {
		//Debug.Log ("Menu Pressed");
	}

	public virtual void OnMenuUp(WandController controller) {
		//Debug.Log ("Menu Released");
	}

	public virtual void OnMenuHeld(WandController controller) {
		//Debug.Log ("Menu Held");
	}

	public virtual void Highlight(bool highlight) {
		//Debug.Log ("Highlighting Laser");
		if (highlight) {
			gameObject.GetComponent<Renderer> ().material = highlightMaterial;
		} else {
			gameObject.GetComponent<Renderer> ().material = oldMaterial;
		}
	}

	protected virtual void PickUp(WandController controller) {
		if (isHeld  && controller.controllerNumber == controllerNumberHolding) {
			//Debug.Log ("Dropping Item");
			isHeld = false;
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
}

/*	
    public override void OnTriggerDown(GameObject controller) {
		//Debug.Log ("Trigger Pressed");
	}

	public override void OnTriggerUp(GameObject controller) {
		//Debug.Log ("Trigger Released");
	}

	public override void OnTriggerHeld(GameObject controller) {
		//Debug.Log ("Trigger Held");
	}

	public override void OnHairTriggerDown(GameObject controller) {
		//Debug.Log ("Hair Trigger Pressed");
	}

	public override void OnHairTriggerUp(GameObject controller) {
		//Debug.Log ("Hair Trigger Released");
	}

	public override void OnHairTriggerHeld(GameObject controller) {
		//Debug.Log ("Hair Trigger Held");
	}

	public override void OnGripDown(GameObject controller) {
		//Debug.Log ("Grip Pressed");
	}

	public override void OnGripUp(GameObject controller) {
		//Debug.Log ("Grip Released");
	}

	public override void OnGripHeld(GameObject controller) {
		//Debug.Log ("Grip Held");
	}

	public override void OnTouchpadDown(GameObject controller) {
		//Debug.Log ("Touchpad Pressed");
	}

	public override void OnTouchpadUp(GameObject controller) {
		//Debug.Log ("Touchpad Released");
	}

	public override void OnTouchpadHeld(GameObject controller) {
		//Debug.Log ("Touchpad Held");
	}

	public override void OnMenuDown(GameObject controller) {
		//Debug.Log ("Menu Pressed");
	}

	public override void OnMenuUp(GameObject controller) {
		//Debug.Log ("Menu Released");
	}

	public override void OnMenuHeld(GameObject controller) {
		//Debug.Log ("Menu Held");
	}
	*/