using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
	public int numBullets;
	protected Quaternion magazineRotation;
	protected Vector3 magazinePosition;
	public bool hasMagazine = false;
	public string gunName;
	private bool touchingHolster = false;
	private bool isHolstered = false;
	public Vector3 holsterPosition = Vector3.zero;
	public Quaternion holsterRotation = Quaternion.Euler(Vector3.zero);
	public Holster holster;
	protected WandController otherController;
	protected bool primaryWeapon = false;
	protected int damage = 50;
	protected int headShotMultiplier = 4;
	public GameObject dirtSpray;
	public Transform shootPoint;
	public GameObject shotFired;
	public AudioSource[] gunSounds;
	public Vector3 recoilPosition = Vector3.zero;
	protected float secondaryDropAngle = 0.0f;
	protected float time;
	protected float rateOfFire = 0.0f;
	protected Magazine magazine;
	public bool aimable;


	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected virtual void Fire(WandController controller) {
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
					spray.transform.LookAt (hit.normal);
				}
			}
			if (GetComponentInChildren<ParticleSystem>()) {
				foreach (ParticleSystem system in GetComponentsInChildren<ParticleSystem>()) {
					system.Emit(10);
					//if (GetComponentInChildren<WFX_LightFlicker>().time 
				}
			}
			GetComponent<Recoil> ().StartRecoil (0.1f, 5f, 1.5f);
		} else {
			gunSounds [0].Play ();
		}
	}

	protected virtual void AutomaticFire(WandController controller) {
		if (Time.realtimeSinceStartup - time > rateOfFire) {
			time = Time.realtimeSinceStartup;
			Fire (controller);
		}
	}

	public virtual void Reload (Magazine magazineScript) {
		//Debug.Log ("Loading Magazine");
		magazine = magazineScript;
		hasMagazine = true;
		gunSounds[1].Play();
		numBullets += magazineScript.numBullets;
		WandController oldController = magazineScript.GetHoldingController();
		oldController.SetControllerVisible (true);
		oldController.DropItem ();

		if (magazineScript.gameObject.GetComponent<Rigidbody> ()) {
			Destroy (magazineScript.gameObject.GetComponent<Rigidbody> ());
		}
		Physics.IgnoreCollision(GetComponent<Collider>(), magazineScript.GetComponent<Collider>());
		magazineScript.transform.parent = transform;
		magazineScript.transform.localRotation = magazineRotation;
		magazineScript.transform.localPosition = magazinePosition;
	}

	public virtual void Unload(Magazine magazineScript) {
		//Debug.Log ("Unloading Magazine");
		hasMagazine = false;
		magazine = null;
		magazineScript.attached = false;
		magazineScript.attachedWeapon = null;
		gunSounds[1].Play();
		Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), magazineScript.GetComponent<Collider>(), false);
		if (numBullets > 0) {
			magazineScript.numBullets = numBullets - 1;
			numBullets = 1;
		} else {
			magazineScript.numBullets = 0;
			numBullets = 0;
		}
		Highlight (false);
	}

	protected virtual void Aim() {
		//Debug.Log ("Aiming with other controller");
		Vector3 forward = ((otherController.transform.position) - holdingController.transform.position).normalized;

		// calculate rightLocked rotation
		Quaternion rightLocked = Quaternion.LookRotation(forward, Vector3.Cross(-holdingController.transform.right, forward).normalized);

		// delta from current rotation to the rightLocked rotation
		Quaternion rightLockedDelta = Quaternion.Inverse(gameObject.transform.rotation) * rightLocked;

		float rightLockedAngle;
		Vector3 rightLockedAxis;

		// forward direction and roll
		rightLockedDelta.ToAngleAxis(out rightLockedAngle, out rightLockedAxis);

		if (rightLockedAngle > secondaryDropAngle) {
			if (rightLockedAngle < 350) {
				//Debug.Log ("Letting go with secondary controller");
				otherController.secondaryHoldObject = null;
				UnsetOtherController ();
				gameObject.transform.rotation = holdingController.transform.rotation * gripRotation;
				return;
			}
		}

		if (rightLockedAngle > 180f)
		{
			// remap ranges from 0-360 to -180 to 180
			rightLockedAngle -= 360f;
		}

		// make any negative values into positive values;
		rightLockedAngle = Mathf.Abs(rightLockedAngle);

		// calculate upLocked rotation
		Quaternion upLocked = Quaternion.LookRotation(forward, holdingController.transform.forward);

		// delta from current rotation to the upLocked rotation
		Quaternion upLockedDelta = Quaternion.Inverse(gameObject.transform.rotation) * upLocked;

		float upLockedAngle;
		Vector3 upLockedAxis;

		// forward direction and roll
		upLockedDelta.ToAngleAxis(out upLockedAngle, out upLockedAxis);

		// remap ranges from 0-360 to -180 to 180
		if (upLockedAngle > 180f)
		{
			upLockedAngle -= 360f;
		}

		// make any negative values into positive values;
		upLockedAngle = Mathf.Abs(upLockedAngle);

		// assign the one that involves less change to roll
		gameObject.transform.rotation = (upLockedAngle < rightLockedAngle ? upLocked : rightLocked);
		//gameObject.transform.rotation = Quaternion.Slerp(transform.rotation,(upLockedAngle < rightLockedAngle ? upLocked : rightLocked), 5);
	}

	protected override void PickUp(WandController controller) {
		base.PickUp (controller);
		GetComponent<Collider> ().isTrigger = false;
		if (controller.GetHoldingItem ()) {
			//Debug.Log ("Setting controller invisible");
			//controller.SetControllerVisible (false);
		} else {
			//Debug.Log ("Setting controller visible");
			controller.SetControllerVisible (true);
		}
	}

	public void SetOtherController(WandController controller) {
		if (holdingController) {
			otherController = controller;
			controller.secondaryHoldObject = gameObject;
			//otherController.SetControllerVisible (false);
		}
	}

	public void UnsetOtherController () {
		if (otherController) {
			if (otherController.secondaryHoldObject) {
				otherController.secondaryHoldObject = null;
			}
			otherController.SetCollidingObject (null);
			otherController.SetControllerVisible (true);
			otherController = null;
		}
	}

	public WandController GetOtherController () {
		return otherController;
	}

	public override void OnTriggerDown(WandController controller) {
		//Debug.Log ("Trigger Pressed");
		if (controller.GetObjectInHand () == gameObject) {
			Fire (controller);
		} else {
			if (aimable) {
				if (!otherController) {
					//Debug.Log ("Aim Weapon");
					SetOtherController (controller);
					Highlight (false);
				} else if (otherController) {
					//Debug.Log ("Unaim Weapon");
					gameObject.transform.rotation = holdingController.transform.rotation * gripRotation;
					UnsetOtherController ();
				}
			}
		}
	}

	public override void OnGripDown(WandController controller) {
		//Debug.Log ("Pressing grips on weapon");
		//Debug.Log (GetTouchingHolster());
		//Debug.Log (!GetIsHolstered());
		//Debug.Log (GetIsHeld ());
		if (GetTouchingHolster() && !GetIsHolstered () && GetIsHeld ()) {
			//Debug.Log ("Holstering Weapon");
			bool isLeft = controller.isLeftcontroller;
			PickUp (controller);
			if (!holster.HolsterWeapon (this, isLeft)) {
				PickUp (controller);
			}

		} else if (GetIsHolstered ()) {
			//Debug.Log ("Unholstering weapon");
			SetTouchingHolster (false);
			holster.UnholsterWeapon (this);
			PickUp (controller);
		} 	else if (!holdingController) {
			//Debug.Log ("Pick up Weapon");
			PickUp (controller);
		} else if (holdingController) {
			if (!controller.secondaryHoldObject) {
				//Debug.Log ("Drop Weapon");
				PickUp (controller);
				UnsetOtherController ();
			}
		}
	}

	public override void OnTouchpadDown(WandController controller, Vector2 touchPosition) {
		//Debug.Log ("Touchpad Pressed " + touchPosition);
	}

	public Quaternion GetGripRotation() {
		return gripRotation;
	}

	public bool GetTouchingHolster() {
		return touchingHolster;
	}

	public void SetTouchingHolster(bool setTouching) {
		touchingHolster = setTouching;
	}

	public bool GetIsHolstered() {
		return isHolstered;
	}

	public void SetIsHolstered(bool setHolstered) {
		isHolstered = setHolstered;
	}

	public bool GetIsPrimaryWeapon() {
		return primaryWeapon;
	}

	public void SetIsPrimaryWeapon(bool setPrimary) {
		primaryWeapon = setPrimary;
	}
}
