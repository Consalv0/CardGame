using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCardBehaviour : CardBehaviour
{
    public struct CastInfo
    {
        public PlayerHolder holder;
        public EntityHolder target;
    }

    private CastInfo castInfo;

    public int heal {
        get { return cardBehaviourProperties.intValue; }
    }

    public override bool canResolve {
        get { return castInfo.target != null; }
    }

    public override bool Cast()
    {
        castInfo = new CastInfo();
        castInfo.holder = cardHolder.player;

        SelectEntity selection = new SelectEntity();
        selection.OnClick = OnClick;
        selection.ActivateEvent();
        return true;
    }

    private void OnClick(EntityHolder entity)
    {
        if (entity)
        {
            castInfo.target = entity;
            cardHolder.card.CheckForResolve();
        }
        else
        {
            cardHolder.player.cardSelection.CancelCast();
        }
    }

    public override void Resolve()
    {
        HealInfo healInfo = new HealInfo(castInfo.holder, castInfo.target, heal);
        castInfo.target.stats.DoHeal(healInfo);
        castInfo.holder.hand.RemoveCard(cardHolder);
    }
}
