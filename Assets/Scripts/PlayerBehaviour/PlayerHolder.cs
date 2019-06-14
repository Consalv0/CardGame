using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHand), typeof(PlayerCardSelection))]
public class PlayerHolder : EntityHolder
{
    private PlayerHand m_hand;
    private PlayerCardSelection m_cardSelection;

    [Header("Remove")]
    public CardInfo[] cardsInfo;

    protected override void Awake()
    {
        base.Awake();
        m_hand = GetComponent<PlayerHand>();
        m_cardSelection = GetComponent<PlayerCardSelection>();
    }
    
    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            hand.AddCard(cardsInfo[Random.Range(0, cardsInfo.Length)]);
        }
        if (Input.GetButtonDown("Fire1") && DungeonTable.instance.isWaiting)
        {
            cardSelection.SelectCard(hand.hoveredCard);
            cardSelection.CastCard();
        }
    }

    public PlayerHand hand {
        get { return m_hand; }
    }

    public PlayerCardSelection cardSelection {
        get { return m_cardSelection; }
    }
}
