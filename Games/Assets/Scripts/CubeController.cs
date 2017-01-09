using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Item {

	// Use this for initialization
	void Start () {
		
	}
	
	public override void OnTriggerDown(GameObject controller) {
		Debug.Log ("Trigger Pressed While Holding Cube");
	}

	public override void OnTriggerUp(GameObject controller) {
		Debug.Log ("Trigger Released While Holding Cube");
	}

	public override void OnTriggerHeld(GameObject controller) {
		Debug.Log ("Trigger Held While Holding Cube");
	}

	public override void OnHairTriggerDown(GameObject controller) {
		Debug.Log ("Hair Trigger Pressed While Holding Cube");
	}

	public override void OnHairTriggerUp(GameObject controller) {
		Debug.Log ("Hair Trigger Released While Holding Cube");
	}

	public override void OnHairTriggerHeld(GameObject controller) {
		Debug.Log ("Hair Trigger Held While Holding Cube");
	}

	public override void OnGripDown(GameObject controller) {
		Debug.Log ("Grip Pressed While Holding Cube");
	}

	public override void OnGripUp(GameObject controller) {
		Debug.Log ("Grip Released While Holding Cube");
	}

	public override void OnGripHeld(GameObject controller) {
		Debug.Log ("Grip Held While Holding Cube");
	}

	public override void OnTouchpadDown(GameObject controller) {
		Debug.Log ("Touchpad Pressed While Holding Cube");
	}

	public override void OnTouchpadUp(GameObject controller) {
		Debug.Log ("Touchpad Released While Holding Cube");
	}

	public override void OnTouchpadHeld(GameObject controller) {
		Debug.Log ("Touchpad Held While Holding Cube");
	}

	public override void OnMenuDown(GameObject controller) {
		Debug.Log ("Menu Pressed While Holding Cube");
	}

	public override void OnMenuUp(GameObject controller) {
		Debug.Log ("Menu Released While Holding Cube");
	}

	public override void OnMenuHeld(GameObject controller) {
		Debug.Log ("Menu Held While Holding Cube");
	}
}
