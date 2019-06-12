using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CastInfo
{
    public float value;
    public int cardIndex;
    public PlayerHolder player;
    public PlayerHolder target;
}

public class CardBehaviour : ScriptableObject
{
    public virtual void Cast(CastInfo info) { }
}

[CreateAssetMenu(fileName = "CardInfo", menuName = "Cards/Card Info")]
public class CardInfo : ScriptableObject
{
    public CardBehaviour[] cardBehaviours;
    public string displayName = "New Card";
    [TextArea]
    public string description;
    public Texture2D backCardImage;
    public int cost;
}
