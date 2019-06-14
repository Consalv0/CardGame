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
    private CardInfo m_info;
    
    public CardHolder holder {
        get { return m_holder; }
    }

    public CardInfo info {
        get { return m_info; }
        set {
            m_info = value;
            UpdateInfo();
        }
    }

    private void Awake()
    {
        m_holder = GetComponentInParent<CardHolder>();
    }

    protected virtual void Resolve()
    {
        m_info.cardBehaviour.Resolve();
    }

    public virtual void CheckForResolve()
    {
        bool canResolve = true;
        if (!m_info.cardBehaviour.canResolve)
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
        if (!m_info.cardBehaviour.Cast(holder))
        {
            return false;
        }
        return true;
    }

    public void UpdateInfo()
    {
        nameText.text = info.displayName;
        descriptionText.text = info.description;
        costText.text = info.cost.ToString();
    }
}
