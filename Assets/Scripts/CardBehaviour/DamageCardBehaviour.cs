﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Simple Damage Card Behaviour", menuName = "Cards/Behaviours/Simple Damage")]
public class DamageCardBehaviour : CardBehaviour
{
    public struct CastInfo {
        public PlayerHolder holder;
        public EntityHolder target;
    }

    private CardHolder cardHolder;
    public int damage;
    private CastInfo castInfo;

    public override bool canResolve {
        get { return castInfo.target != null; }
    }

    public override bool Cast(CardHolder card)
    {
        cardHolder = card;
        castInfo = new CastInfo();
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
        DamageInfo damageInfo = new DamageInfo(castInfo.holder, castInfo.target, damage);
        castInfo.target.stats.DoDamage(damageInfo);
        cardHolder.player.hand.RemoveCard(cardHolder);
    }
}
