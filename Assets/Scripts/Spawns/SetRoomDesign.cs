  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoomDesign : MonoBehaviour {
    SpriteRenderer rend;
    public Sprite[] designList;
    public int[] memoryDoorsOpen = new int[4] { 0, 0, 0, 0 }; //top, bottom, right, left
                                                              //1 means door is closed at the end of spawning
    bool memoryStored = false;
    int xPos = 0; //transformed both into int
    int yPos = 0;
    RoomMemory roomMemory;
    int roomKey;
    public int whichDesign = 0;
    public List<int> theme2Designs;

    public void setRoomID(int whatRoomType)
    {
        int tempx = 0;
        int tempy = 0;
        tempx = 15 + xPos;
        tempy = 15 + yPos;
        roomMemory.roomID[tempx, tempy] = whatRoomType;
    }

    void updateAntiSpawnSpaceSpawner()
    {
        if (memoryDoorsOpen[0] == 1)
        {
            transform.GetChild(0).GetComponent<AntiSpawnSpaceDetailer>().topOpening = false;
        }
        if (memoryDoorsOpen[1] == 1)
        {
            transform.GetChild(0).GetComponent<AntiSpawnSpaceDetailer>().bottomOpening = false;
        }
        if (memoryDoorsOpen[2] == 1)
        {
            transform.GetChild(0).GetComponent<AntiSpawnSpaceDetailer>().rightOpening = false;
        }
        if (memoryDoorsOpen[3] == 1)
        {
            transform.GetChild(0).GetComponent<AntiSpawnSpaceDetailer>().leftOpening = false;
        }
    }

    void Awake () {
        roomMemory = GameObject.Find("RoomMemory").GetComponent<RoomMemory>();
        rend = GetComponent<SpriteRenderer>();
        if (this.gameObject.name != "SpawnRoom")
        {
            whichDesign = Random.Range(0, designList.Length);
            rend.sprite = designList[whichDesign];
        }
        xPos = Mathf.RoundToInt(transform.position.x / 19.98f);
        yPos = Mathf.RoundToInt(transform.position.y / 19.98f);
    }

    void storeInMemory()
    {
        if (memoryDoorsOpen[0] == 1)
        {
            if (memoryDoorsOpen[1] == 1)
            {
                if(memoryDoorsOpen[2] == 1)
                {
                    roomKey = 4;
                }
                else
                {
                    if (memoryDoorsOpen[3] == 1)
                    {
                        roomKey = 3;
                    }
                    else
                    {
                        roomKey = 6;
                    }
                }
            }
            else
            {
                if (memoryDoorsOpen[2] == 1)
                {
                    if (memoryDoorsOpen[3] == 1)
                    {
                        roomKey = 2;
                    }
                    else
                    {
                        roomKey = 9;
                    }
                }
                else
                {
                    if (memoryDoorsOpen[3] == 1)
                    {
                        roomKey = 10;
                    }
                    else
                    {
                        roomKey = 13;
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
                    roomKey = 1;
                }
                else
                {
                    roomKey = 7;
                }
            }
            else
            {
                if (memoryDoorsOpen[3] == 1)
                {
                    roomKey = 8;
                }
                else
                {
                    roomKey = 14;
                }
            }
        }
        else if(memoryDoorsOpen[2] == 1)
        {
            if(memoryDoorsOpen[3] == 1)
            {
                roomKey = 5;
            }
            else
            {
                roomKey = 12;
            }
        }
        else if(memoryDoorsOpen[3] == 1)
        {
            roomKey = 11;
        }
        else
        {
            roomKey = 15;
        }
        if (xPos < 0)
        { //these two loop the array around so that no negative integers appear in the coordonates of the array
            xPos = 39 + xPos;
        }
        if (yPos < 0)
        {
            yPos = 39 + yPos;
        }
        roomMemory.roomLayOut[xPos, yPos] = roomKey;
    }

    void IdentifyOffset(){ //added for various purposes... makes job easier
        if (roomMemory.roomLeft > xPos){
            roomMemory.roomLeft = xPos;
        }
        if (roomMemory.roomDown > yPos){
            roomMemory.roomDown = yPos;
        }
        if (roomMemory.roomRight < xPos){
            roomMemory.roomRight = xPos;
        }
        if (roomMemory.roomUp < yPos){
            roomMemory.roomUp = yPos;
        }
    }

    void Update () {
        if (GameObject.Find("RoomTemplates").GetComponent<RoomTemplates>().spawnPeriod >= 6.5f && gameObject.name != "SpawnRoom" && gameObject.name != "DoorBlock(Clone)")
        {
            if(memoryStored == false){
                updateAntiSpawnSpaceSpawner();
                IdentifyOffset();
                storeInMemory();
                memoryStored = true;
            }
        }
    }
}
