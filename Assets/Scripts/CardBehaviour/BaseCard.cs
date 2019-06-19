using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCard : MonoBehaviour
{
    public TMPro.TextMeshPro nameText;
    public TMPro.TextMeshPro descriptionText;
    public TMPro.TextMeshPro costText;
    public Renderer mainImage;
    public Renderer backImage;

    private CardHolder m_holder;
    private CardInfo m_cardInfo;
    private CardBehaviour m_cardBehaviour;
    
    public CardHolder holder {
        get { return m_holder; }
    }

    public CardInfo info {
        get { return m_cardInfo; }
        set {
            m_cardInfo = value;
            UpdateInfo();
        }
    }

    public void Awake()
    {
        m_holder = GetComponentInParent<CardHolder>();
    }

    protected virtual void Resolve()
    {
        m_cardBehaviour.Resolve();
    }

    public virtual void CheckForResolve()
    {
        bool canResolve = true;
        if (!m_cardBehaviour.canResolve)
        {
            canResolve = false;
        }

        if (canResolve)
        {
            Resolve();
        }
    }

    public virtual bool Cast()
    {
        if (m_cardBehaviour.CanCast())
        {
            m_cardBehaviour.Cast();
            return true;
        }
        return false;
    }

    public virtual void CancelCast()
    {
        m_cardBehaviour.CancelCast();
    }

    public void UpdateInfo()
    {
        nameText.text = info.displayName;
        descriptionText.text = info.description;
        costText.text = info.cost.ToString();

        if (m_cardBehaviour) Destroy(m_cardBehaviour);
        System.Type behviourType = System.Type.GetType(m_cardInfo.cardBehaviourName);
        m_cardBehaviour = gameObject.AddComponent(behviourType) as CardBehaviour;
    }
}
