using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            PlayerController.m_Dead = true;
        else if (other.tag == "SaveZone")
        {
            GameObject o = GetComponentInParent<GameObject>();
            Destroy(o);
        }
    }
}
