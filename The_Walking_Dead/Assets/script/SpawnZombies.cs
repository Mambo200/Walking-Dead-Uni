using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour {

    [Tooltip("How many Zombies spawn in total. Default = 40")]
    /// <summary>total number of zombies spawning</summary>
    public int m_ZombieAmount = 40;
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
            for (int x = 0; x < m_ZombieAmount; x++)
            {
                SpawnZombie();
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
            SpawnZombie();

            // set Time Gone back to 0
            m_TimeGone = 0;
        }
	}
    
    // ------------------------------------------------------------------------------------------- \\

    private void SpawnZombie()
    {
        Instantiate(
                    m_Zombie,                   // prefab
                    new Vector3(                // Position
                        Random.Range(-90, 90),      // x position
                        2f,                         // y position
                        Random.Range(-60, 90)       // z position
                        ),
                    new Quaternion(),           // Rotation
                    this.transform);            // parent
    }
}
