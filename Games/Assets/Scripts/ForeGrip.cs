using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeGrip : Item {
	public override void OnGripDown(GameObject controller) {
		//Debug.Log ("Testing");
		PickUp(controller);
	}

	protected override void PickUp(GameObject controller) {
		WandController controllerScipt = controller.GetComponent (typeof(WandController)) as WandController;
		if (isHeld && controllerScipt.controllerNumber == controllerNumberHolding) {
			Debug.Log ("Releasing Foregrip");
			isHeld = false;
			controllerScipt.holdingItem = false;
			controllerNumberHolding = 0;
			controllerScipt.objectInHand = null;

			controllerScipt.SetControllerVisible (controller, true);

			if (GetComponent<FixedJoint> ()) {
				GetComponent<FixedJoint> ().connectedBody = null;
				Destroy (GetComponent<FixedJoint> ());
			}
			gameObject.GetComponent<Rigidbody> ().velocity = controllerScipt.getVelocity ();
			gameObject.GetComponent<Rigidbody> ().angularVelocity = controllerScipt.getVelocity ();

		} else if (!isHeld) {
			if (!controllerScipt.holdingItem) {
				Debug.Log ("Grabbing Foregrip");
				Highlight (false);
				isHeld = true;
				controllerScipt.holdingItem = true;
				controllerNumberHolding = controllerScipt.controllerNumber;
				controllerScipt.objectInHand = gameObject;

				controllerScipt.SetControllerVisible (controller, false);

				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();

				// Position the gun properly
				gameObject.transform.position = controller.transform.position;
			}
		}
	}
}
