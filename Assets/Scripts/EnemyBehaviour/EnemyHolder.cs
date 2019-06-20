using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : EntityHolder
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void Attack (PlayerHolder player)
    {
        int dmg = (int)Random.Range(0, (player.stats.maxHealth * 0.4F) + 0.1F);
        player.stats.DoDamage(new DamageInfo(this, player, dmg));

        player.hand.DiscardHand();
        player.AddHand();
        player.mana = player.maxMana;
    }
}
