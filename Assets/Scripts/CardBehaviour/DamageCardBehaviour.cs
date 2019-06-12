using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Behaviour", menuName = "Cards/Behaviours/Damage Card")]
public class DamageCardBehaviour : CardBehaviour
{
    public int damage;

    public override void Cast(CastInfo info)
    {
        DamageInfo damageInfo = new DamageInfo(info.player, info.target, damage);
        info.target.stats.DoDamage(damageInfo);
        info.player.hand.RemoveCard(info.cardIndex);
    }
}
