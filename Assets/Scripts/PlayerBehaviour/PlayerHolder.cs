using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : EntityHolder
{
    private PlayerHand m_hand;

    protected override void Awake()
    {
        base.Awake();
        m_hand = GetComponent<PlayerHand>();
    }

    public PlayerHand hand {
        get { return m_hand; }
    }
}
