using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMemory : MonoBehaviour
{
    public int[,] roomLayOut = new int[37,37];

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
    //16 - spawn room

    private void Start()
    {
        for(int i = 0; i < 37; i++)
        {
            for(int k = 0; k < 37; k++)
            {
                roomLayOut[i,k] = 0;
            }
        }
    }
}
