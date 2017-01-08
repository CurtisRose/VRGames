using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
	private float maxFOV = 60;
	private float minFOV = 40;
	private float zoomSpeed = 2;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 2;

	public Material onTarget;
	public Material offTarget;
	public Material automaticFireColor;
	public Material burstFireColor;
	public Material singleFireColor;

	private int fireToggle = 0; //0 single, 1 burst, 2 auto
	private float time;
	private float rateOfFire = 0.0f;
	private int burstCount = 50;

	float timeToGo;

	// Use this for initialization
	void Start () {
		time = Time.time;
	}
		

	// Update is called once per frame
	void Update () {
		// Toggling Rate of Fire
		if (Input.GetKeyDown(KeyCode.E)) {
			if (fireToggle == 0) {
				fireToggle = 1;
				transform.GetChild (4).GetComponent<Renderer>().material = burstFireColor;
			}
			else if (fireToggle == 1) {
				fireToggle = 2;
				transform.GetChild (4).GetComponent<Renderer>().material = automaticFireColor;
			}
			else if (fireToggle == 2) {
				fireToggle = 0;
				transform.GetChild (4).GetComponent<Renderer>().material = singleFireColor;
			}
		}

		// Shooting
		if (Input.GetMouseButtonDown(0)) {
			//Debug.Log("Shooting");
			if (fireToggle == 0) {
				Fire ();
			} else if (fireToggle == 1) {
				for (int i = 0; i <= burstCount; i++) {
					Fire ();
				}
			}
		} else if (Input.GetMouseButton(0) && fireToggle == 2) {
			//Debug.Log("Shooting");
			Fire();
		}

		// Aiming Down Sights
		if (Input.GetMouseButtonDown (1)) {
			//Debug.Log ("Aiming Down Sights");
			transform.Translate (-.264f, .2f, 0.1f);
		}
		else if (Input.GetMouseButtonUp (1)) {
			transform.Translate (.264f, -.2f, -0.1f);
		}
		if (Input.GetMouseButton(1)) {
			if (Camera.main.fieldOfView >= minFOV) {
				if (Camera.main.fieldOfView - zoomSpeed < minFOV) {
					Camera.main.fieldOfView = minFOV;
				} else {
					Camera.main.fieldOfView -= zoomSpeed;
				}
			}
		} else {
			if (Camera.main.fieldOfView <= maxFOV) {
				if (Camera.main.fieldOfView + zoomSpeed > maxFOV) {
					Camera.main.fieldOfView = maxFOV;
				} else {
					Camera.main.fieldOfView += zoomSpeed;
				}
			}
		}

		// Raycasting to change sight colors
		Ray ray = new Ray(transform.GetChild (0).GetComponent<Transform>().position, 
			transform.GetChild (0).GetComponent<Transform>().forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			if (hit.transform.tag == "Target") {
				transform.GetChild(0).GetComponent<Renderer>().material = onTarget;
				transform.GetChild(1).GetComponent<Renderer>().material = onTarget;
				transform.GetChild(2).GetComponent<Renderer>().material = onTarget;
			}
			else if (hit.transform.tag == "Bullet") {

			}
			else {
				transform.GetChild(0).GetComponent<Renderer>().material = offTarget;
				transform.GetChild(1).GetComponent<Renderer>().material = offTarget;
				transform.GetChild(2).GetComponent<Renderer>().material = offTarget;
			}
		} else {
			transform.GetChild(0).GetComponent<Renderer>().material = offTarget;
			transform.GetChild(1).GetComponent<Renderer>().material = offTarget;
			transform.GetChild(2).GetComponent<Renderer>().material = offTarget;
		}
	}

	void Fire() {
		if (fireToggle == 2) { // Automatic
			if (Time.realtimeSinceStartup - time > rateOfFire) {
				time = Time.realtimeSinceStartup;
				// Create the Bullet from the Bullet Prefab
				GameObject bullet = Instantiate (
					bulletPrefab,
					bulletSpawn.position,
					bulletSpawn.rotation);
				// Add velocity to the bullet
				bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * Time.deltaTime * bulletSpeed;
				// Destroy the bullet after 8 seconds
				Destroy(bullet, 8.0f);
			}
		}else if (fireToggle == 1) { // Burst Fire
			// Create the Bullet from the Bullet Prefab
			GameObject bullet = Instantiate (
				bulletPrefab,
				bulletSpawn.position,
				bulletSpawn.rotation);
			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * Time.deltaTime * bulletSpeed;
			// Destroy the bullet after 8 seconds
			Destroy(bullet, 8.0f);
		}else if (fireToggle == 0) { // Single Shot
			// Create the Bullet from the Bullet Prefab
			GameObject bullet = Instantiate (
				bulletPrefab,
				bulletSpawn.position,
				bulletSpawn.rotation);
			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * Time.deltaTime * bulletSpeed;
			// Destroy the bullet after 8 seconds
			Destroy(bullet, 8.0f);
		}
	}
}
