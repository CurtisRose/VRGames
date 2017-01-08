using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject targetPrefab;

	// Use this for initialization
	void Start () {
		// Make full screen
		Screen.fullScreen = true;
		// Do not display cursor
		Cursor.visible = false;


		// Add target
		Vector3 position = new Vector3 (0, 2, 5);
		Instantiate (
			targetPrefab,
			position,
			Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
