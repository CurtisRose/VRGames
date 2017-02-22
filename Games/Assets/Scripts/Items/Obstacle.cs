using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Item {
	public int health;
	private int maxHealth = 50;

	// Use this for initialization
	void Start () {
		health = 50;
	}
	


	public void Damage(int damageAmount) {
		health -= damageAmount;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

	public void Repair(int repairAmount) {
		Debug.Log ("Repairing Obstacle");
		if (health < maxHealth) {
			health += repairAmount;
		}
		if (health > maxHealth) {
			Debug.Log ("Obstacle at full health");
			health = maxHealth;
		}
	}

	public override void OnTriggerHeld(WandController controller) {
		Repair (1);
	}
}
