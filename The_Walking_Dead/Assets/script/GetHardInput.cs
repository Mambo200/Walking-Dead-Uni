using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHardInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if player is dead and press R key
        if (Input.GetKeyDown(KeyCode.R) && PlayerController.GameOver())
        {

            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            PlayerController.Reload();
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
