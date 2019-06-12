using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Behaviour", menuName = "Cards/Behaviours/Heal Card")]
public class HealCardBehaviour : CardBehaviour
{
    public int heal;

    public override void Cast(CastInfo info)
    {
        HealInfo healInfo = new HealInfo(info.player, info.target, heal);
        info.target.stats.DoHeal(healInfo);
        info.player.hand.RemoveCard(info.cardIndex);
    }
}
