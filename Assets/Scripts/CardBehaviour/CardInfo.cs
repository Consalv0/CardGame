using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Spell, Instant, Weapon
}

[CreateAssetMenu(fileName = "CardInfo", menuName = "Cards/Card Info")]
public class CardInfo : ScriptableObject
{
    public CardType cardType;
    public string displayName = "New Card";
    [TextArea]
    public string description;
    public Texture2D backCardImage;
    public int cost;
}
