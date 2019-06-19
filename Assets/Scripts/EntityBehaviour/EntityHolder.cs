using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHolder : MonoBehaviour
{
    protected EntityStats m_stats;
    
    protected virtual void Awake()
    {
        m_stats = GetComponent<EntityStats>();
    }

    public EntityStats stats {
        get { return m_stats; }
    }

    public bool isDeath()
    {
        if (stats.GetHealth() <=0 )
        {
            return true;
        }
        return false;
    }
}
