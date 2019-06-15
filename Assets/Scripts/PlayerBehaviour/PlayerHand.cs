using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform handTransform;
    public float handWidth = 1.0F;
    public float handAngle = 35.0F;
    public Vector3 cardScale = Vector3.one;
    public AnimationCurve handCurve;

    private PlayerHolder m_player;
    private List<CardHolder> m_cards = new List<CardHolder>();
    private RaycastHit[] m_hits = new RaycastHit[0];
    private CardHolder m_hoveredCard;
    private Ray m_mouseRay;

    [Header("Remove")]
    public GameObject baseCard;

    private void Awake()
    {
        m_player = GetComponent<PlayerHolder>();
    }

    public PlayerHolder player {
        get { return m_player; }
    }

    public CardHolder hoveredCard {
        get {
            return m_hoveredCard;
        }
        set {
            if (m_hoveredCard != value)
            {
                m_hoveredCard = value;
                ResetCardScales();
                if (hoveredCard)
                {
                    IncreaseCardScale(hoveredCard);
                }
            }
        }
    }

    private void Update()
    {
        ComputeClosestCardFromMouse();
    }

    public bool ContainsCard(CardHolder cardHolder)
    {
        return m_cards.Contains(cardHolder);
    }

    private void ComputeClosestCardFromMouse()
    {
        m_mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        m_hits = Physics.RaycastAll(m_mouseRay);

        float totalCardWidth = 0;
        foreach (var card in m_cards)
        {
            totalCardWidth += GetCardWidth(card) * 0.9F;
        }
        float handCardWidth = UpdateRelativeHandWidth(totalCardWidth);
        float constrict = handCardWidth / totalCardWidth;

        if (!new Bounds(handTransform.position, new Vector3(handCardWidth, 0.5F, 0))
            .IntersectRay(m_mouseRay))
        {

            if (m_hits.Length <= 0)
            {
                hoveredCard = null;
            }
            if (m_hits.Length > 0)
            {
                bool mouseStillOverSelectedCard = false;
                foreach (var hit in m_hits)
                {
                    CardHolder cardHolder = hit.collider.gameObject.GetComponent<CardHolder>();
                    if (cardHolder)
                    {
                        if (cardHolder == hoveredCard)
                        {
                            mouseStillOverSelectedCard = true;
                            break;
                        }
                    }
                }
                if (!mouseStillOverSelectedCard)
                {
                    float minDistance = Mathf.Infinity;
                    foreach (var hit in m_hits)
                    {
                        CardHolder cardHolder = hit.collider.gameObject.GetComponent<CardHolder>();
                        if (cardHolder)
                        {
                            float distance = Vector3.Distance(hit.point, cardHolder.transform.position);
                            if (distance < minDistance)
                            {
                                hoveredCard = cardHolder;
                                minDistance = distance;
                            }
                        }
                    }
                }
            }
            return;
        }

        if (handTransform)
        {
            Vector3 position = handTransform.position - new Vector3(totalCardWidth * 0.5F * constrict, 0, 0);
            foreach (var card in m_cards)
            {
                float cardWidth = GetCardWidth(card) * 0.9F * constrict;
                
                position += new Vector3(cardWidth * 0.5F, 0, 0);
                if (new Bounds(
                        position,
                        new Vector3(cardWidth, 0.5F, 0)
                    ).IntersectRay(m_mouseRay))
                {
                    hoveredCard = card;
                }
                position += new Vector3(cardWidth * 0.5F, 0, 0);
            }
        }
    }

    private void ResetCardScales()
    {
        foreach (var card in m_cards)
        {
            card.UpdateScale(cardScale, "FastIn", 1.5F);
        }
        UpdateCardPositions(1.5F);
    }

    private void IncreaseCardScale(CardHolder card)
    {
        card.UpdateScale(cardScale * 1.7F, "FastIn", 1.5F);
        UpdateCardPositions(1.5F);
    }

    public void RemoveCard(int index)
    {
        if (index >= m_cards.Count || index < 0)
            return;

        Destroy(m_cards[index].gameObject);
        m_cards.RemoveAt(index);

        UpdateCardPositions(0.8F);
    }

    public void RemoveCard(CardHolder cardHolder)
    {
        int index = m_cards.FindIndex(card => card == cardHolder);
        if (index >= m_cards.Count || index < 0)
            return;

        Destroy(m_cards[index].gameObject);
        m_cards.RemoveAt(index);

        UpdateCardPositions(0.8F);
    }

    public void AddCard(CardInfo info)
    {
        GameObject card = Instantiate(baseCard, handTransform);
        CardHolder cardHolder = card.GetComponent<CardHolder>();
        cardHolder.UpdateScale(cardScale, "FadeInFadeOut", 1.5F);
        cardHolder.player = player;
        m_cards.Add(cardHolder);
        cardHolder.card.info = info;

        UpdateCardPositions(0.8F);
    }

    private float UpdateRelativeHandWidth(float totalCardWidth)
    {
        float handCardWidth = handWidth * Mathf.Clamp01(totalCardWidth / handWidth);
        return handCardWidth;
    }

    public float GetCardWidth(CardHolder cardHolder)
    {
        return cardScale.x * (cardHolder == hoveredCard ? 1.5F : 1.0F);
    }

    public void UpdateCardPositions(float transitionSpeed)
    {
        int cardCount = m_cards.Count;
        if (cardCount <= 0) return;

        float totalCardWidth = 0;
        foreach (var card in m_cards)
        {
            totalCardWidth += GetCardWidth(card) * 0.9F;
        }
        float handCardWidth = UpdateRelativeHandWidth(totalCardWidth);
        float constrict = handCardWidth / totalCardWidth;
        
        Vector3 position = handTransform.position - new Vector3(totalCardWidth * 0.5F * constrict, 0, 0);
        foreach (var card in m_cards)
        {
            bool isSelected = card == hoveredCard;
            float cardWidth = GetCardWidth(card) * 0.9F * constrict;

            position += new Vector3(cardWidth * 0.5F, 0, -0.001F);
            float rotation = Mathf.LerpAngle(-handAngle, handAngle, Mathf.Abs(position.x - handTransform.position.x - handWidth * 0.5F) / handWidth);
            float heightStamp = handCurve.Evaluate(Mathf.Abs(position.x - handTransform.position.x - handCardWidth * 0.5F) / handCardWidth);
            position.y = handTransform.position.y + Mathf.Lerp(GetCardWidth(card) * 0.4F, 0, heightStamp) + 
                (isSelected ? GetCardWidth(card) : 0);
            if (card != player.cardSelection.selectedCard)
            {
                card.UpdatePath(
                    position,
                    isSelected ? Quaternion.identity : Quaternion.AngleAxis(rotation, handTransform.forward),
                    "FadeInFadeOut", transitionSpeed
                );
            }
            position += new Vector3(cardWidth * 0.5F, 0, 0);
        }
    }

    void OnDrawGizmos()
    {
        float totalCardWidth = 0;
        foreach (var card in m_cards)
        {
            totalCardWidth += GetCardWidth(card) * 0.9F;
        }
        float handCardWidth = UpdateRelativeHandWidth(totalCardWidth);
        float constrict = handCardWidth / totalCardWidth;

        if (handTransform)
        {
            Gizmos.DrawWireCube(handTransform.position, new Vector3(handCardWidth, 0.5F, 0));
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(handTransform.position, new Vector3(handWidth, 0.5F, 0));

            Vector3 position = handTransform.position - new Vector3(totalCardWidth * 0.5F * constrict, 0, 0);
            foreach (var card in m_cards)
            {
                bool isSelected = card == hoveredCard;
                float cardWidth = GetCardWidth(card) * 0.9F * constrict;

                position += new Vector3(cardWidth * 0.5F, 0, 0);
                Gizmos.color = 
                    new Bounds(
                        position,
                        new Vector3(cardWidth, 0.5F, 0)
                    ).IntersectRay(Camera.main.ScreenPointToRay(Input.mousePosition)) ? Color.red : Color.cyan;
                Gizmos.DrawWireCube(
                    position,
                    new Vector3(cardWidth, 0.5F)
                );
                position += new Vector3(cardWidth * 0.5F, 0, 0);
            }
        }
    }
}
