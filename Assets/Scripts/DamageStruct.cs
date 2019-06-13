using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct DamageInfo
{
    public EntityHolder player;
    public EntityHolder target;
    public int count;

    public DamageInfo(EntityHolder who, EntityHolder to, int damage)
    {
        player = who;
        target = to;
        count = damage;
    }
}

public struct HealInfo
{
    public EntityHolder player;
    public EntityHolder target;
    public int count;

    public HealInfo(EntityHolder who, EntityHolder to, int heal)
    {
        player = who;
        target = to;
        count = heal;
    }
}

[System.Serializable]
public class DamageEvent : UnityEvent<DamageInfo> { }
[System.Serializable]
public class HealEvent : UnityEvent<HealInfo> { }
[System.Serializable]
public class IntChangeEvent : UnityEvent<int> { }