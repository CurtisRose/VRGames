using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowPart : Item {
	public override void OnTriggerDown(WandController controller) {
		Debug.Log ("Attempting to repair the window");
		GetComponentInParent<Obstacle> ().Repair (1);
	}
}
