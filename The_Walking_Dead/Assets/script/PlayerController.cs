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


    // Use this for initialization
    void Start ()
    {
		
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
        float speed = m_Speed * Time.deltaTime;
        this.gameObject.transform.Translate(
            new Vector3(
                _horizontal * speed * Run(),
                0,
                _vertical * speed * Run())
                );
    }

    /// <summary>
    /// Run Speed
    /// </summary>
    /// <returns>run value</returns>
    float Run()
    {
        /// <summary>if left shift is pressed = 1, if not = 0</summary>
        float run = Input.GetAxis("Fire3");

        if (run > 0)
            return run * 1.5f;
        else
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
}
