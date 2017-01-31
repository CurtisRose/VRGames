using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item {
	float time;
	float detonationTime = 6.0f;
	bool primed = false;
	bool detonated = false;
	GameObject explosion;
	GameObject pin;
	int damage = 200;
	float radius = 15;
	public bool handleDown = false;

	// Use this for initialization
	protected override void Start () {
		highlightObject = transform.GetChild (0).gameObject;
		explosion = transform.GetChild (2).gameObject;
		pin = transform.GetChild (1).gameObject;
		base.Start ();
	}

	protected override void Update() {
		if (transform.childCount > 2) {
			if (GetIsHeld () && handleDown) {
				transform.GetChild (1).GetComponent<Collider> ().enabled = true;
			} else {
				transform.GetChild (1).GetComponent<Collider> ().enabled = false;
			}
		}
		if (transform.childCount < 3 && !primed && (!GetIsHeld() || !handleDown)) { // Boom
			//Debug.Log("Primed");
			GetComponent<AudioSource>().Play();
			primed = true;
			time = Time.realtimeSinceStartup;
		}
		if (primed) {
			if (Time.realtimeSinceStartup - time > detonationTime) {
				if (!detonated) {
					//Debug.Log ("Boom");
					detonated = true;
					explosion.transform.parent = null;
					explosion.transform.rotation = Quaternion.Euler (Vector3.zero);
					explosion.GetComponent<ParticleSystem> ().Play ();
					explosion.GetComponent<AudioSource>().Play();
					transform.GetChild (0).GetComponent<Renderer> ().enabled = false;
					DealDamage ();
					Destroy (gameObject, 3.0f);
				}
			}
		}
	}

	private void DealDamage() {
		Collider[] colliders = Physics.OverlapSphere (explosion.transform.position, radius);
		foreach (Collider collider in colliders) {
			if (collider.GetComponent<ZombieController> ()) {
				//Debug.Log ("Damaging zombie");
				float distance = (collider.transform.position - transform.position).magnitude;
				if (distance > (radius / 2)) {
					float calcDamage = damage * ((radius - distance) / radius * 2);
					//Debug.Log ("Doing " + (int)calcDamage + " In CalcDamage at " + distance + " distance.");
					collider.GetComponent<ZombieController> ().DoDamage ((int)calcDamage);
				} else {
					//Debug.Log ("Doing " + damage + " In Damage at " + distance + " distance.");
					collider.GetComponent<ZombieController> ().DoDamage (damage);
				}
			}
		}
	}

	public override void OnGripUp(WandController controller) {
		base.OnGripDown (controller);
	}

	public override void OnTriggerDown(WandController controller) {
		handleDown = true;
	}

	public override void OnTriggerHeld(WandController controller) {
		handleDown = true;
	}

	public override void OnTriggerUp(WandController controller) {
		handleDown = false;
		if (pin.GetComponent<Item>()) {
			if (pin.GetComponent<Item> ().collidingController) {
				pin.GetComponent<Item> ().collidingController.SetCollidingObject (null);
				pin.GetComponent<Item> ().Highlight (false);
			}
		}
	}
}
