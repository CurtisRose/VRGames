using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour {
	float time;
	public float spawnRate;
	public GameObject zombiePrefab;
	public Transform target;
	public int numZombies;
	private int zombieHealth;

	// Use this for initialization
	void Start () {
		//Debug.Log ("SpawnPoint created");
		time = Time.realtimeSinceStartup;
	}

	public void Spawn() {
		//Debug.Log ("Spawning Zombie");
		numZombies--;
		GameObject zombie = Instantiate (
			zombiePrefab,
			transform.position,
			transform.rotation);
		zombie.GetComponent<ZombieController> ().target = target;
		zombie.GetComponent<ZombieController> ().player = target.gameObject;
		zombie.GetComponent<ZombieController> ().SetHealth (zombieHealth);
	}

	public void PrepareSpawning(int num, int spawnrate, int setZombieHealth) {
		//Debug.Log ("Preparing Spawnpoints");
		spawnRate = spawnrate;
		numZombies = num;
		time = Time.realtimeSinceStartup;
		zombieHealth = setZombieHealth;
	}

	// Update is called once per frame
	void Update () {
		if (numZombies > 0) {
			if (Time.realtimeSinceStartup - time > spawnRate) {
				time = Time.realtimeSinceStartup;
				Spawn ();
			}
		}
	}
}
