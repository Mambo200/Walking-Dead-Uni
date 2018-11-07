using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [HideInInspector]
    /// <summary>if player collides with Zombie -> Dead = true</summary>
    public static bool m_Dead = false;

    [Tooltip("player walk speed")]
    /// <summary>player walk speed</summary>
    public float m_Speed;
    [Tooltip("player rotation speed")]
    /// <summary>player rotation speed</summary>
    public float m_RotSpeed = 1;
    /// <summary>horizontal input</summary>
    private float m_Horizontal;
    /// <summary>vertical input</summary>
    private float m_Vertical;
    /// <summary>Mouse X input</summary>
    private float m_RotationY;
    /// <summary>when Player is in Save Zone m_Save is true</summary>
    public static bool m_Save = false;

    private CharacterController m_CharacterController;

    // Use this for initialization
    void Start ()
    {
        m_CharacterController = this.gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
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

    // ------------- \\

    /// <summary>
    /// Move Character
    /// </summary>
    void Move()
    {
        Move(m_Horizontal, m_Vertical);
    }

    /// <summary>
    /// Move Character
    /// </summary>
    /// <param name="_horizontal">horizontal input</param>
    /// <param name="_vertical">vertical input</param>
    void Move(float _horizontal, float _vertical)
    {
        Vector3 v = new Vector3();
        float speed = m_Speed * Time.deltaTime;
        // set vector
        v.x = _horizontal;
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

    void Rotate()
    {
        float y = this.transform.eulerAngles.y + (m_RotationY * m_RotSpeed);
        this.gameObject.transform.eulerAngles = new Vector3(
            gameObject.transform.eulerAngles.x,
            y,
            gameObject.transform.eulerAngles.z
            );

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            m_Save = true;
        }
        else if (other.gameObject.tag == "SaveZone")
        {
            m_Save = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SaveZone")
        {
            m_Save = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SaveZone")
        {
            m_Save = true;
        }
    }
}
