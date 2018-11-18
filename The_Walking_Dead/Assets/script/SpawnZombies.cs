using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour {

    [Tooltip("How many Zombies spawn at start. Default = 40")]
    /// <summary>start number of zombies spawning</summary>
    public int m_ZombieStartAmount = 40;
    [Tooltip("How many Zombies can spawn in total. Default = 100")]
    /// <summary>Max number of zombies in game</summary>
    public int m_ZombieMaxAmount = 100;
    [Tooltip("Emeny Prefab")]
    ///<summary> Zombie Gameobject </summary>
    public GameObject m_Zombie;
    [Range(0, 1000)]
    [Tooltip("if Seed is 0 dont set new seed")]
    /// <summary>Monster Spawn Seed</summary>
    public int m_Seed;

    [Tooltip("Time Spawn Range, x: min / y: max")]
    ///<summary>Spawn time range (x: min - y: max)</summary>
    public Vector2 m_TimeSpawnRange;
    ///<summary>Time bygone</summary>
    private float m_TimeGone;
    ///<summary>Time which it needs to spawn a new Zombie</summary>
    private float m_TimeToSpawn;

    [Header("Debug")]
    [Tooltip("true -> Zombies will spawn")]
    ///<summary>Zombies spawn switch</summary>
    public bool m_SpawnZombies = true;


    // Use this for initialization
    void Start ()
    {
        // if seed it not 0 set new seed, else do nothing
        if (m_Seed != 0)
        {
            // set seed
            Random.InitState(m_Seed);
        }

        if (m_SpawnZombies)
        {
            // spawn enemies
            for (int x = 0; x < m_ZombieStartAmount; x++)
            {
                SpawnZombie();
            }
        }

        // set zombie spawn time
        m_TimeToSpawn = Random.Range(m_TimeSpawnRange.x, m_TimeSpawnRange.y);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if spawn zombies is deactivated or player is dead/has won return
        if (!m_SpawnZombies || PlayerController.GameOver())
            return;


        // if max number of Zombies in Scene dont spawn
        if (m_ZombieMaxAmount > ZombieBehaviour.ZombieCount)
        {
            //copy delta time value to variable
            float deltaTime = Time.deltaTime;
            // add deltatime
            m_TimeGone += deltaTime;

            // when 2 seconds are gone and  game does not jerk (ruckeln) to much
            if (m_TimeGone >= m_TimeToSpawn && deltaTime < 0.02f)
            {
                // spawn enemy at random position
                SpawnZombie();

                // set Time Gone back to 0
                m_TimeGone = 0;

                // set new spawn time range
                m_TimeToSpawn = Random.Range(m_TimeSpawnRange.x, m_TimeSpawnRange.y);
            }
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
