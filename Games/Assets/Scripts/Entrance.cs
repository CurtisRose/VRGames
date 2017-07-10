using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entrance : MonoBehaviour {
	public Material highlightMaterial;
	public bool isOpen;
	public int cost;
	TextMeshPro text;

	// Use this for initialization
	void Start () {
		isOpen = false;
		text = GetComponentInChildren<TextMeshPro> ();
		text.text = "$" + cost;
		text.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Open() {
		Debug.Log ("Trying to open this entrance.");
		if (GameController.GetPlayerScore() >= cost) {
			GameController.SetPlayerScore (GameController.GetPlayerScore() - cost);
			isOpen = true;
			gameObject.SetActive (false);
		}
	}

	public virtual void Highlight(bool highlight) {
		if (highlight) {
			text.alpha = 1;
			GetComponent<Renderer> ().sharedMaterials = new Material[] {
				GetComponent<Renderer> ().material,
				highlightMaterial
			};
		} else {
			text.alpha = 0;
			GetComponent<Renderer> ().sharedMaterials = new Material[] {
				GetComponent<Renderer> ().material,
				GetComponent<Renderer> ().material
			};
		}
	}

	public virtual void OnTriggerDown(WandController controller) {
		Debug.Log ("Pressed trigger on entrance.");
		Open();
	}

	public virtual void OnTriggerUp(WandController controller) {

	}

	public virtual void OnTriggerHeld(WandController controller) {

	}

	public virtual void OnHairTriggerDown(WandController controller) {

	}

	public virtual void OnHairTriggerUp(WandController controller) {

	}

	public virtual void OnHairTriggerHeld(WandController controller) {

	}

	public virtual void OnGripDown(WandController controller) {
		
	}

	public virtual void OnGripUp(WandController controller) {

	}

	public virtual void OnGripHeld(WandController controller) {

	}

	public virtual void OnTouchpadDown(WandController controller, Vector2 touchPosition) {

	}

	public virtual void OnTouchpadUp(WandController controller) {

	}

	public virtual void OnTouchpadHeld(WandController controller, Vector2 touchPosition) {

	}

	public virtual void OnMenuDown(WandController controller) {

	}

	public virtual void OnMenuUp(WandController controller) {

	}

	public virtual void OnMenuHeld(WandController controller) {

	}
}
