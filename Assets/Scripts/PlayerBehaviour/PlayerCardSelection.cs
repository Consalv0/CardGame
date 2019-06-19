using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardSelection : MonoBehaviour
{
    public Transform cardAnchor;

    private PlayerHolder m_player;
    private CardHolder m_selectedCard;

    public PlayerHolder player {
        get { return m_player; }
    }

    public CardHolder selectedCard {
        get { return m_selectedCard; }
    }

    private void Awake()
    {
        m_player = GetComponent<PlayerHolder>();
    }

    public void CancelSelection()
    {
        m_selectedCard.card.CancelCast();
        m_selectedCard = null;
        player.hand.UpdateCardPositions(0.65F);
    }

    public bool SelectCard(CardHolder cardHolder)
    {
        if (cardHolder == null) return false;
        if (!cardHolder.card.Cast()) return false;
        if (!player.hand.ContainsCard(cardHolder)) return false;

        m_selectedCard = cardHolder;
        m_selectedCard.transform.Rotate(Vector3.right, 25.0F);
        m_selectedCard.UpdatePath(
            new Vector3(cardAnchor.position.x, cardAnchor.position.y, m_selectedCard.transform.position.z),
            cardAnchor.rotation, "FadeInFadeOut"
        );
        return true;
    }
}
