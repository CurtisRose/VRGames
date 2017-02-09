using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinePouch : MonoBehaviour {
	public Material highlightMaterial;
	public bool hasMagazine;

	void OnTriggerEnter(Collider col) {
		//Debug.Log ("Something touched magazinePouch");
		if (col.GetComponent<Magazine>()) {
			Highlight (true);
			col.GetComponent<Magazine> ().SetTouchingMagazinePouch (true, this);
		}
	}

	void OnTriggerStay(Collider col) {
		//Debug.Log ("Something is still touching magazinePouch");
		if (col.GetComponent<Magazine>()) {
			Highlight (true);
			col.GetComponent<Magazine> ().SetTouchingMagazinePouch (true, this);
		}
	}

	void OnTriggerExit(Collider col) {
		//Debug.Log ("Something stopped touching magazinePouch");
		if (col.GetComponent<Magazine>()) {
			Highlight (false);
			col.GetComponent<Magazine> ().SetTouchingMagazinePouch (false, this);
		}
	}

	public bool AddMagazine(Magazine magazine) {
		if (!hasMagazine) {
			//Debug.Log ("Adding a magazine to the magazine pouch");
			hasMagazine = true;
			Destroy (magazine.GetComponent<Rigidbody> ());
			magazine.transform.parent = transform;
			magazine.transform.localPosition = transform.GetChild(0).transform.localPosition + magazine.holsterPosition;
			magazine.transform.localRotation = magazine.holsterRotation;
			Highlight (false);
			return true;
		}
		return false;
	}

	public bool RemoveMagazine(Magazine magazine) {
		if (hasMagazine) {
			//Debug.Log ("Removing a magazine from the magazine pouch");
			hasMagazine = false;
			magazine.gameObject.AddComponent<Rigidbody> ();
			return true;
		}
		return false;
	}

	public virtual void Highlight(bool highlight) {
		if (highlight) {
			transform.GetChild(0).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild(0).GetComponent<Renderer> ().material,
				highlightMaterial
			};
		} else {
			transform.GetChild(0).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild(0).GetComponent<Renderer> ().material,
				transform.GetChild(0).GetComponent<Renderer> ().material
			};
		}
	}
}
