using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : Item {
	private Vector3 hitPoint;
	private bool laserOn = false;
	private float maxLaserDistance = 100;

	void Start () {
		//Debug.Log(transform.GetChild (0).name);
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild(1).gameObject.SetActive (false);
		oldMaterial = gameObject.GetComponent<Renderer> ().material;
	}

	private void ShowLaser(RaycastHit hit) {
		//Debug.Log ("hit distance: " + hit.distance);
		transform.GetChild(0).position = Vector3.Lerp (transform.position, hitPoint, 0.5f);
		transform.GetChild(0).LookAt (hitPoint);
		transform.GetChild (1).position = hitPoint;
		transform.GetChild(0).localScale = new Vector3 (
			transform.GetChild(0).localScale.x,
			transform.GetChild(0).localScale.y,
			hit.distance/(transform.localScale.z * transform.parent.localScale.z));
		transform.GetChild(1).gameObject.SetActive (true);
	}
	private void ShowLaserInfinite() {
		//Debug.Log (transform.forward);
		transform.GetChild(1).gameObject.SetActive (false);
		transform.GetChild(0).position = Vector3.Lerp (transform.position, transform.position + (-1 * transform.forward * maxLaserDistance), 0.5f);
		transform.GetChild (0).LookAt (transform.position);
		transform.GetChild(0).localScale = new Vector3 (
			transform.GetChild(0).localScale.x,
			transform.GetChild(0).localScale.y,
			maxLaserDistance/(transform.localScale.z * transform.parent.localScale.z));
	}

	void Update () {
		if (laserOn) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, -1 * transform.forward, out hit, maxLaserDistance)) {
				hitPoint = hit.point;
				ShowLaser (hit);
				transform.GetChild(0).gameObject.SetActive (true);
			} else {
				ShowLaserInfinite ();
				transform.GetChild(0).gameObject.SetActive (true);
			}
		}
	}

	void turnLaserOn () {
		if (!laserOn) {
			laserOn = true;
			//transform.GetChild(0).gameObject.SetActive (true);
			//transform.GetChild(1).gameObject.SetActive (true);
		} else {
			laserOn = false;
			transform.GetChild(0).gameObject.SetActive (false);
			transform.GetChild(1).gameObject.SetActive (false);
		}
	}

	public override void OnTriggerDown(WandController controller) {
		//Debug.Log ("Turning Laser On/Off");
		turnLaserOn ();
	}

	public override void OnTriggerUp(WandController controller) {
		
	}

	public override void OnTriggerHeld(WandController controller) {
		
	}

	public override void OnHairTriggerDown(WandController controller) {
		
	}

	public override void OnHairTriggerUp(WandController controller) {
		
	}

	public override void OnHairTriggerHeld(WandController controller) {
		
	}

	public override void OnGripDown(WandController controller) {
		
	}

	public override void OnGripUp(WandController controller) {
		
	}

	public override void OnGripHeld(WandController controller) {
		
	}

	public override void OnTouchpadDown(WandController controller) {
		
	}

	public override void OnTouchpadUp(WandController controller) {
		
	}

	public override void OnTouchpadHeld(WandController controller) {
		
	}

	public override void OnMenuDown(WandController controller) {
		
	}

	public override void OnMenuUp(WandController controller) {
		
	}

	public override void OnMenuHeld(WandController controller) {
		
	}
}
