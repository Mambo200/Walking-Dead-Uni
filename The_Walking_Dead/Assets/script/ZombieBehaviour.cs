using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{

    [Tooltip("Speed of Zombie")]
    /// <summary>speed of Zombie</summary>
    public float m_ZombieSpeed;
    [Tooltip("Distance the Zombie can see the Player")]
    /// <summary>Distance the Zombie can see the Player</summary
    public float m_ZombieFollowDistance;
    [Tooltip("Additional Zombie Speed. If set to 0 a random Rumber will added")]
    /// <summary>additional Speed to Zombie (Calculaded in start)</summary>
    public float m_AdditionalSpeed = 0;

    /// <summary>Player</summary>
    private GameObject m_Player;
    /// <summary>Idle Behaviour String, DO NOT CHANGE!</summary>
    private string m_BehaviourIdle = "Idle";
    /// <summary>Haunt Behaviour String, DO NOT CHANGE!</summary>
    private string m_BehaviourHaunt = "Haunt";
    /// <summary>Search Behaviour String, DO NOT CHANGE!</summary>
    private string m_BehaviourSearch = "Search";
    /// <summary>previous behaviour</summary>
    private string m_PreviousBehaviour;
    /// <summary>next behaviour, set next behaviour to Idle</summary>
    private string m_NextBehaviour;
    /// <summary>whether player can be found by zombies or not </summary>
    private bool m_PlayerIsSave;

    // variables for Search Function
    /// <summary>Time Zombie is in Search state</summary>
    private float search_TimeGone = 0;

    // variable for Move Function
    /// <summary>let zombie move in opposite direction;  true -> moves to positive direction</summary>
    private bool[] move_TurnXZ= new bool[2];

    void Awake()
    {
        // inizialize move_TurnXY
        for (int i = 0; i < move_TurnXZ.GetLength(0); i++)
        {
            move_TurnXZ[i] = false;
        }

        // get player
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start()
    {
        // Set additional Zombie Speed
        // If Additional speed = 0, set random number
        if (m_AdditionalSpeed == 0)
            m_AdditionalSpeed = Random.Range(1, 5);


        // set next behaviour
        m_NextBehaviour = m_BehaviourIdle;
    }

    // Update is called once per frame
    void Update()
    {
        // if player is dead stop Zombie movement
        if (PlayerController.GameOver())
            return;

        // update Player Save State
        m_PlayerIsSave = PlayerController.PlayerSave();

        // check which behaviour is next
        switch (m_NextBehaviour)
        {
            // Play Idle behaviour
            case "Idle":
                Idle();
                m_PreviousBehaviour = m_BehaviourIdle;
                break;

            // Do Hauning behaviour
            case "Haunt":
                Haunt();
                m_PreviousBehaviour = m_BehaviourHaunt;
                break;

            // Do Searching Behaviour
            case "Search":
                Search();
                m_PreviousBehaviour = m_BehaviourSearch;
                break;

            // m_NextBehaviour was set wrong
            default:
                Debug.Log("Wrong Behaviour: " + m_NextBehaviour);
                break;
        }
    }

    // ------------------------------------------------------------------------------------------- \\

    #region Behaviours

    /// <summary>
    /// Idle Behaviour, Stands still and looking for player
    /// </summary>
    void Idle()
    {
        Move();
        // when player is in save area return
        if (m_PlayerIsSave)
            return;

        // looking for player
        if (Distance(m_ZombieFollowDistance / 2))
            m_NextBehaviour = m_BehaviourHaunt;
        else
            m_NextBehaviour = m_BehaviourIdle;
    }

    /// <summary>
    /// Follow Player
    /// </summary>
    void Haunt()
    {
        // if player is in save area set behaviour to search
        if (m_PlayerIsSave)
        {
            m_NextBehaviour = m_BehaviourSearch;
            return;
        }

        // move Zombie in direction of Player   
        this.transform.position = Vector3.MoveTowards
        (
            this.transform.position,
            m_Player.transform.position,
            m_ZombieSpeed * 1.1f * Time.deltaTime
        );

        // check and set next Behaviour
        if (Distance(m_ZombieFollowDistance / 2))
            m_NextBehaviour = m_BehaviourHaunt;
        else
            m_NextBehaviour = m_BehaviourSearch;
    }

    /// <summary>
    /// Search more intense
    /// </summary>
    void Search()
    {
        // add delteTime to multiplier
        search_TimeGone += Time.deltaTime;

        // check multiplier
        if (search_TimeGone <= 5)
        {
            // check if player is in save zone
            if (m_PlayerIsSave)
                return;

            // search player
            bool d = Distance(m_ZombieFollowDistance * 0.6f);
            // if player is in range
            if (d)
            {
                // set next behaviour to Haunt
                m_NextBehaviour = m_BehaviourHaunt;
                // reset multiplier
                search_TimeGone = 0;
            }
            // if player is not in range
            else
                m_NextBehaviour = m_BehaviourSearch;
        }
        else
        {
            // set search multiplier to 0
            search_TimeGone = 0;

            // change to Idle Behaviour
            m_NextBehaviour = m_BehaviourIdle;
        }
    }

    #endregion

    /// <summary>
    /// calculate x- and z-Distance between player and zombie
    /// </summary>
    /// <returns>Distance as Vector3</returns>
    Vector3 Distance()
    {
        Vector3 distance = new Vector3();
        distance.x = Mathf.Abs(m_Player.transform.position.x) - Mathf.Abs(this.transform.position.x);
        distance.z = Mathf.Abs(m_Player.transform.position.z) - Mathf.Abs(this.transform.position.z);

        return distance;
    }

    /// <summary>
    /// Check if player is in zombie area
    /// </summary>
    /// <param name="_distance">The Distance where zombie can see player</param>
    /// <returns>if player is in area --> true</returns>
    bool Distance(float _distance)
    {
        Vector3 distance = Distance();
        if ((distance.x >= -_distance && distance.x <= _distance) &&
           (distance.z >= -_distance && distance.z <= _distance))
            return true;
        else
            return false;
    }

    void Move()
    {
        // mit Random Range einen Float Wert in transform.translate und so den Zombie random bewegen

        float moveX = m_ZombieSpeed * Time.deltaTime;
        float moveZ = m_ZombieSpeed * Time.deltaTime;

        // check x coordinate
        if (this.gameObject.transform.position.x < -95f)
            move_TurnXZ[0] = true;
        else if (this.gameObject.transform.position.x > 95f)
            move_TurnXZ[0] = false;

        // check z coordinate
        if (this.gameObject.transform.position.z < -95f)
            move_TurnXZ[1] = true;
        else if (this.gameObject.transform.position.z > 95f)
            move_TurnXZ[1] = false;

        // if move_TurnXZ is false set move variables to negative
        if (!move_TurnXZ[0])
            moveX = -moveX;
        if (!move_TurnXZ[1])
            moveZ = -moveZ;

        // move Zombie
        this.gameObject.transform.Translate(new Vector3(
            moveX,
            0,
            moveZ
            ));
    }
}
