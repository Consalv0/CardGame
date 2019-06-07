using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerHolder))]
public class PlayerStats : MonoBehaviour
{
    public DamageEvent OnDamage;
    public HealEvent OnHeal;
    public IntChangeEvent OnChangeHealth;

    [SerializeField]
    private int m_maxHealth = 100;
    private PlayerHolder m_player;
    private int m_health;

    public int maxHealth 
    {
        get { return m_maxHealth; }
        set {
            m_maxHealth = value;
            OnChangeHealth.Invoke(m_health);
        }
    }

    private void Awake()
    {
        m_player = GetComponent<PlayerHolder>();
        m_health = maxHealth;
    }

    public PlayerHolder player
    {
        get { return m_player; }
    }

    public void DoDamage(DamageInfo info)
    {
        m_health -= info.count;
        m_health = Mathf.Clamp(m_health, 0, maxHealth);
        OnDamage.Invoke(info);
    }

    public void DoHeal(HealInfo info)
    {
        m_health += info.count;
        m_health = Mathf.Clamp(m_health, 0, maxHealth);
        OnHeal.Invoke(info);
    }

    public void SetHeath(int count)
    {
        if (m_health != count)
        {
            m_health = count;
            m_health = Mathf.Clamp(m_health, 0, maxHealth);
            OnChangeHealth.Invoke(m_health);
        }
    }

    public int GetHealth()
    {
        return m_health;
    }
}
