using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottledFrost : ArtifactEffect
{
    public GameObject slowEffect;

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if (!enemy.containsStatus("Slow Effect"))
        {
            GameObject slowEffectInstant = Instantiate(slowEffect, enemy.transform.position, Quaternion.identity);
            enemy.addStatus(slowEffectInstant.GetComponent<SlowEffect>(), 2);
        }
    }
}
