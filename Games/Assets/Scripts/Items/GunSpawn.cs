using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSpawn : MonoBehaviour {
	public GameObject itemToSpawn;
	public int gunCost;
	public Vector3 rotation;
	TextMesh cost;


	// Use this for initialization
	void Start () {
		cost = GetComponentInChildren<TextMesh> ();
		cost.text = "$" + gunCost;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponentInChildren<Item>()) {
			GameObject item = Instantiate (
				itemToSpawn,
				transform.position,
				Quaternion.Euler(transform.rotation.eulerAngles + rotation));
			
			item.GetComponent<Rigidbody> ().isKinematic = true;
			item.transform.parent = transform;
			if (item.GetComponent<Weapon> ()) {
				item.GetComponent<Weapon> ().numBullets = 0;
			}
		}
	}
}
