using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {
    public GameObject[] topOpenRooms;
    public GameObject[] bottomOpenRooms;
    public GameObject[] rightOpenRooms;
    public GameObject[] leftOpenRooms;
    public GameObject doorBlock;

    public int maxRoomCount = 40;
    public int roomCount = 0;

    public float waitTime = 6.3f;
    public float spawnPeriod = 0;

    private void Update()
    {
        if(spawnPeriod < 6.5f)
        {
            spawnPeriod += Time.deltaTime;
        }
    }
}
