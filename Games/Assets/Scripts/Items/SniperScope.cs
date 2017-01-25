using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScope : Attachment {
	protected RenderTexture renderTexture;
	public Material crosshairs;
	protected override void Start () {
		highlightObject = transform.GetChild (0).GetChild (0).gameObject;
		base.Start ();
		attachmentType = "Optics";
		attachmentPosition = new Vector3 (4, 0 ,0);

		renderTexture = new RenderTexture(256, 256, 24);
		renderTexture.Create ();
		transform.GetChild (0).GetChild (2).GetComponent<MeshRenderer> ().materials [0].SetTexture ("_MainTex", renderTexture);
		transform.GetChild (0).GetChild (2).GetComponent<MeshRenderer> ().materials [0].name = "lensView";
		transform.GetComponentInChildren<Camera> ().targetTexture = 
			(RenderTexture)transform.GetChild (0).GetChild (2).GetComponent<MeshRenderer> ().materials [0].mainTexture;
		transform.GetComponentInChildren<Camera> ().targetTexture.name = "scope";
		/*transform.GetChild (0).GetChild (2).GetComponent<MeshRenderer> ().materials[1] = crosshairs;
		transform.GetChild (0).GetChild (2).GetComponent<MeshRenderer> ().materials [1].shader = Shader.Find ("Unlit/Transparent");
		transform.GetChild (0).GetChild (2).GetComponent<MeshRenderer> ().materials [1].name = "crosshairs";*/
	}
}
