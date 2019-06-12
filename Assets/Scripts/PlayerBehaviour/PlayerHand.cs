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
    private CardHolder m_selectedCard;
    private Ray m_mouseRay;

    [Header("Remove")]
    public CardInfo[] cardsInfo;
    public GameObject baseCard;

    private void Awake()
    {
        m_player = GetComponent<PlayerHolder>();
    }

    public PlayerHolder player {
        get { return m_player; }
    }

    public CardHolder selectedCard {
        get {
            return m_selectedCard;
        }
        set {
            if (m_selectedCard != value)
            {
                m_selectedCard = value;
                ResetCardScales();
                if (selectedCard)
                {
                    IncreaseCardScale(selectedCard);
                }
            }
        }
    }

    private void Update()
    {
        ComputeClosestCardFromMouse();

        if (Input.GetButtonDown("Jump"))
        {
            AddCard(cardsInfo[Random.Range(0, cardsInfo.Length)]);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (selectedCard)
            {
                m_cards.Find(card => card == selectedCard);
                CastInfo castInfo = new CastInfo();
                castInfo.player = player;
                castInfo.target = player;
                castInfo.cardIndex = m_cards.FindIndex(card => card == selectedCard);
                selectedCard.card.Cast(castInfo);
            }
        }
    }

    private void ComputeClosestCardFromMouse()
    {
        m_mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        m_hits = Physics.RaycastAll(m_mouseRay);

        float totalCardWidth = 0;
        foreach (var card in m_cards)
        {
            totalCardWidth += card.transform.localScale.x * 0.9F;
        }
        float handCardWidth = UpdateRelativeHandWidth(totalCardWidth);
        float constrict = handCardWidth / totalCardWidth;

        if (!new Bounds(handTransform.position, new Vector3(handCardWidth, 0.5F, 0))
            .IntersectRay(m_mouseRay))
        {

            if (m_hits.Length <= 0)
            {
                selectedCard = null;
            }
            if (m_hits.Length > 0)
            {
                bool mouseStillOverSelectedCard = false;
                foreach (var hit in m_hits)
                {
                    CardHolder cardHolder = hit.collider.gameObject.GetComponent<CardHolder>();
                    if (cardHolder)
                    {
                        if (cardHolder == selectedCard)
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
                                selectedCard = cardHolder;
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
                float cardWidth = card.transform.localScale.x * 0.9F * constrict;

                position += new Vector3(cardWidth * 0.5F, 0, 0);
                if (new Bounds(
                        position,
                        new Vector3(cardWidth, 0.5F, 0)
                    ).IntersectRay(m_mouseRay))
                {
                    selectedCard = card;
                }
                position += new Vector3(cardWidth * 0.5F, 0, 0);
            }
        }
    }

    private void ResetCardScales()
    {
        foreach (var card in m_cards)
        {
            card.transform.localScale = cardScale;
        }
        UpdateCardPositions(1.5F);
    }

    private void IncreaseCardScale(CardHolder card)
    {
        card.transform.localScale = cardScale * 1.4F;
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

    public void AddCard(CardInfo info)
    {
        GameObject card = Instantiate(baseCard, handTransform);
        card.transform.localScale = cardScale;
        CardHolder cardHolder = card.GetComponent<CardHolder>();
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

    public void UpdateCardPositions(float transitionSpeed)
    {
        int cardCount = m_cards.Count;
        if (cardCount <= 0) return;

        float totalCardWidth = 0;
        foreach (var card in m_cards)
        {
            totalCardWidth += card.transform.localScale.x * 0.9F;
        }
        float handCardWidth = UpdateRelativeHandWidth(totalCardWidth);
        float constrict = handCardWidth / totalCardWidth;
        
        Vector3 position = handTransform.position - new Vector3(totalCardWidth * 0.5F * constrict, 0, 0);
        foreach (var card in m_cards)
        {
            bool isSelected = card == selectedCard;
            float cardWidth = card.transform.localScale.x * 0.9F * constrict;

            position += new Vector3(cardWidth * 0.5F, 0, -0.001F);
            float t = Mathf.Abs(position.x - handTransform.position.x - handCardWidth * 0.5F) / handCardWidth;
            float rotation = Mathf.LerpAngle(-handAngle, handAngle, t);
            t = handCurve.Evaluate(t);
            position.y = handTransform.position.y + Mathf.Lerp(card.transform.localScale.y * 0.3F, 0, t) + 
                (isSelected ? card.transform.localScale.y * 0.8F : 0);
            card.UpdatePath(
                position,
                isSelected ? Quaternion.identity : Quaternion.AngleAxis(rotation, handTransform.forward),
                "FadeInFadeOut", transitionSpeed
            );
            position += new Vector3(cardWidth * 0.5F, 0, 0);
        }
    }

    void OnDrawGizmos()
    {
        float totalCardWidth = 0;
        foreach (var card in m_cards)
        {
            totalCardWidth += card.transform.localScale.x * 0.9F;
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
                bool isSelected = card == selectedCard;
                float cardWidth = card.transform.localScale.x * 0.9F * constrict;

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
