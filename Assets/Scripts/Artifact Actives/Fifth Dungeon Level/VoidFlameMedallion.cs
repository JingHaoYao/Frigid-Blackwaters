using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidFlameMedallion : ArtifactEffect
{
    Enemy targetEnemy;
    int currentCount = 0;
    [SerializeField] GameObject voidExplosion;

    public override void dealtDamage(int damageDealt, Enemy enemy)
    { 
        if(targetEnemy == enemy)
        {
            currentCount++;
            if (currentCount >= 3)
            {
                currentCount = 0;
                Vector3 directionVector = enemy.transform.position - PlayerProperties.playerShipPosition;
                Instantiate(voidExplosion, enemy.transform.position + Vector3.up, Quaternion.Euler(0, 0, Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg + 180));
                enemy.dealDamage(8);
                targetEnemy = null;
            }
        }
        else
        {
            targetEnemy = enemy;
            currentCount = 1;
        }
    }
}
