using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLifeArtifact : MonoBehaviour
{
    DisplayItem displayItem;
    Artifacts artifacts;
    PlayerScript playerScript;
    GameObject playerShip;
    bool set1 = false, set2 = false;
    bool hasUsedSecondLife = false;
    int numberLifeAdded = 0;
    public Sprite exhaustedImage;

    void Start()
    {
        displayItem = GetComponent<DisplayItem>();
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        if (displayItem.isEquipped == true)
        {
            if(set1 == false && hasUsedSecondLife == false)
            {
                set1 = true;
                playerScript.numberLives++;
                numberLifeAdded = playerScript.numberLives;
            }
            set2 = false;

            if(playerScript.playerDead == true && hasUsedSecondLife == false)
            {
                if (playerScript.numberLives == numberLifeAdded - 1)
                {
                    hasUsedSecondLife = true;
                    displayItem.displayIcon = exhaustedImage;
                }
            }
        }
        else
        {
            if(set2 == false && playerScript.numberLives > 0 && hasUsedSecondLife == false)
            {
                if (numberLifeAdded == playerScript.numberLives)
                {
                    set2 = true;
                    playerScript.numberLives--;
                }
            }
            set1 = false;
        }
    }
}
