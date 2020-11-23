using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalsOfLife : ArtifactEffect
{
    [SerializeField] GameObject healParticles;
    [SerializeField] DisplayItem displayItem;
    bool onCoolDown = false;

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if(((float)PlayerProperties.playerScript.shipHealth / PlayerProperties.playerScript.shipHealthMAX) <= 0.1f)
        {
            if (onCoolDown == false)
            {
                PlayerProperties.playerScript.healPlayer(Mathf.Clamp(PlayerProperties.playerScript.shipHealthMAX - PlayerProperties.playerScript.shipHealth, 0, PlayerProperties.playerScript.shipHealthMAX));

                foreach(Enemy existingEnemy in EnemyPool.enemyPool)
                {
                    existingEnemy.heal(existingEnemy.maxHealth - existingEnemy.health);
                    GameObject heal = Instantiate(healParticles, existingEnemy.transform.position, Quaternion.identity);
                    heal.GetComponent<FollowObject>().objectToFollow = existingEnemy.gameObject;
                }

                StartCoroutine(coolDown());
                PlayerProperties.durationUI.addTile(displayItem.displayIcon, 60);
            }
            
        }
    }

    IEnumerator coolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(60f);
        onCoolDown = false;
    }
}
