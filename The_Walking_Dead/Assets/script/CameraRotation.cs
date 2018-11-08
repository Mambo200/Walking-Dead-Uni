using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {

    /// <summary>Mouse Rotation Speed around X-Axis</summary>
    public float m_RotSpeedX = 1;

    /// <summary>cursor movement</summary>
    private float MouseY;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if player dies or won return
        if (PlayerController.GameOver())
            return;

        // get mouse axis input
        MouseY = Input.GetAxis("Mouse Y");

        // set camera function
        SetCameraRotation();
    }

    // ------------------------------------------------------------------------------------------- \\
    /// <summary>
    /// Set Camera Rotation
    /// </summary>
    private void SetCameraRotation()
    {
        float x = this.transform.eulerAngles.x - MouseY;
        this.gameObject.transform.eulerAngles = new Vector3(
            x,
            gameObject.transform.eulerAngles.y,
            gameObject.transform.eulerAngles.z
            );
    }
}
