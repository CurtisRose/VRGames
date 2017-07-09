using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holster : MonoBehaviour {
	public Material highlightMaterial;
	private Material oldMaterial;
	private Weapon primaryWeapon;
	private Magazine primaryMagazine;
	private Weapon secondaryWeapon;
	private Magazine secondaryMagazine;
	private bool primaryGrenade;
	private bool secondaryGrenade;
	private int numPrimaryMagazines = 0;

	// Use this for initialization
	void Start () {
		oldMaterial = gameObject.GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		Transform head = transform.parent.GetComponentInChildren<Camera>().transform;
		transform.rotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, head.eulerAngles.y, transform.eulerAngles.z));
		transform.localPosition = new Vector3 (head.localPosition.x, head.localPosition.y - .75f, head.localPosition.z);
	}

	protected virtual void OnTriggerEnter(Collider col) {
		if (col.GetComponent<Weapon> ()) {
			Highlight (true);
			col.GetComponent<Weapon> ().holster = this;
			col.GetComponent<Weapon> ().SetTouchingHolster (true);
		}
	}

	protected virtual void OnTriggerStay(Collider col) {
		if (col.GetComponent<Weapon>()) {
			Highlight (true);
			col.GetComponent<Weapon> ().holster = this;
			col.GetComponent<Weapon> ().SetTouchingHolster (true);
		}
	}

	protected virtual void OnTriggerExit(Collider col) {
		if (col.GetComponent<Weapon>()) {
			Highlight (false);
			col.GetComponent<Weapon> ().holster = null;
			col.GetComponent<Weapon> ().SetTouchingHolster (false);
		}
	}

	public bool HolsterWeapon(Weapon weapon, bool isLeft) {
		//Debug.Log ("Holstering weapon");
		weapon.transform.parent = transform;
		//weapon.transform.localPosition = weapon.holsterPosition;
		weapon.transform.localRotation = transform.rotation;
		weapon.transform.localRotation = Quaternion.Euler (new Vector3 (weapon.transform.localRotation.x + 45f, weapon.transform.localRotation.y - 45, weapon.transform.localRotation.z));
		Highlight (false);
		Destroy (weapon.GetComponent<Rigidbody> ());
		if (weapon.GetIsPrimaryWeapon ()) {
			if (!primaryWeapon) {
				//Debug.Log ("Holstering primary weapon");
				weapon.SetIsHolstered (true);
				primaryWeapon = weapon;
				if (isLeft) {
					weapon.transform.localRotation = Quaternion.Euler (new Vector3 (weapon.transform.localRotation.x + 45f, weapon.transform.localRotation.y + 45, weapon.transform.localRotation.z));
					weapon.transform.localPosition = new Vector3 (-weapon.holsterPosition.x, weapon.holsterPosition.y, weapon.holsterPosition.z);
				} else {
					weapon.transform.localRotation = Quaternion.Euler (new Vector3 (weapon.transform.localRotation.x + 45f, weapon.transform.localRotation.y - 45, weapon.transform.localRotation.z));
					weapon.transform.localPosition = new Vector3 (weapon.holsterPosition.x, weapon.holsterPosition.y, weapon.holsterPosition.z);
				}
				return true;
			} else {
				//Debug.Log ("No room for another primary weapon.");
				return false;
			}
		} else {
			if (!secondaryWeapon) {
				//Debug.Log ("Holstering secondary weapon");
				weapon.SetIsHolstered (true);
				secondaryWeapon = weapon;
				if (isLeft) {
					weapon.transform.localRotation = Quaternion.Euler (new Vector3 (weapon.transform.localRotation.x + 45f, weapon.transform.localRotation.y + 45, weapon.transform.localRotation.z));
					weapon.transform.localPosition = new Vector3 (-weapon.holsterPosition.x, weapon.holsterPosition.y, weapon.holsterPosition.z);
				} else {
					weapon.transform.localRotation = Quaternion.Euler (new Vector3 (weapon.transform.localRotation.x + 45f, weapon.transform.localRotation.y - 45, weapon.transform.localRotation.z));
					weapon.transform.localPosition = new Vector3 (weapon.holsterPosition.x, weapon.holsterPosition.y, weapon.holsterPosition.z);
				}
				return true;
			} else {
				//Debug.Log ("No room for another secondary weapon.");
				return false;
			}
		}
	}

	public void UnholsterWeapon(Weapon weapon) {
		weapon.SetIsHolstered (false);
		if (weapon == primaryWeapon) {
			Debug.Log ("Unholstering the primary weapon.");
			primaryWeapon = null;
		} else if (weapon == secondaryWeapon) {
			Debug.Log ("Unholstering the secondary weapon.");
			secondaryWeapon = null;
		} else {
			Debug.Log ("This weapon is not holstered");
		}
	}

	public bool HolsterMagazine(Magazine magazine) {
		Debug.Log ("Holstering magazine");
		magazine.transform.parent = transform;
		magazine.SetIsHolstered (true);
		//weapon.transform.localPosition = weapon.holsterPosition;
		magazine.transform.localRotation = transform.rotation;
		magazine.transform.localRotation = Quaternion.Euler (new Vector3 (magazine.transform.localRotation.x + 90, magazine.transform.localRotation.y + 90, magazine.transform.localRotation.z));
		magazine.transform.localPosition = new Vector3 (magazine.holsterPosition.x, magazine.holsterPosition.y, magazine.holsterPosition.z + .3f);
		Highlight (false);
		Destroy (magazine.GetComponent<Rigidbody> ());
		if (numPrimaryMagazines == 0) {
			Debug.Log ("Adding first primary magazine");
			numPrimaryMagazines += 1;
			primaryMagazine = magazine;
			return true;
		} else if (numPrimaryMagazines > 0 && primaryMagazine.GetType() == magazine.GetType()) {
			numPrimaryMagazines += 1;
			Debug.Log ("Adding another primary magazine");
			return true;
		} else {
			Debug.Log ("Not the same type of magazine");
			return false;
		}
	}

	public void UnholsterWeapon(Magazine magazine) {
		magazine.SetIsHolstered (false);
		if (magazine == primaryMagazine && numPrimaryMagazines > 1) {
			Debug.Log ("Unholstering a primary magazine.");
			numPrimaryMagazines -= 1;
		} else if(magazine == primaryMagazine && numPrimaryMagazines == 1) {
			Debug.Log ("Unholstering last primary magazine.");
			numPrimaryMagazines -= 1;
			primaryMagazine = null;
		}else {
			Debug.Log ("This magazine is not holstered");
		}
	}

	public virtual void Highlight(bool highlight) {
		if (highlight) {
			GetComponent<Renderer> ().sharedMaterials = new Material[] {
				GetComponent<Renderer> ().material,
				highlightMaterial
			};
		} else {
			GetComponent<Renderer> ().sharedMaterials = new Material[] {
				GetComponent<Renderer> ().material,
				GetComponent<Renderer> ().material
			};
		}
	}
}
