using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTombStones : MonoBehaviour {

    [Tooltip("true -> Tombstones will spawn")]
    ///<summary>Tombstone spawn switch</summary>
    public bool m_SpawnTombStone = true;
    [Tooltip("Tombstone Prefab")]
    ///<summary> Tombstone Gameobject </summary>
    public GameObject m_TombStone;

    // Use this for initialization
    void Start ()
    {
        // if spawntombstones is false cancel spawning
        if (!m_SpawnTombStone)
            return;

        // spawn Tombstones
        for (int x = 0; x < 4; x++)
        {
            for (int z = 0; z < 4; z++)
            {
                Instantiate(
                    m_TombStone,
                    new Vector3(
                        Random.Range(-90, 90),
                        0.2f,
                        Random.Range(-60, 90)
                        ),
                    Quaternion.Euler(
                        0,
                        Random.Range(0, 90),
                        0
                        ),
                    //new Quaternion(),
                    this.transform);
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
