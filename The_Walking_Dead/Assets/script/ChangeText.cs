using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour {

    /// <summary>Dead Text</summary>
    public static string TextDead {get { return ("YOU ARE DEAD"); } }
    /// <summary>Win Text</summary>
    public static string TextWin {get { return ("YOU REACHED THE FINISH LINE"); } }
    /// <summary>Gameobject of Text</summary>
    public static GameObject WinDeadText {get { return GameObject.Find("WinDieText"); } }

	// Use this for initialization
	void Start ()
    {
        WinDeadText.GetComponent<Text>().color = Color.white;
    }

    /// <summary>
    /// Change Text of Textbox
    /// </summary>
    /// <param name="_text">Text</param>
    public static void ChangeTextBox(string _text, Color _color)
    {
        WinDeadText.GetComponent<Text>().color = _color;
        WinDeadText.GetComponent<Text>().text = _text + "\nPress R to reload";
    }
}
