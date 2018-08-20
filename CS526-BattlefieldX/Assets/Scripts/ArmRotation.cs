using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 90;
	
	// Update is called once per frame
	void Update () {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        //Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //difference.Normalize();

        //float rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rotz + rotationOffset);
        transform.rotation = Quaternion.Euler(0f, 0f, Vector3.SignedAngle(Vector3.right, new Vector3(h, v, 0), Vector3.forward));
    }
}
