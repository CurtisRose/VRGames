using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	float playerSpeed = 0.05f;
	public int health = 100;
	private int fullHealth = 100;
	AudioSource[] playerSounds;
	PlayerController instance;

	// Use this for initialization
	void Start () {
		playerSounds = GetComponents<AudioSource> ();
		GameController.SetPlayerHealth (health);
	}

	public void MovePlayer (Vector2 direction, Vector3 controllerRotation) {
		//Debug.Log ("Moving Player");
		//MoveHorizontal (direction.x * Vector3.Project(controllerRotation, Vector3.up).y);
		//MoveVertical (direction.y * Vector3.Project(controllerRotation, Vector3.up).y);
	}

	private void MoveVertical(float value) {
		transform.position += transform.right * value * playerSpeed;
	}

	private void MoveHorizontal(float value) {
		transform.position -=  transform.forward * value * playerSpeed;
	}

	public void DoDamage(int damage) {
		//Debug.Log ("Player is being damaged");
		if (!playerSounds [0].isPlaying) {
			playerSounds [0].Play ();
		}
		if (health <= fullHealth) {
			Debug.Log ("Dislplaying blood.");
			Debug.Log ("Fullhealth: " + fullHealth);
			Debug.Log ("Health: " + health);
			Debug.Log ((float)Mathf.Abs (fullHealth - health) / fullHealth);
			BleedBehavior.BloodAmount = (float)Mathf.Abs (fullHealth - health) / fullHealth * 2;
		}
		health -= damage;
		GameController.SetPlayerHealth (health);
		if (health <= 0) {
			GameController.EndGame ();
		}
	}
}
