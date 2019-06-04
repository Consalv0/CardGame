using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    private PlayerStats m_stats;

    private void Awake()
    {
        m_stats = GetComponent<PlayerStats>();
    }

    public PlayerStats stats {
        get { return m_stats; }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            DamageInfo damage;
            damage.count = Random.Range(0, m_stats.maxHealth / 3);
            damage.target = this;
            damage.player = this;

            m_stats.DoDamage(damage);
        }

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
