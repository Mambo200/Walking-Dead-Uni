﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHardInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        #region Key R
        // if player is dead and press R key
        if (Input.GetKeyDown(KeyCode.R) && PlayerController.GameOver())
        {
            // unload Scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            // reset Scene Time
            SceneTime.ResetSceneTime = true;
            // Load Scene
            PlayerController.Reload();
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        #endregion
    }
}
