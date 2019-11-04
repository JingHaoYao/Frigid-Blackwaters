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

    public override void addedKill(string tag, Vector3 deathPos)
    {
    }
    // Whenever the player takes damage
    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        amountToHeal += Mathf.RoundToInt(amountDamage * 0.75f);
    }
    // Whenever the player fires the left weapon, and so on
    public override void firedLeftWeapon(GameObject[] bullet)
    {
    }
    public override void firedFrontWeapon(GameObject[] bullet)
    {
    }
    public override void firedRightWeapon(GameObject[] bullet)
    {
    }
    // Whenever the player enters a previously unentered room
    public override void exploredNewRoom(int whatRoomType) { }
    // Whenever the player picks up an item (updates the inventory)
    public override void updatedInventory()
    {
    }
    // whenever the player dashes
    public override void playerDashed()
    {
    }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
    }
}
