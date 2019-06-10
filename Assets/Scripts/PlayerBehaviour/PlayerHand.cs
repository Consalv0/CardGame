using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform handTransform;
    public float handWidth = 1.0F;

    private PlayerHolder m_player;
    private List<CardHolder> m_cards = new List<CardHolder>();

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

    private void Update()
    {
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
                float rotation = Mathf.LerpAngle(-45, 45, Mathf.Abs(position.x - handTransform.position.x - handWidth * 0.5F) / handWidth);
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
