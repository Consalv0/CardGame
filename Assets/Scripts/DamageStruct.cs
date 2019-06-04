using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct DamageInfo
{
    public PlayerHolder player;
    public PlayerHolder target;
    public int count;
}

public struct HealInfo
{
    public PlayerHolder player;
    public PlayerHolder target;
    public int count;
}

[System.Serializable]
public class DamageEvent : UnityEvent<DamageInfo> { }
[System.Serializable]
public class HealEvent : UnityEvent<HealInfo> { }
[System.Serializable]
public class IntChangeEvent : UnityEvent<int> { }