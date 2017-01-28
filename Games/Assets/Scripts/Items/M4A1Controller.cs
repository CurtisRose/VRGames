using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4A1Controller : Weapon {
	public float bulletSpeed = 15000;
	float time;
	float rateOfFire = 0.15f;
	private bool automatic = false;
	public AudioSource[] gunSounds;
	int damage = 50;
	int headShotMultiplier = 4;
	public GameObject dirtSpray;
	public Transform shootPoint;
	public GameObject shotFired;


	protected override void Start () {
		highlightObject = transform.GetChild (0).GetChild (0).gameObject;
		base.Start ();
		time = Time.realtimeSinceStartup;
		gripRotation = Quaternion.Euler (55, 0, 0);
		gripPosition = new Vector3 (0.0f, 0.00f, -0.1f);
		magazineRotation = Quaternion.Euler (-90, 0, 0);
		magazinePosition = new Vector3 (0f,0.0307f, 0.1366f);
		gunSounds = GetComponents<AudioSource>();
		hasMagazine = false;
		hasGripPosition = true;
		gunName = "M4A1";
	}

	void Fire(WandController controller) {
		if (numBullets > 0) {
			controller.VibrateController (3999);
			gunSounds [numBullets % 2 + 2].Play ();
			numBullets -= 1;
			GameObject bullet = Instantiate (
						shotFired,
						shootPoint.position,
						shootPoint.rotation);
			RaycastHit hit;
			if (Physics.Raycast (shootPoint.position, shootPoint.forward, out hit)) {
				if (hit.collider.gameObject.CompareTag ("Enemy")) {
					//Debug.Log ("Bodyshot");
					ZombieController zombie = hit.collider.gameObject.GetComponent<ZombieController> ();
					zombie.DoDamage (damage);
					//Debug.Log ("Hit Zombie");
				} else if (hit.collider.gameObject.CompareTag ("Head")) {
					//Debug.Log ("Headshot");
					hit.collider.gameObject.SetActive (false);
					ZombieController zombie = hit.collider.gameObject.GetComponentInParent<ZombieController> ();
					zombie.DoDamage (damage * headShotMultiplier);
				} else if (hit.collider.gameObject.CompareTag ("Limb")) {
					//Debug.Log ("Limbshot");
					hit.collider.gameObject.SetActive (false);
					ZombieController zombie = hit.collider.gameObject.GetComponentInParent<ZombieController> ();
					zombie.DoDamage (damage);
				} else if (hit.collider.gameObject.CompareTag ("Target")) {
					GameObject spray = Instantiate (
						dirtSpray,
						hit.point,
						Quaternion.Euler(0,0,0));
					spray.GetComponent<ParticleSystem> ().Play ();
					hit.collider.gameObject.GetComponentInParent<Target> ().SwitchColors ();
				} else if (hit.collider) {
					//Debug.Log ("Spray dirt");
					GameObject spray = Instantiate (
						dirtSpray,
						hit.point,
						Quaternion.Euler(0,0,0));
					spray.GetComponent<ParticleSystem> ().Play ();
				}
			}
			if (GetComponentInChildren<ParticleSystem>()) {
				foreach (ParticleSystem system in GetComponentsInChildren<ParticleSystem>()) {
					system.Emit(10);
					//if (GetComponentInChildren<WFX_LightFlicker>().time 
				}
			}

		} else {
			gunSounds [0].Play ();
		}
	}

	void AutomaticFire(WandController controller) {
		if (Time.realtimeSinceStartup - time > rateOfFire) {
			time = Time.realtimeSinceStartup;
			Fire (controller);
		}
	}

	public override void Reload (Magazine magazineScript) {
		hasMagazine = true;
		gunSounds[1].Play();
		numBullets += magazineScript.numBullets;
		magazineScript.SetIsHeld(false);
		if (magazineScript.gameObject.GetComponent<ConfigurableJoint> ()) {
			WandController oldController = magazineScript.GetHoldingController();
			oldController.SetControllerVisible (true);
			oldController.DropItem ();
			magazineScript.gameObject.GetComponent<ConfigurableJoint> ().connectedBody = null;
			Destroy (magazineScript.gameObject.GetComponent<ConfigurableJoint> ());
		}
		Destroy (magazineScript.gameObject.GetComponent<Rigidbody> ());
		Physics.IgnoreCollision(GetComponent<Collider>(), magazineScript.GetComponent<Collider>());
		magazineScript.transform.parent = transform;
		magazineScript.transform.localRotation = magazineRotation;
		magazineScript.transform.localPosition = magazinePosition;
	}

	public override void Unload(Magazine magazineScript) {
		//Debug.Log ("Unloading Magazine");
		hasMagazine = false;
		gunSounds[1].Play();
		Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), magazineScript.GetComponent<Collider>(), false);
		if (numBullets > 0) {
			magazineScript.numBullets = numBullets - 1;
			numBullets = 1;
		} else {
			magazineScript.numBullets = 0;
			numBullets = 0;
		}
	}

	protected override void PickUp(WandController controller) {
		base.PickUp (controller);
		if (GetIsHeld ()) {
			controller.SetControllerVisible (false);
		} else {
			controller.SetControllerVisible (true);
		}
	}

	public override void OnTriggerDown(WandController controller) {
		//Debug.Log ("Trigger Pressed");
		if (!automatic && controller.GetObjectInHand() == gameObject) {
			Fire (controller);
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

	public override void OnTriggerUp(WandController controller) {

	}

	public override void OnGripDown(WandController controller) {
		//Debug.Log ("Grip Pressed");
		PickUp(controller);
	}

	public override void OnTouchpadDown(WandController controller) {
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
			transform.GetChild (0).GetChild (1).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (0).GetChild (1).GetComponent<Renderer> ().material,
				highlightMaterial
			};
		} else {
			transform.GetChild (0).GetChild (1).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (0).GetChild (1).GetComponent<Renderer> ().material,
				transform.GetChild (0).GetChild (1).GetComponent<Renderer> ().material,
			};
		}
	}
}
