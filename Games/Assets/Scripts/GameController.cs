using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static int numZombies = 0;
	public static int numZombiesLevel;
	public static int levelNumber = 0;
	static GameController instance;
	public int numSpawnpoints;
	private int spawnTime = 5;
	private PlayerController playerController;
	private static int playerScore;
	private static int zombiePoints = 100;
	private static int levelPoints = 0;
	private static int zombieHealth = 100;
	private static bool gameOver = false;
	private static int playerHealth;


	public static GameController GetInstance() {
		if (instance == null) {
			instance = new GameController ();
		}
		return instance;
	}

	private Spawnpoint[] spawnpoints;
	// Use this for initialization
	void Start () {
		//Debug.Log ("Starting Game");
		// Make full screen
		Screen.fullScreen = true;
		// Do not display cursor
		Cursor.visible = false;
		spawnpoints = GetComponentsInChildren<Spawnpoint> ();
		numSpawnpoints = spawnpoints.Length;
		//playerController = PlayerController.GetInstance ();
	}

	public void KillZombie() {
		numZombies--;
		playerScore += zombiePoints;
		//Debug.Log("Number of zombies left = " + numZombies);
	}

	// Update is called once per frame
	void Update () {
		if (numZombies <= 0) {
			//Debug.Log ("Starting Level...");
			NewLevel ();
		}
	}

	void NewLevel() {
		levelNumber++;
		playerScore += levelPoints;
		levelPoints += 100;
		//playerController.DisplayLevel (levelNumber);
		Debug.Log ("Starting Level " + levelNumber + " Now");
		numZombies = numSpawnpoints + numZombiesLevel;
		numZombiesLevel = numSpawnpoints + numZombiesLevel;
		foreach (Spawnpoint point in spawnpoints) {
			point.target = GetComponentInChildren<PlayerController> ().transform.GetChild (0).GetChild (2).transform;
			//point.target = GameObject.FindWithTag("Player").transform;
			point.PrepareSpawning (levelNumber, spawnTime, zombieHealth);
		}
		spawnTime -= 1;
		zombieHealth += 10;
	}

	public static void EndGame() {
		//Debug.Log ("Game Over");
		gameOver = true;
	}

	public static bool GetGameOver() {
		return gameOver;
	}

	public static int GetNumZombiesLeft() {
		return numZombies;
	}

	public static int GetNumZombiesLevel() {
		return numZombiesLevel;
	}

	public static int GetPlayerScore() {
		return playerScore;
	}

	public static int GetLevelNumber() {
		return levelNumber;
	}

	public static int GetPlayerHealth() {
		return playerHealth;
	}

	public static void SetPlayerHealth(int setHealth) {
		playerHealth = setHealth;
	}
}
