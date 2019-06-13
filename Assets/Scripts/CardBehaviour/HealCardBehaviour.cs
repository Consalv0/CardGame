using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Behaviour", menuName = "Cards/Behaviours/Heal Card")]
public class HealCardBehaviour : CardBehaviour
{
    public struct CastInfo
    {
        public CardHolder card;
        public PlayerHolder holder;
        public EntityHolder target;
    }

    public int heal;

    public override void Cast(CardHolder card)
    {
        base.Cast(card);
        CastInfo castInfo = new CastInfo();
        castInfo.card = card;
        castInfo.holder = card.player;
        castInfo.target = card.player;

        Invoke(castInfo);
    }

    private void Invoke(CastInfo info)
    {
        HealInfo healInfo = new HealInfo(info.holder, info.target, heal);
        info.target.stats.DoHeal(healInfo);
        info.card.player.hand.RemoveCard(info.card);
    }
}
