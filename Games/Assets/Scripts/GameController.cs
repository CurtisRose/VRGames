using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject targetPrefab;

	// Use this for initialization
	void Start () {
		Vector3 position = new Vector3 (0, 2, 5);
		var target = (GameObject)Instantiate (
			targetPrefab,
			position,
			Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
