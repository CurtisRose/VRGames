using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public int health = 100;
	private int fullHealth = 100;
	AudioSource[] playerSounds;
	PlayerController instance;

	public float maxWalkSpeed = 3f;
	public float deceleration = 0.1f;
	private float movementSpeed = 0f;
	private float strafeSpeed = 0f;
	private float runningMultiplier = 1.5f;

	CharacterController characterController;
	public ZombieController zombie;

	// Use this for initialization
	void Start () {
		playerSounds = GetComponents<AudioSource> ();
		GameController.SetPlayerHealth (health);
		characterController = GetComponentInChildren<CharacterController> ();
		Collider[] colliders = zombie.GetComponentsInChildren<Collider> ();
		foreach (Collider collider in colliders) {
			//Debug.Log ("Test");
			Physics.IgnoreCollision (characterController.GetComponent<Collider> (), collider);
		}
		//Debug.Log (characterController.GetComponent<Collider> ().gameObject.name);
		Physics.IgnoreCollision (characterController.GetComponent<Collider> (), zombie.GetComponent<Collider>());
	}

	private void CalculateSpeed(ref float speed, float inputValue) {
		//Debug.Log ("Calculating Speed");
		if (inputValue != 0f)
		{
			speed = (maxWalkSpeed * inputValue);
		}
		else
		{
			Decelerate(ref speed);
		}
	}

	private void Decelerate(ref float speed) {
		//Debug.Log ("Decelerating");
		if (speed > 0)
		{
			speed -= Mathf.Lerp(deceleration, maxWalkSpeed, 0f);
		}
		else if (speed < 0)
		{
			speed += Mathf.Lerp(deceleration, -maxWalkSpeed, 0f);
		}
		else
		{
			speed = 0;
		}

		float deadzone = 0.1f;
		if (speed < deadzone && speed > -deadzone)
		{
			speed = 0;
		}
	}

	public void Move() {
		//Debug.Log ("Moving");
		Vector3 movement;
		Vector3 strafe;
		float tempRunningMultiplier = 1f;
		if (GameController.movementController.controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
			tempRunningMultiplier = runningMultiplier;
		}

		//Debug.Log ("1 " + GameController.movementController.transform.forward);
		//Debug.Log ("2 " + movementSpeed);
		//Debug.Log ("3 " + tempRunningMultiplier);
		movement = GameController.movementController.transform.forward * movementSpeed * tempRunningMultiplier;// * Time.deltaTime;
		strafe = GameController.movementController.transform.right * strafeSpeed * tempRunningMultiplier;// * Time.deltaTime;
		//Debug.Log ("Movement = " + movement);
		//Debug.Log ("Strafe = " + strafe);
		float fixY = transform.position.y;
		Vector3 temp = movement + strafe;
		//transform.position += (movement + strafe);
		//transform.position = new Vector3 (transform.position.x, fixY, transform.position.z);
		//Debug.Log(temp);
		characterController.SimpleMove(temp);
	}

	private void FixedUpdate() {
		if (GameController.movementController.isReady) {
			CalculateSpeed (ref movementSpeed, GameController.movementController.controller.GetAxis().y);
			CalculateSpeed (ref strafeSpeed, GameController.movementController.controller.GetAxis().x);
			Move ();
		}
	}

	public void DoDamage(int damage) {
		//Debug.Log ("Player is being damaged");
		if (!playerSounds [0].isPlaying) {
			playerSounds [0].Play ();
		}
		if (health <= fullHealth) {
			//Debug.Log ("Dislplaying blood.");
			//Debug.Log ("Fullhealth: " + fullHealth);
			//Debug.Log ("Health: " + health);
			//Debug.Log ((float)Mathf.Abs (fullHealth - health) / fullHealth);
			BleedBehavior.BloodAmount = (float)Mathf.Abs (fullHealth - health) / fullHealth * 2;
		}
		health -= damage;
		GameController.SetPlayerHealth (health);
		if (health <= 0) {
			GameController.EndGame ();
		}
	}
}
