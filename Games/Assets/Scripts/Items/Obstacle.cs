using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Item {
	public int health;
	private int maxHealth;
	public bool destroyed;
	public GameObject hitSpray;
	public Transform hitSprayTransform;

	// Use this for initialization
	protected override void Start () {
		maxHealth = health;
		destroyed = false;
	}

	private void ChangeMeshCollider() {
		GetComponent<MeshCollider> ().sharedMesh = highlightObject.GetComponent<MeshCollider> ().sharedMesh;
	}

	public void Damage(int damageAmount) {
		int currentWindowNumber = maxHealth - health;
		int nextWindowNumber = maxHealth - health + damageAmount;
		GameObject spray = Instantiate (
			hitSpray,
			hitSprayTransform.position,
			Quaternion.Euler(0,0,0));
		spray.transform.localScale *= 10;
		spray.GetComponent<ParticleSystem> ().Play ();
		if (nextWindowNumber < maxHealth) {
			highlightObject = transform.GetChild (nextWindowNumber).gameObject;
			transform.GetChild (nextWindowNumber).gameObject.SetActive (true);
		} else {
			destroyed = true;
		}
			
		transform.GetChild (currentWindowNumber).gameObject.GetComponent<Item> ().Highlight (false);
		if (transform.GetChild (currentWindowNumber).gameObject.GetComponent<Item> ().collidingController) {
			transform.GetChild (currentWindowNumber).gameObject.GetComponent<Item> ().collidingController.SetCollidingObject (null);
		}
		transform.GetChild (currentWindowNumber).gameObject.SetActive (false);
		health -= damageAmount;
		if (health < 0) {
			health = 0;
		}
	}

	public void Repair(int repairAmount) {
		int currentWindowNumber = maxHealth - health;
		int nextWindowNumber = maxHealth - health - repairAmount;
		if (health != maxHealth) {
			GameObject spray = Instantiate (
				hitSpray,
				hitSprayTransform.position,
				Quaternion.Euler(0,0,0));
			spray.transform.localScale *= 10;
			spray.GetComponent<ParticleSystem> ().Play ();
			GetComponent<AudioSource> ().Play ();
		}
		health += repairAmount;
		if (nextWindowNumber <= 0) {
			health = maxHealth;
			nextWindowNumber = 0;
		}
		if (transform.GetChild (maxHealth - health).gameObject.GetComponent<Item> ().collidingController) {
			transform.GetChild (currentWindowNumber).GetComponent<Item> ().collidingController.SetCollidingObject (null);
		}
		transform.GetChild (currentWindowNumber).gameObject.GetComponent<Item> ().Highlight (false);
		transform.GetChild (currentWindowNumber).gameObject.SetActive (false);
		transform.GetChild (nextWindowNumber).gameObject.SetActive (true);
		transform.GetChild (currentWindowNumber).gameObject.GetComponent<Item> ().Highlight (false);
	}
}
