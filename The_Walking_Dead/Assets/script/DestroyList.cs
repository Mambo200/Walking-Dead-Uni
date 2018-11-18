using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyList : MonoBehaviour {

    public static List<GameObject> DestroyThis;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (var go in DestroyThis)
        {
            Destroy(go);
        }
	}
}
