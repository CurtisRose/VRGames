using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
	public Material targetUnhit;
	public Material targetHit;

	// Use this for initialization
	void Start () {
		gameObject.transform.GetChild(0).GetComponent<Renderer>().material = targetUnhit;
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "Bullet(Clone)") {
			//Debug.Log ("Bullet hit target");
			if (gameObject.transform.GetChild (0).GetComponent<Renderer> ().material.name == (targetUnhit.name + " (Instance)")) {
				gameObject.transform.GetChild (0).GetComponent<Renderer> ().material = targetHit;
			} else {
				gameObject.transform.GetChild (0).GetComponent<Renderer> ().material = targetUnhit;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
