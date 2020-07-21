using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiDimensionalFlower : ArtifactEffect
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
            PlayerProperties.playerScript.addImmunityItem(this.gameObject);
        }
        else
        {
            if(ghostShipInstant.activeSelf == true)
            {
                ghostShipInstant.SetActive(false);
                PlayerProperties.playerScript.removeImmunityItem(this.gameObject);
            }
        }
    }

    public override void cameraMovedPosition(Vector3 currentPosition)
    {
        if (ghostShip != null)
        {
            ghostShip.transform.position = PlayerProperties.playerShipPosition;
        }
    }
}
