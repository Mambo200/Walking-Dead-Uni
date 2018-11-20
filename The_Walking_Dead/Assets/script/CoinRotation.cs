using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour {

    /// <summary>The rotation speed</summary>
    public float m_RotationSpeed = 4f;

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * m_RotationSpeed);
    }
}
