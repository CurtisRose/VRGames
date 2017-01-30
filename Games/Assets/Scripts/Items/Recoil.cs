using UnityEngine;
using System.Collections;
 
public class Recoil : MonoBehaviour
{
    private float recoil = 0.0f;
    private float maxRecoil_x = -20f;
    private float maxRecoil_y = 20f;
    private float recoilSpeed = 2f;
 
    public void StartRecoil (float recoilParam, float maxRecoil_xParam, float recoilSpeedParam)
    {
        // in seconds
        recoil = recoilParam;
        maxRecoil_x = maxRecoil_xParam;
        recoilSpeed = recoilSpeedParam;
        maxRecoil_y = Random.Range(-maxRecoil_xParam, maxRecoil_xParam);
    }
 
    void recoiling ()
    {
        if (recoil > 0f) {
			Quaternion maxRecoil = Quaternion.Euler (
				transform.localRotation.eulerAngles.x - maxRecoil_x, 
				transform.localRotation.eulerAngles.y + maxRecoil_y, 
				transform.localRotation.eulerAngles.z);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp (transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        } else {
            recoil = 0f;
            // Dampen towards the target rotation
			if (gameObject.GetComponent<Weapon>().GetIsHeld()) {
				//Debug.Log("Test");
				transform.localRotation = Quaternion.Slerp (transform.localRotation, gameObject.GetComponent<Weapon>().GetGripRotation(), Time.deltaTime * recoilSpeed * 2);
			}
        }
    }
 
    // Update is called once per frame
    void Update ()
    {
        recoiling ();
    }
}
