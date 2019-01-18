using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiSpawnSpaceDetailer : MonoBehaviour {
    public bool leftOpening, rightOpening, topOpening, bottomOpening;
    public GameObject waterTile;
    bool hasSpawnedWaterTile = false;

    void setRoomMemory()
    {
        if (!leftOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[3] = 1;
        }
        if (!rightOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[2] = 1;
        }
        if (!topOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[0] = 1;
        }
        if (!bottomOpening)
        {
            transform.parent.gameObject.GetComponent<SetRoomDesign>().memoryDoorsOpen[1] = 1;
        }
    }

    private void Start()
    {
        setRoomMemory();
    }

    private void Update()
    {
        if(GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().spawnPeriod >= 6.5f)
        {
            Destroy(this.gameObject, 0.2f);
            if (hasSpawnedWaterTile == false)
            {
                Instantiate(waterTile, transform.position, Quaternion.identity);
                hasSpawnedWaterTile = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AntiSpawnSpaceSpawner") {
            //If two rooms overlap by random instancing, destroy both
            Destroy(transform.parent.gameObject);
        }
    }
}
