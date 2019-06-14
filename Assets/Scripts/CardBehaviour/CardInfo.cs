using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : ScriptableObject
{
    public virtual bool Cast(CardHolder card) { return false; }
    public virtual void Resolve() { }
    public virtual bool canResolve {
        get { return false; }
    }
}

[CreateAssetMenu(fileName = "CardInfo", menuName = "Cards/Card Info")]
public class CardInfo : ScriptableObject
{
    public CardBehaviour cardBehaviour;
    public string displayName = "New Card";

    [TextArea]
    public string description;
    public Texture2D backCardImage;
    public int cost;
}
