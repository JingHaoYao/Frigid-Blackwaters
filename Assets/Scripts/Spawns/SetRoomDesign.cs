using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoomDesign : MonoBehaviour {
    SpriteRenderer rend;
    public Sprite[] designList;
    public int[] memoryDoorsOpen = new int[4] { 0, 0, 0, 0 }; //top, bottom, right, left
                                                              //1 means door is closed at the end of spawning
    bool memoryStored = false;
    float xPos = 0;
    float yPos = 0;
    GameObject roomMemory;

    void Start () {
        roomMemory = GameObject.Find("RoomMemory");
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = designList[Random.Range(0, designList.Length - 1)];
        xPos = transform.position.x / 19.98f;
        yPos = transform.position.y / 19.98f;
    }

    void storeInMemory()
    {
        if (memoryDoorsOpen[0] == 1)
        {
            if (memoryDoorsOpen[1] == 1)
            {
                if(memoryDoorsOpen[2] == 1)
                {
                    roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 4;
                }
                else
                {
                    if (memoryDoorsOpen[3] == 1)
                    {
                        roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 3;
                    }
                    else
                    {
                        roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 6;
                    }
                }
            }
            else
            {
                if (memoryDoorsOpen[2] == 1)
                {
                    if (memoryDoorsOpen[3] == 1)
                    {
                        roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 2;
                    }
                    else
                    {
                        roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 9;
                    }
                }
                else
                {
                    if (memoryDoorsOpen[3] == 1)
                    {
                        roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 10;
                    }
                    else
                    {
                        roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 13;
                    }
                }
            }
        }
        else if(memoryDoorsOpen[1] == 1)
        {
            if(memoryDoorsOpen[2] == 1)
            {
                if(memoryDoorsOpen[3] == 1)
                {
                    roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 1;
                }
                else
                {
                    roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 7;
                }
            }
            else
            {
                if (memoryDoorsOpen[3] == 1)
                {
                    roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 8;
                }
                else
                {
                    roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 14;
                }
            }
        }
        else if(memoryDoorsOpen[2] == 1)
        {
            if(memoryDoorsOpen[3] == 1)
            {
                roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 5;
            }
            else
            {
                roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 12;
            }
        }
        else if(memoryDoorsOpen[3] == 1)
        {
            roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 11;
        }
        else
        {
            roomMemory.GetComponent<RoomMemory>().roomLayOut[(int)xPos, (int)yPos] = 15;
        }
    }

	void Update () {
        if (GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().spawnPeriod >= 6.5f)
        {
            if(memoryStored == false)
            {
                storeInMemory();
                memoryStored = true;
            }
        }
    }
}
