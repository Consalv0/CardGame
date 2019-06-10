using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCard : MonoBehaviour
{
    public TMPro.TextMeshPro nameText;
    public TMPro.TextMeshPro descriptionText;
    public TMPro.TextMeshPro costText;

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

    public void UpdateInfo()
    {
        nameText.text = info.displayName;
        descriptionText.text = info.description;
        costText.text = info.cost.ToString();
    }
}
