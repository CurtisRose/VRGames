using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : Weapon {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 1000;

	public Material onTarget;
	public Material offTarget;

	float time;

	float rateOfFire = 0.2f;

	bool twoHanded = false;

	// Use this for initialization
	protected override void Start () {
		highlightObject = transform.GetChild (1).gameObject;
		base.Start ();
		time = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		// Raycasting to change sight colors
		Ray ray = new Ray(transform.GetChild (3).GetComponent<Transform>().position, 
			transform.GetChild (3).GetComponent<Transform>().forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			if (hit.transform.tag == "Target") {
				transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = onTarget;
				transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = onTarget;
				transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material = onTarget;
			}
			else if (hit.transform.tag == "Bullet") {

			}
			else {
				transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = offTarget;
				transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = offTarget;
				transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material = offTarget;
			}
		} else {
			transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = offTarget;
			transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = offTarget;
			transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material = offTarget;
		}
		if (twoHanded) {
			//gameObject.transform.rot
		}
	}

	void Fire() {
		// Create the Bullet from the Bullet Prefab
		//Debug.Log("Firing");
		GameObject bullet = Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
		Physics.IgnoreCollision (bullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody> ().AddForce(bullet.transform.forward * bulletSpeed);
		// Destroy the bullet after 8 seconds
		Destroy(bullet, 8.0f);
	}

	void AutomaticFire() {
		if (Time.realtimeSinceStartup - time > rateOfFire) {
			time = Time.realtimeSinceStartup;
			Fire ();
		}
	}

	public override void OnTriggerHeld(WandController controller) {
		//Debug.Log ("Trigger Held While Holding Gun");
		if (isHeld) {
			AutomaticFire ();
		}
	}
}
