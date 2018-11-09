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
    [Tooltip("Material to show current Behaviour. 0 = Idle, 1 = Haunt, 2 = Search")]
    /// <summary>Material to show current Behaviour. 0 = Idle, 1 = Haunt, 2 = Search</summary>
    public Material[] m_ZombieColor;

    /// <summary>Player</summary>
    private GameObject m_Player;
    /// <summary>Idle Behaviour String</summary>
    private string BehaviourIdle { get { return "Idle"; } }
    /// <summary>Haunt Behaviour String</summary>
    private string BehaviourHaunt { get { return "Haunt"; } }
    /// <summary>Search Behaviour String</summary>
    private string BehaviourSearch { get { return "Search"; } }
    /// <summary>previous behaviour</summary>
    private string m_PreviousBehaviour;
    /// <summary>next behaviour, set next behaviour to Idle</summary>
    private string m_NextBehaviour;
    /// <summary>whether player can be found by zombies or not </summary>
    private bool m_PlayerIsSave;
    /// <summary>Body of Gameobject</summary>
    private GameObject m_Body;
    /// <summary>Material of Body</summary>
    private Material m_Material;

    // variables for Search Function
    /// <summary>Time Zombie is in Search state</summary>
    private float search_TimeGone = 0;

    // variable for Move Function
    /// <summary>let zombie move in opposite direction;  true -> moves to positive direction</summary>
    private bool[] move_TurnXZ= new bool[2];
    /// <summary>Time gone since lase save</summary>
    private float move_SaveTime = 0;

    void Awake()
    {
        // get body of Zombie
        m_Body = this.transform.GetChild(0).gameObject;
        // get material ob Body
        m_Material = m_Body.GetComponent<Renderer>().material; 

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
        m_NextBehaviour = BehaviourIdle;
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
                m_PreviousBehaviour = BehaviourIdle;
                break;

            // Do Hauning behaviour
            case "Haunt":
                Haunt();
                m_PreviousBehaviour = BehaviourHaunt;
                break;

            // Do Searching Behaviour
            case "Search":
                Search();
                m_PreviousBehaviour = BehaviourSearch;
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
        // Change Material
        if (m_PreviousBehaviour != BehaviourIdle)
            ChangeColor(0);

        Move();
        // when player is in save area return
        if (m_PlayerIsSave)
            return;

        // looking for player
        if (Distance(m_ZombieFollowDistance / 2))
            m_NextBehaviour = BehaviourHaunt;
        else
            m_NextBehaviour = BehaviourIdle;
    }

    /// <summary>
    /// Follow Player
    /// </summary>
    void Haunt()
    {
        // Change Material
        if (m_PreviousBehaviour != BehaviourHaunt)
            ChangeColor(1);

        // if player is in save area set behaviour to search
        if (m_PlayerIsSave)
        {
            m_NextBehaviour = BehaviourSearch;
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
            m_NextBehaviour = BehaviourHaunt;
        else
            m_NextBehaviour = BehaviourSearch;
    }

    /// <summary>
    /// Search more intense
    /// </summary>
    void Search()
    {
        // Change Material
        if (m_PreviousBehaviour != BehaviourSearch)
            ChangeColor(2);

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
                m_NextBehaviour = BehaviourHaunt;
                // reset multiplier
                search_TimeGone = 0;
            }
            // if player is not in range
            else
                m_NextBehaviour = BehaviourSearch;
        }
        else
        {
            // set search multiplier to 0
            search_TimeGone = 0;

            // change to Idle Behaviour
            m_NextBehaviour = BehaviourIdle;
        }
    }

    #endregion

    #region calculaton
    /// <summary>
    /// calculate x- and z-Distance between player and zombie
    /// </summary>
    /// <returns>Distance as square float</returns>
    float Distance()
    {
        float distance;
        distance = Vector3.SqrMagnitude(m_Player.transform.position - this.transform.position);
        return distance;
    }

    /// <summary>
    /// Check if player is in zombie area
    /// </summary>
    /// <param name="_distance">The Distance where zombie can see player</param>
    /// <returns>if player is in area --> true</returns>
    bool Distance(float _distance)
    {
        float distance = Distance();
        if (distance <= _distance * _distance)
            return true;
        else
            return false;
    }

    /// <summary>
    /// get random bool
    /// </summary>
    /// <returns>random bool</returns>
    private bool RandomBool()
    {
        float rndNr = Random.Range(0, 2);
        if (rndNr == 0)
            return false;
        else
            return true;
    }
    #endregion

    /// <summary>
    /// Move Zombie
    /// </summary>
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

        // change directions random
        move_SaveTime += Time.deltaTime;
        if(move_SaveTime >= 5)
        {
            for (int i = 0; i < move_TurnXZ.GetLength(0); i++)
            {
                move_TurnXZ[i] = RandomBool();
            }

            // reset time
            move_SaveTime = 0;
        }
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

    /// <summary>
    /// Change Color of Body
    /// </summary>
    /// <param name="_index">0 = Idle, 1 = Haunt, 2 = Search</param>
    private void ChangeColor(int _index)
    {
        // check if index is right
        if(_index >= 0 && _index < m_ZombieColor.GetLength(0))
            // change color
            m_Material.CopyPropertiesFromMaterial(m_ZombieColor[_index]);
    }

}
