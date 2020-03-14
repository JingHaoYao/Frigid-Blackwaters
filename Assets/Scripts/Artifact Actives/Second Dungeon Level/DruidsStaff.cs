using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidsStaff : ArtifactEffect
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    ArtifactBonus artifactBonus;
    int amountToHeal;
    float healPeriod = 0;
    public GameObject healingCircle;
    GameObject healCircleInstant;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        artifactBonus = GetComponent<ArtifactBonus>();
    }

    private void Update()
    {
        if(displayItem.isEquipped == true)
        {
            if(healCircleInstant == null)
            {
                healCircleInstant = Instantiate(healingCircle, playerScript.transform.position, Quaternion.identity);
                healCircleInstant.GetComponent<FollowObject>().objectToFollow = playerScript.gameObject;
            }

            healPeriod += Time.deltaTime;
            if (healPeriod > 0.5f)
            {
                if (amountToHeal > 0)
                {
                    if (amountToHeal > 50)
                    {
                        playerScript.healPlayer(50);
                        amountToHeal -= 50;
                    }
                    else
                    {
                        playerScript.healPlayer(amountToHeal);
                        amountToHeal = 0;
                    }
                }
                healPeriod = 0;
            }
        }
        else
        {
            if(amountToHeal != 0)
            {
                amountToHeal = 0;
            }

            if (healCircleInstant != null)
            {
                Destroy(healCircleInstant);
            }
        }
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        amountToHeal += Mathf.RoundToInt(amountDamage * 0.75f);
    }
}
