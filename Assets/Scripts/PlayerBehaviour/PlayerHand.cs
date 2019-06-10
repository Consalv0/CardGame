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
    private RaycastHit[] hits = new RaycastHit[0];
    private CardHolder m_selectedCard;
    private Ray mouseRay;

    [Header("Remove")]
    public CardInfo cardInfo;
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

        if (Input.GetButtonDown("Fire1"))
        {
            AddCard(cardInfo);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            RemoveCard(Random.Range(0, m_cards.Count));
        }
    }

    private void ComputeClosestCardFromMouse()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(mouseRay);

        if (hits.Length <= 0)
        {
            selectedCard = null;
        }
        if (hits.Length > 0)
        {
            bool mouseStillOverSelectedCard = false;
            foreach (var hit in hits)
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
                foreach (var hit in hits)
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
    }

    private void ResetCardScales()
    {
        foreach (var card in m_cards)
        {
            card.transform.localScale = cardScale;
        }
        UpdateCardPositions();
    }

    private void IncreaseCardScale(CardHolder card)
    {
        card.transform.localScale = cardScale * 1.4F;
        UpdateCardPositions();
    }

    public void RemoveCard(int index)
    {
        if (index >= m_cards.Count || index < 0)
            return;

        Destroy(m_cards[index].gameObject);
        m_cards.RemoveAt(index);

        UpdateCardPositions();
    }

    public void AddCard(CardInfo info)
    {
        GameObject card = Instantiate(baseCard, handTransform);
        CardHolder cardHolder = card.GetComponent<CardHolder>();
        cardHolder.player = player;
        m_cards.Add(cardHolder);
        cardHolder.card.info = info;

        UpdateCardPositions();
    }

    public void UpdateCardPositions()
    {
        int cardCount = m_cards.Count;
        if (cardCount <= 0) return;

        float totalCardWidth = 0;
        foreach (var card in m_cards)
        {
            totalCardWidth += card.transform.localScale.x * 0.9F;
        }
        float constrict = handWidth / totalCardWidth;

        if (cardCount <= 1000)
        {
            Vector3 position = handTransform.position - new Vector3(totalCardWidth * 0.5F * constrict, 0, 0);
            foreach (var card in m_cards)
            {
                bool isSelected = card == selectedCard;
                float cardWidth = card.transform.localScale.x * 0.9F * constrict;

                position += new Vector3(cardWidth * 0.5F, 0, -0.001F);
                float t = Mathf.Abs(position.x - handTransform.position.x - handWidth * 0.5F) / handWidth;
                float rotation = Mathf.LerpAngle(-handAngle, handAngle, t);
                t = handCurve.Evaluate(t);
                position.y = handTransform.position.y + Mathf.Lerp(card.transform.localScale.y * 0.3F, 0, t) +
                    (isSelected ? card.transform.localScale.y * 0.15F : 0);
                card.UpdatePath(
                    position + (isSelected ? handTransform.forward * cardCount * -0.001F : Vector3.zero ),
                    isSelected ? Quaternion.identity : Quaternion.AngleAxis(rotation, handTransform.forward),
                    "FadeInFadeOut"
                );
                position += new Vector3(cardWidth * 0.5F, 0, 0);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (handTransform)
            Gizmos.DrawWireCube(handTransform.position, new Vector3(handWidth, 0.5F, 0));
    }
}
