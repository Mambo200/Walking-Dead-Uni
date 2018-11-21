using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_KeyBringer : MonoBehaviour {

    /// <summary>The reward of this NPC</summary>
    public GameObject m_Reward;

    /// <summary>how many Food player need to buy Item</summary>
    private int m_RewardCost = 1;

    // Use this for initialization
    void Start ()
    {
        m_Reward.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        // if other is not player return
        if (other.gameObject.tag != "Player")
            return;

        // if player has enough coins spawn 1up and despawn NPC
        if (PlayerController.Food >= m_RewardCost)
        {
            PlayerController.Food = PlayerController.Food - m_RewardCost;
            this.gameObject.SetActive(false);
            m_Reward.SetActive(true);

        }
    }

}
