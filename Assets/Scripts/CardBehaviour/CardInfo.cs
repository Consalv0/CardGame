using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : ScriptableObject
{
    public virtual void Cast(CardHolder card) { }
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
