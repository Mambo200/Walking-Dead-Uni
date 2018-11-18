using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour
{
    // Count Zombies
    private static int m_ZombieCount;
    /// <summary>Zombiecount Propertie</summary>
    public static int ZombieCount
    {
        get
        {
            return m_ZombieCount;
        }
        set
        {
            m_ZombieCount = value;
            if(!showZombieCountStatic)
            Debug.Log(m_ZombieCount);
        }
    }

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

    [Header("Debug")]
    // static version of showZombieCount
    private static bool showZombieCountStatic = false;
    [Tooltip("when activated zombiecount will show up in debug log")]
    // when activated zombiecount will show up in debug log
    public bool showZombieCount = false;

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
        // add one to Zombie Counter
        ZombieCount++;

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
        showZombieCountStatic = showZombieCount;
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

        //// check x coordinate
        //if (this.gameObject.transform.position.x < -95f)
        //    move_TurnXZ[0] = true;
        //else if (this.gameObject.transform.position.x > 95f)
        //    move_TurnXZ[0] = false;
        //
        //// check z coordinate
        //if (this.gameObject.transform.position.z < -95f)
        //    move_TurnXZ[1] = true;
        //else if (this.gameObject.transform.position.z > 95f)
        //    move_TurnXZ[1] = false;

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


    #region Turn Functions
    /// <summary>
    /// When collided with front wall Zombie move to another direction
    /// </summary>
    /// <param name="_wall">0 = front, 1 = right, 2 = left, 3 = back</param>
    private void TurnAround(int _wall)
    {
        switch (_wall)
        {
            case 0:
                TurnAroundWallFront();
                break;
            case 1:
                TurnAroundWallRight();
                break;
            case 2:
                TurnAroundWallLeft();
                break;
            case 3:
                TurnAroundWallBack();
                break;
            default:
                throw new System.IndexOutOfRangeException();
                break;
        }
        /* vordere Wand (xz)
         * vorher ++, danach +-
         * vorher -+, danach --
         * 
         * rechte Wand
         * vorher -+, danach ++
         * vorher --, danach +- --
         * 
         * linke Wand
         * vorher ++, danach -+
         * vorher +-, danach --
         * 
         * hintere Wand
         * vorher +-, danach ++
         * vorher --, danach -+
         */

    }

    /// <summary>
    /// change move direction when hit front wall
    /// </summary>
    private void TurnAroundWallFront()
    {
        move_TurnXZ[1] = false;
    }
    
    /// <summary>
    /// change move direction when hit right wall
    /// </summary
    private void TurnAroundWallRight()
    {
        move_TurnXZ[0] = true;
    }

    /// <summary>
    /// change move direction when hit left wall
    /// </summary>
    private void TurnAroundWallLeft()
    {
        move_TurnXZ[0] = false;
    }

    /// <summary>
    /// change move direction when hit back wall
    /// </summary>
    private void TurnAroundWallBack()
    {
        move_TurnXZ[1] = true;
    }
    #endregion

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


    /// <summary>
    /// When Object gets destroyed
    /// </summary>
    private void OnDestroy()
    {
        ZombieCount--;
    }

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
                ChangeText.ChangeTextBox(ChangeText.TextDead, Color.red);

                // Get Camera
                GameObject c = GameObject.Find("Main Camera");

                // set position above player
                c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y + 50, c.transform.position.z);

                // set rotation
                c.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
        #endregion

        #region SaveZone / End
        // if Zombie enters Savearea destroy it
        if (other.tag == "SaveZone" || other.tag == "Finish")
        {

            #region if this is Body
            // get parent GameObject
            //GameObject p = transform.parent.gameObject;
            // set parent to null
            //transform.parent = null;
            // destroy parent Gameobject
            //Destroy(p);
            #endregion
            // destroy Gameobject
            Destroy(this.gameObject);
        }
        #endregion

    }

    private void OnCollisionEnter(Collision collision)
    {
        #region Walls
        if (collision.transform.tag == "WallFront")
        {
            TurnAround(0);
        }
        else if (collision.transform.tag == "WallRight")
        {
            TurnAround(1);
        }
        else if (collision.transform.tag == "WallLeft")
        {
            TurnAround(2);
        }
        else if (collision.transform.tag == "WallBack")
        {
            TurnAround(3);
        }
        #endregion

    }
}
