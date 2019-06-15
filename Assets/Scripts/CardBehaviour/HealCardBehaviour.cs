using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SImple Heal Card Behaviour", menuName = "Cards/Behaviours/Simple Heal")]
public class HealCardBehaviour : CardBehaviour
{
    public struct CastInfo
    {
        public PlayerHolder holder;
        public EntityHolder target;
    }

    [SerializeField]
    private int heal = 0;
    private CardHolder cardHolder;
    private CastInfo castInfo;

    public override bool canResolve {
        get { return castInfo.target != null; }
    }

    public override bool Cast(CardHolder card)
    {
        castInfo = new CastInfo();
        cardHolder = card;
        castInfo.holder = card.player;

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
        } else
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
