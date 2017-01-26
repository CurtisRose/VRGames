using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		//Debug.Log ("Bullet hit " + col.gameObject.name);
		Destroy (gameObject);
	}
}
