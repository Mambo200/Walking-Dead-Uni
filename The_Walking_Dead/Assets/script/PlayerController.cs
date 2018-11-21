using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [HideInInspector]
    /// <summary>if player collides with Zombie -> Dead = true</summary>
    public static bool Dead
    {
        get
        {
            return m_Dead;
        }
        set
        {
            if (!value)
            {
                m_Dead = value;
                m_DeadResetTimer = 0;
                return;
            }

            if(Lives>0)
            {
                --Lives;
                m_DeadResetTimer = 0;
            }
            else
            {
                // set player status to dead
                m_Dead = true;
               
                // change UI Text
                ChangeText.ChangeTextBoxWinDead(ChangeText.TextDead, Color.red);

            }

        }
    }
    private static bool m_Dead = false;

    /// <summary>Resets time after respawn</summary>
    private static float m_DeadResetTimer;

    [HideInInspector]
    /// <summary>when Player is in Save Zone m_Save is true</summary>
    public static bool m_Save = false;
    [HideInInspector]
    /// <summary>when Player reached finish</summar>
    public static bool m_Won = false;
    /// <summary>Lives of Player</summar>
    public static int Lives
    {
        get {return m_Lives;}
        set
        {
            m_Lives = value;
            ChangeText.ChangeTextBoxLives(m_Lives);
        }
    }

    /// <summary>Coins of Player</summar>
    public static int Coins { get { return m_Coins; } set { m_Coins = value; } }
    /// <summary>Amount of Food of Player</summary>
    public static int Food { get { return m_Food; } set { m_Food = value; } }
    /// <summary>Key which is needed to complete the game</summary>
    public static bool Key
    {
        get
        {
            return m_Key;
        }
        set
        {
            m_Key = value;
            ChangeText.ChangeKeyImage(true);
        }
    }

    [Tooltip("player walk speed")]
    /// <summary>player walk speed</summary>
    public float m_Speed;
    [Tooltip("player rotation speed")]
    /// <summary>player rotation speed</summary>
    public float m_RotSpeed = 1;
    [Tooltip("player fall speed")]
    /// <summary>fall speed of player</summary>
    public float m_FallSpeed = 1;
    /// <summary>horizontal input</summary>
    private float m_Horizontal;
    /// <summary>vertical input</summary>
    private float m_Vertical;
    /// <summary>Mouse X input</summary>
    private float m_RotationY;

    // Items
    /// <summary>Lives of Player</summary>
    private static int m_Lives;
    /// <summary>Coins of Player</summary>
    private static int m_Coins;
    /// <summary>Amount of Food of Player</summary>
    private static int m_Food;
    /// <summary>Key which is needed to complete the game</summary>
    private static bool m_Key;

    private CharacterController m_CharacterController;

    // Use this for initialization
    void Start ()
    {
        // lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // get character controller
        m_CharacterController = this.gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if respawn dead timer < 5 return
        if (m_DeadResetTimer <= 5f)
        {
            m_Save = true;
        }
        else
        {
            m_Save = false;
        }

        // if player is dead or won return
        if (m_Dead || m_Won)
            return;

        // update Respawn Timer
        m_DeadResetTimer += Time.deltaTime;

        // Move
        // get Axis input
        m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = Input.GetAxis("Vertical");
        m_RotationY = Input.GetAxis("Mouse X");

        // set input
        Move();

        // Rotate
        Rotate();
    }

    // ------------------------------------------------------------------------------------------- \\
    #region Player Movement
    /// <summary>
    /// Move Character
    /// </summary>
    void Move()
    {
        // check if player is grounded
        if (m_CharacterController.isGrounded)
            // do not fall
            Move(m_Horizontal, m_Vertical, 0);
        else
            // fall
            Move(m_Horizontal, m_Vertical, m_FallSpeed);
    }

    /// <summary>
    /// Move Character to x- and z-Axis
    /// </summary>
    /// <param name="_horizontal">horizontal input</param>
    /// <param name="_vertical">vertical input</param>
    void Move(float _horizontal, float _vertical, float _jump)
    {
        Vector3 v = new Vector3();
        float speed = m_Speed * Time.deltaTime;
        // set vector
        v.x = _horizontal;
        v.y = - _jump;
        v.z = _vertical;

        // multiply by speed and Run
        v = (v.normalized * speed * Run());
        // transform from world into local space
        v = transform.TransformDirection(v);
        // move player
        m_CharacterController.Move(v);
    }

    /// <summary>
    /// Run Speed
    /// </summary>
    /// <returns>run value</returns>
    float Run()
    {
        /// <summary>if left shift is pressed = 1, if not = 0</summary>
        float run = Input.GetAxis("Fire3");

        // check if button is pressed
        if (run > 0)
            // run
            return run * 1.5f;
        else
            // walk
            return 1;
    }

    /// <summary>
    /// Rotate Player
    /// </summary>
    void Rotate()
    {
        // if cursor not locked return
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

            float y = this.transform.eulerAngles.y + (m_RotationY * m_RotSpeed * Time.deltaTime);
        this.gameObject.transform.eulerAngles = new Vector3(
            gameObject.transform.eulerAngles.x,
            y,
            gameObject.transform.eulerAngles.z
            );

    }
    #endregion

    #region Public Static Functions
    /// <summary>
    /// check if player is in savezone, won the game or died
    /// </summary>
    /// <returns>Player save state</returns>
    public static bool PlayerSave()
    {
        // if player is not in savezone, not dead or did not won the game return true
        if (m_Save || m_Dead || m_Won)
            return true;
        else
            return false;
    }

    /// <summary>
    /// check if game is over either win or lose
    /// </summary>
    /// <returns>player game over state</returns>
    public static bool GameOver()
    {
        if (m_Dead || m_Won)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Set values to default
    /// </summary>
    public static void Reload()
    {
        // set savestates to false
        m_Save = false;
        m_Dead = false;
        m_Won = false;

        // Reset lives and Coins
        m_Coins = 0;
        m_Lives = 0;

        // change UI Text
        ChangeText.ChangeTextBoxWinDead("", Color.white);
    }
    #endregion

    #region Trigger Events
    private void OnTriggerEnter(Collider other)
    {
        #region Savezones

        #region Savezone
        if (other.gameObject.tag == "SaveZone")
            m_Save = true;
        #endregion

        #region Finish
        else if (other.gameObject.tag == "Finish")
        {
            // If player has key
            if (Key)
            {
                m_Won = true;
                ChangeText.ChangeTextBoxWinDead(ChangeText.TextWin, Color.green);
            }

            // if player dont have key
            else
            {
                ChangeText.ChangeTextBoxWinDead("Key is Missing!", Color.red);
            }
        } 
        #endregion

        #endregion

        #region Item
        if(other.gameObject.tag == "Item")
        {
            #region One Up
            // 1UP
            if (other.gameObject.name == "1UP")
            {
                ++Lives;
                Destroy(other.gameObject);
            }
            #endregion

            #region Coin
            // Coin
            else if (other.gameObject.name == "Coin")
            {
                ++m_Coins;
                Destroy(other.gameObject);
            }
            #endregion

            #region Key
            // Key
            else if (other.gameObject.name == "Key")
            {
                Key = true;
                Destroy(other.gameObject);
            }
            #endregion

            #region Food
            else if(other.gameObject.name == "Food")
            {
                ++m_Food;
                Destroy(other.gameObject);
            }
            #endregion

        }
        #endregion


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SaveZone")
            m_Save = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SaveZone")
            m_Save = false;

        if (other.gameObject.tag == "Finish")
        {
            ChangeText.ChangeTextBoxWinDead("", Color.black);
        }

    }
    #endregion

    #region Collision Events
    private void OnCollisionEnter(Collision collision)
    {
        // if collided with enemy
        if (collision.collider.tag == "Enemy")
        {

            // when player is already dead skip
            if (m_Dead == false)
            {

                // when player is not safe
                if (!PlayerSave())
                {
                    // if player has lives
                    if (Lives > 0)
                    {
                        Dead = true;
                        this.gameObject.transform.localPosition = new Vector3(0, 20f, 0);

                    }
                    // player has no lives
                    else
                    {
                        Dead = true;

                        // Get Camera
                        GameObject c = GameObject.Find("Main Camera");

                        // set position above player
                        c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y + 50, c.transform.position.z);

                        // make zombie bigger to see from which zombie player got hit
                        collision.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

                        // set rotation
                        c.transform.rotation = Quaternion.Euler(90, 0, 0);
                    }

                } // End !PlayerSave()

            } // End m_Dead == false

        } // End collision.collider.tag == "Enemy"
    }
    #endregion

}
