using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HealCardBehaviour : CardBehaviour
{
    public struct CastInfo
    {
        public PlayerHolder holder;
        public EntityHolder target;
    }

    private CastInfo castInfo;
    private SelectEntity selection;

    public int heal {
        get { return cardBehaviourProperties.intValue; }
    }

    public override bool canResolve {
        get { return castInfo.target != null; }
    }

    public override bool CanCast()
    {
        return cardHolder.player.mana >= cardHolder.card.info.cost;
    }

    public override void Cast()
    {

        castInfo = new CastInfo();
        castInfo.holder = cardHolder.player;

        selection = new SelectEntity();
        selection.OnClick = OnClick;
        selection.ActivateEvent();
    }

    public override void CancelCast()
    {
        selection.RemoveEvent();
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
            cardHolder.player.cardSelection.CancelSelection();
        }
    }

    public override void Resolve()
    {
        HealInfo healInfo = new HealInfo(castInfo.holder, castInfo.target, heal);
        castInfo.target.stats.DoHeal(healInfo);
        castInfo.holder.hand.RemoveCard(cardHolder);
        cardHolder.player.mana -= cardHolder.card.info.cost;
    }
}
