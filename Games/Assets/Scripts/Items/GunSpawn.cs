using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GunSpawn : MonoBehaviour {
	public GameObject itemToSpawn;
	public int gunCost;
	public Vector3 position;
	public Vector3 rotation;
	TextMeshPro test;


	// Use this for initialization
	void Start () {
		test = GetComponentInChildren<TextMeshPro> ();
		test.text = "$" + gunCost;
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
			item.transform.localPosition += position;
			if (item.GetComponent<Weapon> ()) {
				item.GetComponent<Weapon> ().numBullets = 0;
			}
		}
	}
}
