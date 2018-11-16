using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour {

    [Tooltip("The Speed with which the door moves")]
    /// <summary>The Speed with which the door moves</summary>
    public float m_DoorMoveSpeed;
    [Tooltip("The Time when the pressure plate can be activated again")]
    /// <summary>The Time when the pressure plate can be activated again</summary>
    public float m_PlateActivateTimer;
    [Tooltip("Door which shall move")]
    /// <summary>door gameobject</summary>
    public GameObject m_Door;
    /// <summary>changes when enter triggerzone</summary>
    bool m_Activate = false;
    /// <summary>trigger time which decreases every frame</summary>
    float m_TriggerTimer;

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        // reduce trigger Timer
        m_TriggerTimer -= Time.deltaTime;

        switch (m_Activate)
        {
            // Open Door
            case true:
                OpenDoor();
                break;

            // Close Door
            case false:
                CloseDoor();
                break;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        // if other is Player
        if(other.tag == "Player")
        {
            // check Triggertimer
            if (m_TriggerTimer <= 0)
            {
                m_TriggerTimer = m_PlateActivateTimer;
                m_Activate = !m_Activate;
            }
            // reset Trigger Timer
            else
            {
                m_TriggerTimer = m_PlateActivateTimer;
            }
        }
    }

    /// <summary>
    /// Opens the door.
    /// </summary>
    private void OpenDoor()
    {
        float PositionNowZ = m_Door.gameObject.transform.localPosition.z;


        // set rotation    -0,3
        if (PositionNowZ <= -0.3)
        {
            return;
        }

        // Set new position
        SetPosition(PositionNowZ - m_DoorMoveSpeed);

    }

    /// <summary>
    /// Closes the door.
    /// </summary>
    private void CloseDoor()
    {
        float PositionNowZ = m_Door.gameObject.transform.localPosition.z;

        // set rotation    0
        if (PositionNowZ >= 0)
        {
            return;
        }

        // Set new position
        SetPosition(PositionNowZ + m_DoorMoveSpeed);
    }

    /// <summary>
    /// Set the position of the Door. calculate position with speed before using
    /// </summary>
    /// <param name="_zPosition">The z position WITH SPEED.</param>
    private void SetPosition(float _zPosition)
    {
        m_Door.gameObject.transform.localPosition = new Vector3(0, 0, _zPosition);
    }
}
