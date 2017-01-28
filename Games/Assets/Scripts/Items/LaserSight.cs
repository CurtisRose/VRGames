using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : Attachment {
	private Vector3 hitPoint;
	private bool laserOn = false;
	private float maxLaserDistance = 100;

	protected override void Start () {
		highlightObject = transform.GetChild(0).gameObject;
		base.Start ();
		highlightObject.transform.GetChild (0).gameObject.SetActive (false);
		highlightObject.transform.GetChild(1).gameObject.SetActive (false);
		attachmentPosition = new Vector3 (0.0f, -38.0f ,-0.2f);
		attachmentType = "Other";
	}

	private void ShowLaser(RaycastHit hit) {
		highlightObject.transform.GetChild(0).position = Vector3.Lerp (highlightObject.transform.position, hitPoint, 0.5f);
		highlightObject.transform.GetChild(0).LookAt (hitPoint);
		highlightObject.transform.GetChild (1).position = hitPoint;

		highlightObject.transform.GetChild (0).localScale = new Vector3 (
			highlightObject.transform.GetChild (0).localScale.x,
			highlightObject.transform.GetChild (0).localScale.y,
			hit.distance / highlightObject.transform.localScale.z);

		highlightObject.transform.GetChild(1).gameObject.SetActive (true);
	}

	private void ShowLaserInfinite() {
		highlightObject.transform.GetChild(1).gameObject.SetActive (false);
		highlightObject.transform.GetChild(0).position = Vector3.Lerp (highlightObject.transform.position, highlightObject.transform.position + (-1 * highlightObject.transform.forward * maxLaserDistance), 0.5f);
		highlightObject.transform.GetChild (0).LookAt (highlightObject.transform.position);
		
		highlightObject.transform.GetChild (0).localScale = new Vector3 (
			highlightObject.transform.GetChild (0).localScale.x,
			highlightObject.transform.GetChild (0).localScale.y,
			maxLaserDistance / highlightObject.transform.localScale.z);
	}

	protected override void Update () {
		base.Update ();
		if (laserOn) {
			RaycastHit hit;
			if (Physics.Raycast (highlightObject.transform.position, -1 * highlightObject.transform.forward, out hit, maxLaserDistance)) {
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
			highlightObject.transform.GetChild(0).gameObject.SetActive (true);
		} else {
			laserOn = false;
			highlightObject.transform.GetChild(0).gameObject.SetActive (false);
			highlightObject.transform.GetChild(1).gameObject.SetActive (false);
		}
	}

	public override void OnTriggerDown(WandController controller) {
		turnLaserOn ();
	}
}