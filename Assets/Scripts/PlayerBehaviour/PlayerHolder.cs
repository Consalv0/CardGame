using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    private PlayerStats m_stats;
    private PlayerHand m_hand;

    private void Awake()
    {
        m_stats = GetComponent<PlayerStats>();
        m_hand = GetComponent<PlayerHand>();
    }

    public PlayerStats stats {
        get { return m_stats; }
    }

    public PlayerHand hand {
        get { return m_hand; }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            HealInfo heal;
            heal.count = Random.Range(0, m_stats.maxHealth / 3);
            heal.target = this;
            heal.player = this;

            m_stats.DoHeal(heal);
        }
    }
}
