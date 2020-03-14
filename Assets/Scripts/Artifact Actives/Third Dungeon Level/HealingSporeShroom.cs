using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSporeShroom : MonoBehaviour
{
    float healPeriod = 0;
    [SerializeField] private DisplayItem displayItem;
    private GameObject mushRoomInstant;
    public GameObject healingMushroom;
    Camera mainCamera;
    [SerializeField] LayerMask layerMask;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (displayItem.isEquipped && PlayerProperties.playerScript.enemiesDefeated == false)
        {
            if(healPeriod >= 15)
            {
                healPeriod = 0;
                if(mushRoomInstant == null)
                {
                    mushRoomInstant = Instantiate(healingMushroom, randomPos(), Quaternion.identity);
                }
            }
            else
            {
                healPeriod += Time.deltaTime;
            }
        } 
    }

    Vector3 randomPos()
    {
        Vector3 positionToReturn = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
        while(Vector2.Distance(positionToReturn, PlayerProperties.playerShipPosition) < 5 && Physics2D.OverlapCircle(positionToReturn, 0.4f, layerMask))
        {
            positionToReturn = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
        }
        return positionToReturn;
    }
}
