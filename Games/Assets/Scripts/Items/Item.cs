using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Notifier {
	/* What does an item need to do....
	 * 		It must have it's own "snap-to" position and rotation defined.
	 * 		Contain action functions for button events
	 * 
	 * 
	 */
	public Material highlightMaterial;
	protected Material oldMaterial;
	protected GameObject highlightObject;

	protected bool isHeld = false;
	protected uint controllerNumberHolding = 0;
	public WandController holdingController;

	protected bool hasGripPosition = false;
	protected Quaternion gripRotation;
	protected Vector3 gripPosition;

	protected float throwStrength = 2.0f;

	protected override void Start() {
		base.Start ();
		if (highlightObject) {
			oldMaterial = highlightObject.GetComponent<Renderer> ().material;
			highlightObject.GetComponent<Renderer> ().sharedMaterials = new Material[] {
				highlightObject.GetComponent<Renderer> ().material,
				highlightObject.GetComponent<Renderer> ().material
			};
		} else {
			oldMaterial = GetComponent<Renderer> ().material;
			GetComponent<Renderer> ().sharedMaterials = new Material[] {
				GetComponent<Renderer> ().material,
				GetComponent<Renderer> ().material
			};
		}
	}

	protected virtual void Update() {
		// Notify manager of position changes
		//if (transform.hasChanged) {
			//string[] positionArgs = { (transform.position.x + 1) + "", transform.position.y + "", transform.position.z + ""};
			//notify (this.encode(id + "", AttributeNames.position, positionArgs));
		//}

		// TODO: Notify manager of rotation
	}

	public override void decode (string message) {
		string[] separatedMessage = message.Split (AttributeNames.delimiter);
		if (separatedMessage [1].Equals(AttributeNames.position)) {
			Vector3 newPosition = new Vector3 (float.Parse(separatedMessage[2]), 
											   float.Parse(separatedMessage[3]), 
											   float.Parse(separatedMessage[4]));
			transform.position = newPosition;
		}
	}

	protected virtual SpringJoint AddSpringJoint() {
		SpringJoint fx = gameObject.AddComponent<SpringJoint> ();
		fx.breakForce = Mathf.Infinity;
		fx.breakTorque = Mathf.Infinity;
		fx.spring = 50.0f;
		fx.damper = 50.0f;
		fx.maxDistance = 0;
		return fx;
	}

	protected virtual HingeJoint AddHingeJoint() {
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

	protected virtual FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint> ();
		fx.breakForce = Mathf.Infinity;
		fx.breakTorque = Mathf.Infinity;
		fx.enableCollision = false;
		//fx.enablePreprocessing = false;
		return fx;
	}

	protected virtual ConfigurableJoint AddConfigurableJoint() {
		ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint> ();
		//joint.axis = new Vector3 (1, 0, 0);
		//joint.secondaryAxis = new Vector3 (0, 1, 0);
		//joint.anchor = new Vector3 (0, -0.2f, 0);
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
		/*SoftJointLimit limit = new SoftJointLimit ();
		limit.limit = 0f;
		joint.highAngularXLimit = limit;
		joint.angularYLimit = limit;
		joint.angularZLimit = limit;*/
		return joint;
	}

	protected virtual void PickUp(WandController controller) {
		if (isHeld  && controller.GetControllerNumber() == controllerNumberHolding) {
			isHeld = false;
			controller.DropItem ();
			controllerNumberHolding = 0;
			holdingController = null;
			GetComponent<Collider>().isTrigger = false;

			if (GetComponent<ConfigurableJoint> ()) {
				GetComponent<ConfigurableJoint> ().connectedBody = null;
				Destroy (GetComponent<ConfigurableJoint> ());
			}
			transform.parent = null;
			gameObject.AddComponent<Rigidbody> ();
			GetComponent<Rigidbody> ().velocity = controller.GetVelocity;
			gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.GetAngularVelocity;
		}
		else if (isHeld  && controller.GetControllerNumber() != controllerNumberHolding) {
			isHeld = true;
			controller.PickUpItem (gameObject);
			controllerNumberHolding = controller.GetControllerNumber();
			SetHoldingController (controller);

			if (gameObject.GetComponent<ConfigurableJoint> ()) {
				WandController oldController = gameObject.GetComponent<ConfigurableJoint> ().connectedBody.gameObject.GetComponent<WandController>() as WandController;
				oldController.SetControllerVisible (true);
				oldController.DropItem ();
				gameObject.GetComponent<ConfigurableJoint> ().connectedBody = null;
				Destroy (gameObject.GetComponent<ConfigurableJoint> ());
			}

			if (hasGripPosition) {
				gameObject.transform.parent = controller.transform;
				gameObject.transform.rotation = controller.transform.rotation * gripRotation;
				gameObject.transform.localPosition = gripPosition;
				gameObject.transform.parent = null;
			}
			//ConfigurableJoint joint = AddConfigurableJoint();
			//joint.connectedBody = controller.transform.GetComponent<Rigidbody>();
			//joint.anchor = controller.transform.localPosition;
			if (GetComponent<Rigidbody> ()) {
				Destroy (GetComponent<Rigidbody> ());
			}
			transform.parent = controller.transform;
			Highlight (false);
		} 
		else if (!isHeld) { // Picking Up
			if (!controller.GetHoldingItem()) {
				isHeld = true;
				controller.PickUpItem (gameObject);
				controllerNumberHolding = controller.GetControllerNumber();
				SetHoldingController (controller);
				GetComponent<Collider>().isTrigger = true;

				if (hasGripPosition) {
					gameObject.transform.parent = controller.transform;
					gameObject.transform.rotation = controller.transform.rotation * gripRotation;
					gameObject.transform.localPosition = gripPosition;
					gameObject.transform.parent = null;
				}

				//ConfigurableJoint joint = AddConfigurableJoint();
				//joint.connectedBody = controller.transform.GetComponent<Rigidbody>();
				//joint.anchor = controller.transform.localPosition;
				if (GetComponent<Rigidbody> ()) {
					Destroy (GetComponent<Rigidbody> ());
				}
				transform.parent = controller.transform;
				Highlight (false);
			}
		}
	}

	public virtual void Highlight(bool highlight) {
		if (highlightObject) {
			if (highlight) {
				highlightObject.GetComponent<Renderer> ().sharedMaterials = new Material[] {
					highlightObject.GetComponent<Renderer> ().material,
					highlightMaterial
				};
			} else {
				highlightObject.GetComponent<Renderer> ().sharedMaterials = new Material[] {
					highlightObject.GetComponent<Renderer> ().material,
					highlightObject.GetComponent<Renderer> ().material
				};
			}
		} else {
			if (highlight) {
				GetComponent<Renderer> ().sharedMaterials = new Material[] {
					GetComponent<Renderer> ().material,
					highlightMaterial
				};
			} else {
				GetComponent<Renderer> ().sharedMaterials = new Material[] {
					GetComponent<Renderer> ().material,
					GetComponent<Renderer> ().material
				};
			}
		}
	}

	public bool GetIsHeld() {
		return isHeld;
	}

	public void SetIsHeld(bool set) {
		isHeld = set;
	}

	public WandController GetHoldingController() {
		return holdingController;
	}

	public void SetHoldingController(WandController controller) {
		holdingController = controller;
	}

	public virtual void OnTriggerDown(WandController controller) {
		
	}

	public virtual void OnTriggerUp(WandController controller) {

	}

	public virtual void OnTriggerHeld(WandController controller) {

	}

	public virtual void OnHairTriggerDown(WandController controller) {

	}

	public virtual void OnHairTriggerUp(WandController controller) {

	}

	public virtual void OnHairTriggerHeld(WandController controller) {

	}

	public virtual void OnGripDown(WandController controller) {
		PickUp(controller);
	}

	public virtual void OnGripUp(WandController controller) {

	}

	public virtual void OnGripHeld(WandController controller) {

	}

	public virtual void OnTouchpadDown(WandController controller) {

	}

	public virtual void OnTouchpadUp(WandController controller) {

	}

	public virtual void OnTouchpadHeld(WandController controller) {

	}

	public virtual void OnMenuDown(WandController controller) {

	}

	public virtual void OnMenuUp(WandController controller) {

	}

	public virtual void OnMenuHeld(WandController controller) {

	}
}