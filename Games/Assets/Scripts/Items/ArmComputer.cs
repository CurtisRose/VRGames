using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmComputer : MonoBehaviour {
	int numZombiesLeft;
	int numZombiesTotal;
	TextMesh text;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<TextMesh> ();
	}

	void Awake() {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameController.GetGameOver ()) {
			text.text = "Zombies left:\t" + GameController.GetNumZombiesLeft () +
			" \\ " + GameController.GetNumZombiesLevel () + "\nLevel: \t\t\t\t" + GameController.GetLevelNumber () +
				"\nPlayer score: \t" + GameController.GetPlayerScore () + "\nHealth: \t\t\t" + GameController.GetPlayerHealth();
		} else {
			text.text = "GAMEOVER! Your score was: " + GameController.GetPlayerScore ();
		}
	}
}
