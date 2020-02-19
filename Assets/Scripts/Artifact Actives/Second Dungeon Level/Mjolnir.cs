using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mjolnir : ArtifactBonus
{
    public GameObject lightningStrike;
    [SerializeField] DisplayItem displayItem;
    float timer = 0;

    void Update()
    {
        if (displayItem.isEquipped)
        {
            if (timer < 4)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if(PlayerProperties.playerArtifacts.numKills >= 2 && this.isThereViableEnemies() == true)
                {
                    Instantiate(lightningStrike, pickNearbyEnemies().transform.position, Quaternion.identity);
                    PlayerProperties.playerArtifacts.numKills -= 2;
                    timer = 0;
                }
            }
        }
        else
        {
            timer = 0;
        }
    }

    bool isThereViableEnemies()
    {
        foreach (Enemy enemy in EnemyPool.enemyPool)
        {
            if (Vector2.Distance(PlayerProperties.playerShipPosition, enemy.transform.position) <= 5)
            {
                return true;
            }
        }
        return false;
    }

    Enemy pickNearbyEnemies()
    {
        List<Enemy> viableEnemies = new List<Enemy>();
        foreach(Enemy enemy in EnemyPool.enemyPool)
        {
            if(Vector2.Distance(PlayerProperties.playerShipPosition, enemy.transform.position) <= 5)
            {
                viableEnemies.Add(enemy);
            }
        }
        return viableEnemies[Random.Range(0, viableEnemies.Count)];
    }
}
