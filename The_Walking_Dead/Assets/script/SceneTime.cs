using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTime : MonoBehaviour {

    /// <summary>Time in the actual Scene. Reset on Reload Scene</summary>
    private static float m_SceneTime;
    /// <summary>Reset Scene Time upon set</summary>
    public static bool ResetSceneTime { get { return false; } set { m_SceneTime = 0; } }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_SceneTime += Time.deltaTime;
    }
}
