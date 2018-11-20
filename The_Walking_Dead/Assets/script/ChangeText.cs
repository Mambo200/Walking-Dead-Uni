using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour {

    /// <summary>Dead Text</summary>
    public static string TextDead {get { return ("YOU ARE DEAD"); } }
    /// <summary>Win Text</summary>
    public static string TextWin {get { return ("YOU REACHED THE FINISH LINE"); } }
    /// <summary>Property of WinDeadText</summary>
    public static GameObject WinDeadText {get { return m_WinDeadText; } }
    /// <summary>Property of WinDeadText</summary>
    public static GameObject LivesText { get { return m_LivesText; } }

    /// <summary>Gameobject of WinDeadText</summary>
    private static GameObject m_WinDeadText;
    /// <summary>Gameobject of LivesText</summary>
    private static GameObject m_LivesText;
    /// <summary>The color of LivesTextbox when 0 or more than 0</summary>
    private static Color[] m_LiveTextColor = new Color[2];

    // Use this for initialization
    void Start()
    {
        // get Text Gameobjects
        m_WinDeadText = GameObject.Find("WinDieText");
        m_LivesText = GameObject.Find("LivesText");

        // Set default Text Color Text
        WinDeadText.GetComponent<Text>().color = Color.white;
        LivesText.GetComponent<Text>().color = m_LiveTextColor[0];

        // set static color
        m_LiveTextColor[0] = new Color(255, 0, 0);    // 0 lives color
        m_LiveTextColor[1] = new Color(71, 99, 255);  // above 0 lives color
    }

    /// <summary>
    /// Change Text of Textbox WinDeadText
    /// </summary>
    /// <param name="_text">Text</param>
    public static void ChangeTextBoxWinDead(string _text, Color _color)
    {
        WinDeadText.GetComponent<Text>().color = _color;
        WinDeadText.GetComponent<Text>().text = _text + "\nPress R to reload";
    }

    /// <summary>
    /// Change Text of Textbox Lives
    /// </summary>
    /// <param name="_lives">The lives.</param>
    public static void ChangeTextBoxLives(int _lives)
    {
        // set new Text
        LivesText.GetComponent<Text>().text = "Lives: " + _lives;

        // more than 0 lives
        if(_lives > 0)
        {
            Debug.Log("Above 0 Lives");
            LivesText.GetComponent<Text>().CrossFadeColor(m_LiveTextColor[1], 2f, false, false);
        }
        // 0 lives, next hit -> dead
        else
        {
            LivesText.GetComponent<Text>().CrossFadeColor(m_LiveTextColor[0], 2f, false, false);
        }
    }

    /// <summary>
    /// Activate or deactivate Textbox.
    /// </summary>
    /// <param name="_active">if set to <c>true</c> [active].</param>
    public static void ChangeTextBoxLives(bool _active)
    {
        LivesText.SetActive(_active);
    }
}
