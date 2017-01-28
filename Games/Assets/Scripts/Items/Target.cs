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

	public void SwitchColors() {
		if (transform.GetChild (0).GetComponent<Renderer> ().material.name == (targetUnhitRed.name + " (Instance)")) {
			transform.GetChild (0).GetComponent<Renderer> ().material = targetHitGreen;
		} else {
			transform.GetChild (0).GetComponent<Renderer> ().material = targetUnhitRed;
		}
	}
}
