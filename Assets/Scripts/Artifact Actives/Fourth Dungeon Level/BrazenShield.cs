using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazenShield : ArtifactEffect
{
    int hitCounter = 0;
    [SerializeField] GameObject brazenFlame;
    [SerializeField] ArtifactBonus artifactBonus;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        hitCounter++;
        if(hitCounter == 3)
        {
            hitCounter = 0;
            artifactBonus.defenseBonus = 0f;
            PlayerProperties.playerArtifacts.UpdateStats();
            StartCoroutine(spawnFlames(enemy.transform.position));
        }
        else if(hitCounter == 2)
        {
            artifactBonus.defenseBonus = 0.25f;
            PlayerProperties.playerArtifacts.UpdateStats();
        }
    }

    IEnumerator spawnFlames(Vector3 targetPosition)
    {
        float angle = Mathf.Atan2(targetPosition.y - PlayerProperties.playerShipPosition.y, targetPosition.x - PlayerProperties.playerShipPosition.x);
        Vector3 originalPlayerPosition = PlayerProperties.playerShipPosition;
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                float angleToConsider = (angle * Mathf.Rad2Deg - 10 + 10 * k) * Mathf.Deg2Rad;
                Vector3 spawnPos = originalPlayerPosition + new Vector3(Mathf.Cos(angleToConsider), Mathf.Sin(angleToConsider)) * (i + 1);
                if (!Physics2D.OverlapCircle(spawnPos, 0.5f, 12))
                {
                    Instantiate(brazenFlame, spawnPos, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
