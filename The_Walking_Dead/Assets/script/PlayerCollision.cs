using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    /// <summary>Time in the actual Scene. Reset on Reload Scene</summary>
    private static float m_SceneTime;
    /// <summary>Reset Scene Time</summary>
    public static bool ResetSceneTime { get { return false; } set { m_SceneTime = 0; } }



    private void Update()
    {
        m_SceneTime += Time.deltaTime;
    }

    /// <summary>
    /// when object enteres trigger
    /// </summary>
    /// <param name="other">trigger object</param>
    private void OnTriggerEnter(Collider other)
    {
        #region Player
        if (other.tag == "Player")
            // check is player is in save area
            if (!PlayerController.PlayerSave())
            {
                // set player status to dead
                PlayerController.m_Dead = true;

                // change UI Text
                ChangeText.ChangeTextBoxWinDead(ChangeText.TextDead, Color.red);

                // Get Camera
                GameObject c = GameObject.Find("Main Camera");

                // set position above player
                c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y + 50, c.transform.position.z);

                // set rotation
                c.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
        #endregion

        #region SaveZone
        if (other.tag == "SaveZone")
        {
            // get parent GameObject
            GameObject p = transform.parent.gameObject;
            // set parent to null
            transform.parent = null;
            // destroy parent Gameobject
            Destroy(p);
            // destroy Body
            Destroy(this.gameObject);
        }
        #endregion

        #region Walls
        if(other.tag == "WallFront")
        {

        }
        else if(other.tag == "WallRight")
        {

        }
        else if(other.tag == "WallLeft")
        {

        }
        else if(other.tag == "WallBack")
        {

        }
        #endregion
    }
}
