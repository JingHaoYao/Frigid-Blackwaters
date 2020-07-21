using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyHelmet : ArtifactEffect
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject goldParticles;
    [SerializeField] AudioSource goldAudio;

    public override void artifactEquipped()
    {
        if (PlayerItems.totalGoldAmount >= 2000)
        {
            PlayerProperties.playerScript.addImmunityItem(this.gameObject);
        }
    }

    public override void artifactUnequipped()
    {
        PlayerProperties.playerScript.removeImmunityItem(this.gameObject);
    }

    public override void tookDamage(int amountDamage, Enemy enemy)
    {
        if(PlayerItems.totalGoldAmount >= 2000)
        {
            goldAudio.Play();
            Instantiate(goldParticles, PlayerProperties.playerShipPosition, Quaternion.identity);

            int totalGoldToRemove = 2000;

            List<GameObject> itemsToRemove = new List<GameObject>();

            foreach(GameObject item in PlayerProperties.playerInventory.itemList)
            {
                DisplayItem display = item.GetComponent<DisplayItem>();
                if(display.goldValue > 0)
                {
                    int goldToRemove = Mathf.Clamp(display.goldValue, 0, 1000);
                    totalGoldToRemove -= goldToRemove;
                    display.goldValue -= goldToRemove;
                    if(display.goldValue <= 0)
                    {
                        itemsToRemove.Add(item);
                    }
                }

                if(totalGoldToRemove <= 0)
                {
                    break;
                }
            }

            PlayerItems.totalGoldAmount -= 2000;

            foreach(GameObject item in itemsToRemove)
            {
                PlayerProperties.playerInventory.itemList.Remove(item);
                Destroy(item);
            }

            if(PlayerItems.totalGoldAmount <= 2000)
            {
                PlayerProperties.playerScript.removeImmunityItem(this.gameObject);
            }
        }
        else
        {
            PlayerProperties.playerScript.removeImmunityItem(this.gameObject);
        }
    }

}
