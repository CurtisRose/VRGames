using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : Attachment {
	private Vector3 hitPoint;
	private bool laserOn = false;
	private float maxLaserDistance = 100;

	protected override void Start () {
		base.Start ();
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild(1).gameObject.SetActive (false);
		attachmentType = "Optics";
	}

	private void ShowLaser(RaycastHit hit) {
		transform.GetChild(0).position = Vector3.Lerp (transform.position, hitPoint, 0.5f);
		transform.GetChild(0).LookAt (hitPoint);
		transform.GetChild (1).position = hitPoint;

		transform.GetChild (0).localScale = new Vector3 (
			transform.GetChild (0).localScale.x,
			transform.GetChild (0).localScale.y,
			hit.distance / transform.localScale.z);

		transform.GetChild(1).gameObject.SetActive (true);
	}

	private void ShowLaserInfinite() {
		transform.GetChild(1).gameObject.SetActive (false);
		transform.GetChild(0).position = Vector3.Lerp (transform.position, transform.position + (-1 * transform.forward * maxLaserDistance), 0.5f);
		transform.GetChild (0).LookAt (transform.position);
		
		transform.GetChild (0).localScale = new Vector3 (
			transform.GetChild (0).localScale.x,
			transform.GetChild (0).localScale.y,
			maxLaserDistance / transform.localScale.z);
	}

	void Update () {
		if (laserOn) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, -1 * transform.forward, out hit, maxLaserDistance)) {
				hitPoint = hit.point;
				ShowLaser (hit);
			} else {
				ShowLaserInfinite ();
			}
		}
	}

	void turnLaserOn () {
		if (!laserOn) {
			laserOn = true;
			ShowLaserInfinite ();
			transform.GetChild(0).gameObject.SetActive (true);
		} else {
			laserOn = false;
			transform.GetChild(0).gameObject.SetActive (false);
			transform.GetChild(1).gameObject.SetActive (false);
		}
	}

	public override void OnTriggerDown(WandController controller) {
		turnLaserOn ();
	}
}