using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHand), typeof(PlayerCardSelection))]
public class PlayerHolder : EntityHolder
{
    private PlayerHand m_hand;
    private PlayerCardSelection m_cardSelection;
    private int m_mana;
    [SerializeField]
    private int m_maxMana;

    public IntChangeEvent OnChangeMana = new IntChangeEvent();

    public int maxMana {
        get {
            return m_maxMana;
        }
    }

    public int mana {
        get { return m_mana; }
        set 
        {
            m_mana = value;
            OnChangeMana.Invoke(m_mana);
        }
    }

    [Header("Remove")]
    public CardInfo[] cardsInfo;

    protected override void Awake()
    {
        base.Awake();
        m_hand = GetComponent<PlayerHand>();
        m_cardSelection = GetComponent<PlayerCardSelection>();
        mana = m_maxMana;
    }

    private void Start()
    {
        AddHand();
    }

    public void AddHand()
    {
        for (int i = 0; i < 5; i++)
        {
            hand.AddCard(cardsInfo[Random.Range(0, cardsInfo.Length)]);
        }
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
        }
    }

    public PlayerHand hand {
        get { return m_hand; }
    }

    public PlayerCardSelection cardSelection {
        get { return m_cardSelection; }
    }
}
