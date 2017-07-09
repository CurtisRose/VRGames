using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawn : MonoBehaviour {
	public GameObject weaponToSpawn;
	public int gunCost;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.childCount < 1) {
			GameObject weapon = Instantiate (
				weaponToSpawn,
				transform.position,
				transform.rotation);
			weapon.AddComponent<Rigidbody> ();
			weapon.GetComponent<Rigidbody> ().useGravity = false;
			weapon.GetComponent<Rigidbody> ().isKinematic = true;
			weapon.transform.parent = transform;
			weapon.GetComponent<Weapon> ().numBullets = 0;
		}

	}
}
