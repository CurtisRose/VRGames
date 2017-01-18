using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1A1Controller : Weapon {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 9000;
	float time;
	float rateOfFire = 0.15f;
	private bool automatic = false;
	public AudioSource[] gunShot;


	void Start () {
		time = Time.realtimeSinceStartup;
		oldMaterial = gameObject.transform.GetChild(1).GetComponent<Renderer> ().material;
		gripRotation = Quaternion.Euler (-65, 180, 0);
		gripPosition = new Vector3 (0.0f, -0.17f, 0.01f);
		magazineRotation = Quaternion.Euler (-90, 0, 0);
		magazinePosition = new Vector3 (0f, -0.04166641f, 0.1096273f);
		gunShot = GetComponents<AudioSource>();

	}

	void OnCollisionEnter(Collision col) {
		
	}

	void Fire() {
		if (numBullets > 0) {
			gunShot [numBullets%2].Play ();
			numBullets -= 1;
			GameObject bullet = Instantiate (
				                    bulletPrefab,
				                    bulletSpawn.position,
				                    bulletSpawn.rotation);
			Physics.IgnoreCollision (bullet.GetComponent<Collider> (), gameObject.GetComponent<Collider> ());
			bullet.GetComponent<Rigidbody> ().AddForce (bullet.transform.forward * bulletSpeed);
			Destroy (bullet, 8.0f);
		}
	}

	void AutomaticFire() {
		if (Time.realtimeSinceStartup - time > rateOfFire) {
			time = Time.realtimeSinceStartup;
			Fire ();
		}
	}

	public override void Reload (Magazine magazineScript) {
		//Debug.Log ("Loading Magazine");
		gunShot[2].Play();
		numBullets += magazineScript.numBullets;
		magazineScript.isHeld = false;
		if (magazineScript.gameObject.GetComponent<FixedJoint> ()) {
			WandController oldController = magazineScript.holdingController;
			oldController.SetControllerVisible (true);
			oldController.holdingItem = false;
			oldController.objectInHand = null;
			magazineScript.gameObject.GetComponent<FixedJoint> ().connectedBody = null;
			Destroy (magazineScript.gameObject.GetComponent<FixedJoint> ());
		}
		Destroy (magazineScript.gameObject.GetComponent<Rigidbody> ());
		Physics.IgnoreCollision(GetComponent<Collider>(), magazineScript.GetComponent<Collider>());
		magazineScript.transform.parent = transform;
		magazineScript.transform.localRotation = magazineRotation;
		magazineScript.transform.localPosition = magazinePosition;
	}

	public override void Unload(Magazine magazineScript) {
		//Debug.Log ("Unloading Magazine");
		gunShot[2].Play();
		Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), magazineScript.GetComponent<Collider>(), false);
		if (numBullets > 0) {
			magazineScript.numBullets = numBullets - 1;
			numBullets = 1;
		} else {
			magazineScript.numBullets = 0;
			numBullets = 0;
		}
	}

	public override void Highlight(bool highlight) {
		if (highlight) {
			gameObject.GetComponent<Renderer> ().material = highlightMaterial;
		} else {
			gameObject.GetComponent<Renderer> ().material = oldMaterial;
		}
	}

	protected override void PickUp(WandController controller) {
		if (isHeld) {
			if (controller.controllerNumber == controllerNumberHolding) {
				//Debug.Log ("Dropping Gun");
				isHeld = false;
				controller.holdingItem = false;
				controller.objectInHand = null;
				controllerNumberHolding = 0;
				controller.SetControllerVisible (true);
				if (GetComponent<FixedJoint> ()) {
					GetComponent<FixedJoint> ().connectedBody = null;
					Destroy (GetComponent<FixedJoint> ());
				}
				gameObject.GetComponent<Rigidbody> ().velocity = controller.getVelocity ();
				gameObject.GetComponent<Rigidbody> ().angularVelocity = controller.getVelocity ();
			} else {
				//Debug.Log ("Gun Swapping Hands");
				controller.holdingItem = true;
				controllerNumberHolding = controller.controllerNumber;
				controller.objectInHand = gameObject;

				if (gameObject.GetComponent<FixedJoint> ()) {
					WandController oldController = gameObject.GetComponent<FixedJoint> ().connectedBody.gameObject.GetComponent<WandController>() as WandController;
					oldController.SetControllerVisible (true);
					oldController.holdingItem = false;
					oldController.objectInHand = null;
					gameObject.GetComponent<FixedJoint> ().connectedBody = null;
					Destroy (gameObject.GetComponent<FixedJoint> ());
				}

				controller.SetControllerVisible (false);
				gameObject.transform.parent = controller.transform;
				gameObject.transform.rotation = controller.transform.rotation * gripRotation;
				gameObject.transform.localPosition = gripPosition;
				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				gameObject.transform.parent = null;
				Highlight (false);
			}
			//Highlight ();
		} else if (!isHeld) {
			if (!controller.holdingItem) {
				isHeld = true;
				controller.holdingItem = true;
				controllerNumberHolding = controller.controllerNumber;
				controller.objectInHand = gameObject;
				controller.SetControllerVisible (false);
				gameObject.transform.parent = controller.transform;
				gameObject.transform.rotation = controller.transform.rotation * gripRotation;
				gameObject.transform.localPosition = gripPosition;
				FixedJoint joint = AddFixedJoint();
				joint.connectedBody = controller.GetComponent<Rigidbody> ();
				gameObject.transform.parent = null;
				Highlight (false);
			}
		}
	}

	public override void OnTriggerDown(WandController controller) {
		//Debug.Log ("Trigger Pressed");
		if (!automatic) {
			Fire ();
		}
	}

	public override void OnTriggerUp(WandController controller) {
		//Debug.Log ("Trigger Released");
	}

	public override void OnTriggerHeld(WandController controller) {
		//Debug.Log ("Trigger Held");
		if (controller.objectInHand == gameObject) { // If the hand that is holding the gun presses trigger, then shoot.
			if (automatic) {
				AutomaticFire ();
			} 
		}
	}

	public override void OnHairTriggerDown(WandController controller) {
		//Debug.Log ("Hair Trigger Pressed");
	}

	public override void OnHairTriggerUp(WandController controller) {
		//Debug.Log ("Hair Trigger Released");
	}

	public override void OnHairTriggerHeld(WandController controller) {
		//Debug.Log ("Hair Trigger Held");
	}

	public override void OnGripDown(WandController controller) {
		//Debug.Log ("Grip Pressed");
		PickUp(controller);
	}

	public override void OnGripUp(WandController controller) {
		//Debug.Log ("Grip Released");
	}

	public override void OnGripHeld(WandController controller) {
		//Debug.Log ("Grip Held");
	}

	public override void OnTouchpadDown(WandController controller) {
		//Debug.Log ("Touchpad Pressed");
		if (automatic) {
			automatic = false;
		} else {
			automatic = true;
		}
	}

	public override void OnTouchpadUp(WandController controller) {
		//Debug.Log ("Touchpad Released");
	}

	public override void OnTouchpadHeld(WandController controller) {
		//Debug.Log ("Touchpad Held");
	}

	public override void OnMenuDown(WandController controller) {
		//Debug.Log ("Menu Pressed");
	}

	public override void OnMenuUp(WandController controller) {
		//Debug.Log ("Menu Released");
	}

	public override void OnMenuHeld(WandController controller) {
		//Debug.Log ("Menu Held");
	}
}
