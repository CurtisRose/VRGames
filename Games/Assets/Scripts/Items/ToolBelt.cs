using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBelt : MonoBehaviour {
	public Material highlightMaterial;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public virtual void Highlight(bool highlight) {
		if (highlight) {
			GetComponent<Renderer> ().sharedMaterials = new Material[] {
				GetComponent<Renderer> ().material,
				//highlightMaterial
			};
		} else {
			GetComponent<Renderer> ().sharedMaterials = new Material[] {
				GetComponent<Renderer> ().material,
				GetComponent<Renderer> ().material
			};
		}
	}
}
