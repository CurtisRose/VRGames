/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 1000;

	public Material onTarget;
	public Material offTarget;

	public Material singleFireColor;




	// Use this for initialization
	void Start () {

	}


	// Update is called once per frame
	void Update () {
		// Shooting
		if (gameObject.transform.parent != null && 
			SteamVR_Controller.Input ((int)gameObject.transform.parent.gameObject.GetComponent<SteamVR_TrackedObject> ().index).GetHairTriggerDown ()) {
			//Debug.Log("Attempting To Shoot");
			Fire();
		}

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
	}

	void Fire() {
		// Create the Bullet from the Bullet Prefab
		//Debug.Log("Firing");
		GameObject bullet = Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody> ().AddForce(bullet.transform.forward * bulletSpeed);
		// Destroy the bullet after 8 seconds
		Destroy(bullet, 8.0f);
	}
}
*/