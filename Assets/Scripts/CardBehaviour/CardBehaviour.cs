using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public struct CardBehaviourProperties
{
    [SerializeField]
    private int m_intValue;
    [SerializeField]
    private int m_floatValue;

    public int intValue {
        get { return m_intValue; }
    }
    public float floatValue {
        get { return m_floatValue; }
    }
}

[DisallowMultipleComponent]
public class CardBehaviour : MonoBehaviour
{
    protected CardHolder cardHolder;

    public CardBehaviourProperties cardBehaviourProperties {
        get { return cardHolder.card.info.cardBehaviourProperties; }
    }

    public virtual bool canResolve {
        get { return false; }
    }

    private void Awake()
    {
        cardHolder = GetComponent<CardHolder>();
    }

    public virtual void Cast() { }
    public virtual bool CanCast() { return false; }
    public virtual void CancelCast() {  }
    public virtual void Resolve() { }
}