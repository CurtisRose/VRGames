using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
	public Material targetUnhitRed;
	public Material targetHitGreen;

	// Use this for initialization
	void Start () {
		gameObject.transform.GetChild(0).GetComponent<Renderer>().material = targetUnhitRed;
	}

	void OnCollisionEnter (Collision col) {
		//Debug.Log (col.gameObject.name);
		if (col.gameObject.name == "Bullet(Clone)") {
			//Debug.Log ("Bullet hit target");
			//Debug.Log("\n\n===============================");
			//Debug.Log(gameObject.transform.GetChild (0).GetComponent<Renderer> ().material.name + " == " +  targetUnhitRed.name + " (Instance)" + " == " + targetHitGreen.name);
			col.gameObject.GetComponent<Rigidbody> ().useGravity = true;
			if (gameObject.transform.GetChild (0).GetComponent<Renderer> ().material.name == (targetUnhitRed.name + " (Instance)")) {
				col.gameObject.GetComponent<Renderer> ().material = targetHitGreen;
				gameObject.transform.GetChild (0).GetComponent<Renderer> ().material = targetHitGreen;
			} else {
				col.gameObject.GetComponent<Renderer> ().material = targetUnhitRed;
				gameObject.transform.GetChild (0).GetComponent<Renderer> ().material = targetUnhitRed;
			}
			//Debug.Log(gameObject.transform.GetChild (0).GetComponent<Renderer> ().material.name + " == " +  targetUnhitRed.name + " (Instance)" + " == " + targetHitGreen.name + " (Instance)");
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
