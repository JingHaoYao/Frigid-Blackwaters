using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DullEnemy : Enemy
{
    [SerializeField] Enemy enemy;
    public override void deathProcedure()
    {
        throw new System.NotImplementedException();
    }

    public override void damageProcedure(int damage)
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        EnemyPool.removeEnemy(this);
        EnemyPool.addEnemy(enemy);
    }
}
