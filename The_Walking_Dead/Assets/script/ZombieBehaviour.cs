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

    // variables for Search Function
    /// <summary>Time Zombie is in Search state</summary>
    private float search_TimeGone = 0;

    void Awake()
    {
        // get player
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start()
    {
        // set next behaviour
        m_NextBehaviour = m_BehaviourIdle;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_NextBehaviour);
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

    // ----- Behaviours --- \\

    /// <summary>
    /// Idle Behaviour, Stands still and looking for player
    /// </summary>
    void Idle()
    {
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
        // move Zombie in direction of Player
        this.transform.position = Vector3.MoveTowards
        (
            this.transform.position,
            m_Player.transform.position,
            m_ZombieSpeed * Time.deltaTime
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

    // -------- //

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
    }

}
