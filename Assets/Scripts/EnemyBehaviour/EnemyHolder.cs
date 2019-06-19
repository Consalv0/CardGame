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
        int dmg = Random.Range(0, player.stats.maxHealth / 10+1);

    }

    
}
