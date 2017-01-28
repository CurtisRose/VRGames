using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleRedDot : Attachment {
	protected RenderTexture renderTexture;
	public Material crosshairs;

	// Use this for initialization
	protected override void Start () {
		highlightObject = transform.GetChild(0).gameObject;
		base.Start ();
		attachmentType = "Optics";
		attachmentPosition = new Vector3 (0.0f, -28.0f , 2.5f);

		//Camera attempt
		/*renderTexture = new RenderTexture(256, 256, 24);
		renderTexture.Create ();
		transform.GetChild (0).GetChild (0).GetComponent<MeshRenderer> ().materials [0].SetTexture ("_MainTex", renderTexture);
		transform.GetChild (0).GetChild (0).GetComponent<MeshRenderer> ().materials [0].shader = Shader.Find ("Unlit/Texture");
		transform.GetChild (0).GetChild (0).GetComponent<MeshRenderer> ().materials [0].name = "lensView";
		transform.GetComponentInChildren<Camera> ().targetTexture = 
			(RenderTexture)transform.GetChild (0).GetChild (0).GetComponent<MeshRenderer> ().materials [0].mainTexture;
		transform.GetComponentInChildren<Camera> ().targetTexture.name = "scope";
		//transform.GetComponentInChildren<Camera> ().transform.rotation = Quaternion.Euler (-180, 0, 180);
		*/
	}
}
