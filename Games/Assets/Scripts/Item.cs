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

	public Item() {
		//Debug.Log ("Item Created");
	}

	public virtual void OnTriggerDown(GameObject controller) {
		Debug.Log ("Trigger Pressed");
	}

	public virtual void OnTriggerUp(GameObject controller) {
		Debug.Log ("Trigger Released");
	}

	public virtual void OnTriggerHeld(GameObject controller) {
		Debug.Log ("Trigger Held");
	}

	public virtual void OnHairTriggerDown(GameObject controller) {
		Debug.Log ("Hair Trigger Pressed");
	}

	public virtual void OnHairTriggerUp(GameObject controller) {
		Debug.Log ("Hair Trigger Released");
	}

	public virtual void OnHairTriggerHeld(GameObject controller) {
		Debug.Log ("Hair Trigger Held");
	}

	public virtual void OnGripDown(GameObject controller) {
		Debug.Log ("Grip Pressed");
	}

	public virtual void OnGripUp(GameObject controller) {
		Debug.Log ("Grip Released");
	}

	public virtual void OnGripHeld(GameObject controller) {
		Debug.Log ("Grip Held");
	}

	public virtual void OnTouchpadDown(GameObject controller) {
		Debug.Log ("Touchpad Pressed");
	}

	public virtual void OnTouchpadUp(GameObject controller) {
		Debug.Log ("Touchpad Released");
	}

	public virtual void OnTouchpadHeld(GameObject controller) {
		Debug.Log ("Touchpad Held");
	}

	public virtual void OnMenuDown(GameObject controller) {
		Debug.Log ("Menu Pressed");
	}

	public virtual void OnMenuUp(GameObject controller) {
		Debug.Log ("Menu Released");
	}

	public virtual void OnMenuHeld(GameObject controller) {
		Debug.Log ("Menu Held");
	}
}
