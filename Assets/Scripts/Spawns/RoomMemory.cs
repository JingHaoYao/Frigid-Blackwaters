using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMemory : MonoBehaviour{
    public Vector2 playerRoom; //player position in terms of rooms (Based on adjusted)
    public int[,] roomLayOut = new int[39,39]; //memory for walls and exits of rooms (NOT ADJUSTED, look in mapUI for adjusted)
    public int[,] roomID = new int[19, 19]; //memory for content of rooms (Based on adjusted)
    public GameObject[] foams;
    public int roomLeft; //offset of most left room
    public int roomDown; //offset of most down room
    public int roomRight; //you get it by now *right*?
    public int roomUp;

    //0 - no room
    //1 - top open room
    //2 - bottom open room
    //3 - right open room
    //4 - left open room
    //5 - top and bottom open
    //6 - left and right open
    //7 - left and top open
    //8 - right and top open
    //9 - left and bottom open
    //10 - right and bottom open
    //11 - bottom, top, right open
    //12 - bottom, top, left open
    //13 - bottom, left, right open
    //14 - top, left, right open
    //15 - all open
}
