using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDoorCube : MonoBehaviour {
    /// <summary>The Transform of the Door(Cube)</summary>
    public static Transform m_DoorCube;

	// Use this for initialization
	void Awake ()
    {
        // Get Doof
        Transform[] temp = GetComponentsInChildren<Transform>();

        for (int i = 0; i < temp.GetLength(0); i++)
        {
            if(temp[i].name == "DoorCube")
            {
                // set Door to static Transform
                m_DoorCube = temp[i];
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
