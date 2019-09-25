using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapExploration : MonoBehaviour{
    public int xPos;
    public int yPos;
    public int xID;
    public int yID;
    public int explored;
    public GameObject tile;
    public MapUI mapUI;
    public MapSpawn mapSpawn;
    public RoomMemory roomMemory;
    private MapTemplates templates;
    public bool updateNeeded = false;
    public bool revealNeeded = false;
    public int[,] roomRef = new int[16,4] 
    {{0, 0, 0, 0}, //up down right left (1 means open)
    {1, 0, 0, 0},
    {0, 1, 0, 0},
    {0, 0, 1, 0},
    {0, 0, 0, 1},
    {1, 1, 0, 0},
    {0, 0, 1, 1},
    {1, 0, 0, 1},
    {1, 0, 1, 0},
    {0, 1, 0, 1},
    {0, 1, 1, 0},
    {1, 1, 1, 0},
    {1, 1, 0, 1},
    {0, 1, 1, 1},
    {1, 0, 1, 1},
    {1, 1, 1, 1}};

    void Start(){
        mapUI = GameObject.Find("PlayerShip").GetComponent<MapUI>();
        mapSpawn = GameObject.Find("TileBorders").GetComponent<MapSpawn>();
        roomMemory = GameObject.Find("RoomMemory").GetComponent<RoomMemory>();
        templates = GameObject.Find("MapTemplates").GetComponent<MapTemplates>();

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

    void Update(){
        if (mapUI.adjustedStorage[xPos, yPos] != 0 && updateNeeded == true){
            updateNeeded = false;
            tile = gameObject.transform.GetChild(0).gameObject;
            if (roomMemory.playerRoom == new Vector2(xPos, yPos)){
                Destroy(tile);
                tile = Instantiate(templates.Tiles[0], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tile.transform.SetParent(gameObject.transform);
                tile.GetComponent<RectTransform>().sizeDelta = mapSpawn.tileSize;
                tile.transform.localScale = new Vector3(1, 1, 1);
                explored = 1;
            }
            else{
                Destroy(tile);
                tile = Instantiate(templates.Tiles[roomMemory.roomID[xID, yID] * 2 - 1 + explored], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tile.transform.SetParent(gameObject.transform);
                tile.GetComponent<RectTransform>().sizeDelta = mapSpawn.tileSize;
                tile.transform.localScale = new Vector3(1, 1, 1);
            }
        } //next checks for player room's entrances
        if (roomRef[mapUI.adjustedStorage[(int)roomMemory.playerRoom.x, (int)roomMemory.playerRoom.y], 0] == 1){ //checks top opening
            if ((int)roomMemory.playerRoom.x == xPos && (int)roomMemory.playerRoom.y == yPos - 1){
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (roomRef[mapUI.adjustedStorage[(int)roomMemory.playerRoom.x, (int)roomMemory.playerRoom.y], 1] == 1){ //checks bottom opening
            if ((int)roomMemory.playerRoom.x == xPos && (int)roomMemory.playerRoom.y == yPos + 1){
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (roomRef[mapUI.adjustedStorage[(int)roomMemory.playerRoom.x, (int)roomMemory.playerRoom.y], 2] == 1){ //checks right opening
            if ((int)roomMemory.playerRoom.x == xPos - 1 && (int)roomMemory.playerRoom.y == yPos){
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (roomRef[mapUI.adjustedStorage[(int)roomMemory.playerRoom.x, (int)roomMemory.playerRoom.y], 3] == 1){ //checks left opening
            if ((int)roomMemory.playerRoom.x == xPos + 1 && (int)roomMemory.playerRoom.y == yPos){
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (revealNeeded)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
