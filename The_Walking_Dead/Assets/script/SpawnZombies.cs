using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour {

    [Tooltip("true -> Zombies will spawn")]
    ///<summary>Zombies spawn switch</summary>
    public bool m_SpawnZombies = true;
    [Tooltip("Emeny Prefab")]
    ///<summary> Zombie Gameobject </summary>
    public GameObject m_Zombie;

    ///<summary>Time bygone</summary>
    private float m_TimeGone;


	// Use this for initialization
	void Start ()
    {
        if (m_SpawnZombies)
        {
            // spawn enemies
            for (int x = 0; x < 8; x ++)
            {
                for (int z = 0; z < 7; z ++)
                {
                    Instantiate(
                        m_Zombie,
                        new Vector3(
                            Random.Range(-90,90),
                            2f,
                            Random.Range(-60, 90)
                            ),
                        new Quaternion(),
                        this.transform);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_SpawnZombies || PlayerController.GameOver())
            return;
        //copy delta time value to variable
        float deltaTime = Time.deltaTime;
        // add deltatime
        m_TimeGone += deltaTime;

        // when 2 seconds are gone and  game does not jerk (ruckeln) to much
        if (m_TimeGone >= 1f && deltaTime < 0.02f)
        {
            // spawn enemy at random position
            Instantiate
            (
                m_Zombie,
                new Vector3
                    (
                    Random.Range(-90, 90),
                    3,
                    Random.Range(-90,90)
                    ), 
                new Quaternion(),
                this.transform
            );

            // set Time Gone back to 0
            m_TimeGone = 0;
        }
	}
    
    // ------------------------------------------------------------------------------------------- \\

}
