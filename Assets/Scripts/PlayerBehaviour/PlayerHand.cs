using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform handTransform;
    public float handWidth = 1.0F;
    public float handAngle = 35.0F;
    public AnimationCurve handCurve;

    private PlayerHolder m_player;
    private List<CardHolder> m_cards = new List<CardHolder>();
    private RaycastHit[] hits = new RaycastHit[0];
    private GameObject m_closestHit;
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

    public GameObject closestHit {
        get {
            return m_closestHit;
        }
        set {
            if (m_closestHit != value)
            {
                m_closestHit = value;
                CardHolder cardHolder = closestHit.GetComponent<CardHolder>();
                if (cardHolder)
                {
                    foreach (var card in m_cards)
                    {
                        card.transform.localScale = card.initialScale;
                    }
                    cardHolder.transform.localScale = cardHolder.initialScale * 1.4F;
                }
                UpdateCardPositions();
            }
        }
    }

    private void Update()
    {
        if (hits.Length <= 0)
        {
            foreach (var card in m_cards)
            {
                card.transform.localScale = card.initialScale;
            }
            closestHit = gameObject;
        }
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if ((hits = Physics.RaycastAll(mouseRay)).Length > 0)
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
                        closestHit = hit.collider.gameObject;
                        minDistance = distance;
                    }
                }
            }
            Debug.Log(minDistance);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            AddCard(cardInfo);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            RemoveCard(Random.Range(0, m_cards.Count));
        }
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
                float cardWidth = card.transform.localScale.x * 0.9F * constrict;
                position += new Vector3(cardWidth * 0.5F, 0, -0.001F);
                float t = Mathf.Abs(position.x - handTransform.position.x - handWidth * 0.5F) / handWidth;
                float rotation = Mathf.LerpAngle(-handAngle, handAngle, t);
                t = handCurve.Evaluate(t);
                position.y = handTransform.position.y + Mathf.Lerp(card.transform.localScale.y * 0.3F, 0, t);
                card.UpdatePath(position, Quaternion.AngleAxis(rotation, handTransform.forward), "FadeInFadeOut");
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
