using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4A1Controller : Weapon {
	private bool automatic = false;

	protected override void Start () {
		highlightObject = transform.GetChild (0).gameObject;
		base.Start ();
		time = Time.realtimeSinceStartup;
		gripRotation = Quaternion.Euler (55, 0, 0);
		gripPosition = new Vector3 (0.0f, 0.00f, -0.1f);
		magazineRotation = Quaternion.Euler (-90, 0, 0);
		magazinePosition = new Vector3 (0f,0.060f, 0.134f);
		gunSounds = GetComponents<AudioSource>();
		hasMagazine = false;
		hasGripPosition = true;
		gunName = "M4A1";
		holsterPosition = new Vector3 (.5f, 1f, 0);
		primaryWeapon = true;
		damage = 50;
		headShotMultiplier = 4;
		secondaryDropAngle = 60.0f;
		rateOfFire = 0.1f;
		aimable = true;

	}

	protected override void Update () {
		base.Update ();
		if (otherController && holdingController) {
			Aim ();
		}

	}

	public override void OnTriggerHeld(WandController controller) {
		//Debug.Log ("Trigger Held");
		if (controller.GetObjectInHand() == gameObject) { // If the hand that is holding the gun presses trigger, then shoot.
			if (automatic) {
				AutomaticFire (controller);
			} 
		}
	}

	public override void OnTouchpadDown(WandController controller, Vector2 touchPosition) {
		//Debug.Log ("Touchpad Pressed");
		if (controller.GetObjectInHand() == gameObject) {
			gunSounds [0].Play ();
			if (automatic) {
				automatic = false;
			} else {
				automatic = true;
			}
		}
	}

	public override void Highlight(bool setHighlight) {
		base.Highlight (setHighlight);
		if (setHighlight) {
			transform.GetChild (1).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (1).GetComponent<Renderer> ().material,
				highlightMaterial
			};
		} else {
			transform.GetChild (1).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (1).GetComponent<Renderer> ().material,
				transform.GetChild (1).GetComponent<Renderer> ().material,
			};
		}
	}
}
