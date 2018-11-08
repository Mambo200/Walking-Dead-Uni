using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            // check is player is in save area
            if(!PlayerController.PlayerSave())
            {
                // setr player status to dead
                PlayerController.m_Dead = true;

                // change UI Text
                ChangeText.ChangeTextBox(ChangeText.mP_TextDead, Color.red);

                // Get Camera
                GameObject c = GameObject.Find("Main Camera");

                // set position above player
                c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y + 50, c.transform.position.z);

                // set rotation
                c.transform.rotation = Quaternion.Euler(90,0,0);
            }
        else if (other.tag == "SaveZone")
        {
            GameObject o = GetComponentInParent<GameObject>();
            Destroy(o);
        }
    }
}
