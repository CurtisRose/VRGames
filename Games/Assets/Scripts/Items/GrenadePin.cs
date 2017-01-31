using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePin : Item {
	public override void OnGripDown(WandController controller) {
		base.OnGripDown (controller);
		//Debug.Log ("Testing");
		if (!GetIsHeld ()) {
			Destroy (gameObject, 5.0f);
			Destroy(GetComponent<Item>());
			//Debug.Log ("Dropped pin");
		}
	}

	public override void OnTriggerDown(WandController controller) {
		base.OnGripDown (controller);
		//Debug.Log ("Testing");
		if (!GetIsHeld ()) {
			Destroy (gameObject, 5.0f);
			Destroy(GetComponent<Item>());
			//Debug.Log ("Dropped pin");
		}
	}
}
