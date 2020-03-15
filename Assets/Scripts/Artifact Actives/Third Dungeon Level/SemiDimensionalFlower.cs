using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiDimensionalFlower : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    public int numberHits = 0;
    GameObject ghostShipInstant;
    public GameObject ghostShip;

    private void Start()
    {
        ghostShipInstant = Instantiate(ghostShip, PlayerProperties.playerShipPosition, Quaternion.identity);
        ghostShipInstant.GetComponent<GhostShip>().flowerScript = this;
        ghostShipInstant.SetActive(false);
    }

    void Update()
    {
        if(displayItem.isEquipped == true)
        {
            if(ghostShipInstant.activeSelf == false)
            {
                ghostShipInstant.transform.position = PlayerProperties.playerShipPosition;
                ghostShipInstant.SetActive(true);
            }
            PlayerProperties.playerScript.damageImmunity = true;
        }
        else
        {
            if(ghostShipInstant.activeSelf == true)
            {
                ghostShipInstant.SetActive(false);
                PlayerProperties.playerScript.damageImmunity = false;
            }
        }
    }
}
