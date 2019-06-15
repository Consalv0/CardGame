using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Cards/Card Info")]
public class CardInfo : ScriptableObject
{
    [HideInInspector]
    public string cardBehaviourName;
    public CardBehaviourProperties cardBehaviourProperties;
    public string displayName = "New Card";

    [TextArea]
    public string description;
    public Texture2D backCardImage;
    public int cost;
}
