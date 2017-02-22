using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1911Controller : Weapon {

	protected override void Start () {
		highlightObject = transform.GetChild (0).gameObject;
		base.Start ();
		gripRotation = Quaternion.Euler (65, 0, 0);
		gripPosition = new Vector3 (0.0f, -0.016f, -0.067f);
		magazineRotation = Quaternion.Euler (-90, 0, 0);
		magazinePosition = new Vector3 (0.0f, -0.0063f, -0.018f);
		gunSounds = GetComponents<AudioSource>();
		hasMagazine = false;
		hasGripPosition = true;
		gunName = "M1911";
		holsterPosition = new Vector3 (0.0f, 2.07f, 0.0f);
		primaryWeapon = false;
		aimable = false;
	}

	/*public override void OnMenuDown(WandController controller) {
		if (magazine) {
			magazine.transform.parent = null;
			if (!magazine.GetComponent<Rigidbody> ()) {
				magazine.gameObject.AddComponent<Rigidbody> ();
			}
			magazine.gameObject.GetComponent<Collider> ().isTrigger = false;
			Physics.IgnoreCollision(GetComponent<Collider>(), magazine.GetComponent<Collider>());
			Unload (magazine);
		}
	}*/

	public override void Highlight(bool setHighlight) {
		base.Highlight (setHighlight);
		if (setHighlight) {
			transform.GetChild (1).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (1).GetComponent<Renderer> ().material,
				highlightMaterial
			};
			transform.GetChild (2).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (2).GetComponent<Renderer> ().material,
				highlightMaterial
			};
			transform.GetChild (3).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (3).GetComponent<Renderer> ().material,
				highlightMaterial
			};
			transform.GetChild (4).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (4).GetComponent<Renderer> ().material,
				highlightMaterial
			};
		} else {
			transform.GetChild (1).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (1).GetComponent<Renderer> ().material,
				transform.GetChild (1).GetComponent<Renderer> ().material,
			};
			transform.GetChild (2).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (2).GetComponent<Renderer> ().material,
				transform.GetChild (2).GetComponent<Renderer> ().material,
			};
			transform.GetChild (3).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (3).GetComponent<Renderer> ().material,
				transform.GetChild (3).GetComponent<Renderer> ().material,
			};
			transform.GetChild (4).GetComponent<Renderer> ().sharedMaterials = new Material[] {
				transform.GetChild (4).GetComponent<Renderer> ().material,
				transform.GetChild (4).GetComponent<Renderer> ().material,
			};
		}
	}
}
