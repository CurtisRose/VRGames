using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	public int health;

	// Use this for initialization
	void Start () {
		health = 50;
	}
	


	public void Dagmage(int damageAmount) {
		health -= damageAmount;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}
