using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Behaviour", menuName = "Cards/Behaviours/Damage")]
public class DamageCardBehaviour : CardBehaviour
{
    public struct CastInfo {
        public CardHolder card;
        public PlayerHolder holder;
        public EntityHolder target;
    }

    public int damage;
    private CastInfo castInfo;

    public override void Cast(CardHolder card)
    {
        base.Cast(card);
        castInfo = new CastInfo();
        castInfo.card = card;
        castInfo.holder = card.player;

        SelectEntity selection = new SelectEntity();
        selection.OnClick = OnClick;
        selection.ActivateEvent();
    }

    private void OnClick(EntityHolder entity)
    {
        if (entity)
        {
            castInfo.target = entity;
            Invoke(castInfo);
        }
    }

    private void Invoke(CastInfo info)
    {
        DamageInfo damageInfo = new DamageInfo(info.holder, info.target, damage);
        info.target.stats.DoDamage(damageInfo);
        info.card.player.hand.RemoveCard(info.card);
    }
}
