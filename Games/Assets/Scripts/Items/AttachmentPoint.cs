using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour {
	//public GameObject self = gameObject;

	public Material highlightMaterial;
	private Material oldMaterial;

	public string attachmentPointType = null;
	public Attachment attachment = null;

	void Start() {
		oldMaterial = gameObject.GetComponent<Renderer> ().material;
	}

	public virtual void Highlight(bool highlight) {
		//Debug.Log ("Highlighting Laser");
		if (highlight) {
			gameObject.GetComponent<Renderer> ().material = highlightMaterial;
		} else {
			gameObject.GetComponent<Renderer> ().material = oldMaterial;
		}
	}

	public void MakeClear() {
		GetComponent<MeshRenderer> ().enabled = false;
	}

	public void MakeUnclear() {
		GetComponent<MeshRenderer> ().enabled = true;
	}

	//public void ToggleCollider() {
	//	gameObject.GetComponent<Collider> ().enabled = !gameObject.GetComponent<Collider> ().enabled;
	//}
}
