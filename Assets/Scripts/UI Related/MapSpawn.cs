using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSpawn : MonoBehaviour{
    public MapUI mapUI;
    public bool mapUploaded = false;
    private MapTemplates templates;
    public RoomMemory roomMemory;
    public MapExploration mapExploration;
    public Vector2 tileSize;
    List<MapExploration> tileList = new List<MapExploration>();

    void Awake(){
        mapUI = GameObject.Find("PlayerShip").GetComponent<MapUI>();
        templates = GameObject.Find("MapTemplates").GetComponent<MapTemplates>();
        roomMemory = GameObject.Find("RoomMemory").GetComponent<RoomMemory>();
    }

    void LateUpdate(){
        uploadMap();
    }

    void addTiles(MapExploration _mapExploration)
    {
        if(roomMemory.roomID[_mapExploration.xID, _mapExploration.yID] > 3)
        {
            tileList.Insert(Random.Range(0, tileList.Count), _mapExploration);
        }
    }

    void revealTiles()
    {
        int numberRevealed = 0;
        int count = tileList.Count;
        foreach(MapExploration tile in tileList)
        {
            if(numberRevealed >= 3)
            {
                break;
            }

            if(roomMemory.roomID[tile.xID, tile.yID] == 7 || roomMemory.roomID[tile.xID, tile.yID] == 12)
            {
                tile.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                int percentChance = Random.Range(1, 101);
                if (percentChance < 25 + (3 - numberRevealed) * 10)
                {
                    tile.transform.localScale = new Vector3(1, 1, 1);
                    numberRevealed++;
                }
            }
        }
    }

    void uploadMap(){ //actually puts stuff on the map
        if (mapUI.mapLoaded == true){
            if (mapUploaded == false){
                mapUploaded = true;
                if (mapUI.mapWidth > mapUI.mapHeight){
                    tileSize = new Vector2(450 / mapUI.mapWidth - 4, 450 / mapUI.mapWidth - 4); //calculates appropriate tile size
                    gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(450 / mapUI.mapWidth, 450 / mapUI.mapWidth); //calculates appropriate border size
                    gameObject.GetComponent<GridLayoutGroup>().constraintCount = mapUI.mapHeight; //reduces amount of rows on map
                    gameObject.GetComponent<GridLayoutGroup>().padding.bottom = 50 + ((450 - (mapUI.mapHeight * 450 / mapUI.mapWidth)) / 2); //adjusts padding to center map
                    gameObject.GetComponent<GridLayoutGroup>().padding.top = 50 + ((450 - (mapUI.mapHeight * 450 / mapUI.mapWidth)) / 2);
                }
                else{
                    tileSize = new Vector2(450 / mapUI.mapHeight - 4, 450 / mapUI.mapHeight - 4);
                    gameObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(450 / mapUI.mapHeight, 450 / mapUI.mapHeight);
                    gameObject.GetComponent<GridLayoutGroup>().constraintCount = mapUI.mapHeight;
                    gameObject.GetComponent<GridLayoutGroup>().padding.left = 50 + ((450 - (mapUI.mapWidth * 450 / mapUI.mapHeight)) / 2);
                    gameObject.GetComponent<GridLayoutGroup>().padding.right = 50 + ((450 - (mapUI.mapWidth * 450 / mapUI.mapHeight)) / 2);
                }
                for (int i = 0; i < mapUI.mapWidth; i++){
                    for (int j = 0; j < mapUI.mapHeight; j++){
                        GameObject border;
                        GameObject tile;
                        if (i == (-roomMemory.roomLeft) && j == (-roomMemory.roomDown)){ //spawn room
                            roomMemory.playerRoom = new Vector2(i,j); //identify location of player
                            roomMemory.roomLayOut[0, 0] = 13; //updates roomMemory to include spawn (instead of 0)
                            mapUI.adjustedStorage[-roomMemory.roomLeft, -roomMemory.roomDown] = 13; //updates adjustedStorage to include spawn (instead of 0)
                            border = Instantiate(templates.Borders[13], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            mapExploration = border.GetComponent<MapExploration>();
                            mapExploration.xPos = i;
                            mapExploration.yPos = j;
                            mapExploration.xID = i + (roomMemory.roomLeft + 15);
                            mapExploration.yID = j + (roomMemory.roomDown + 15);
                            mapExploration.explored = 1; //discovered
                            border.transform.SetParent(gameObject.transform);
                            border.transform.localScale = new Vector3(1, 1, 1);
                            tile = Instantiate(templates.Tiles[0], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            tile.transform.SetParent(border.transform);
                            tile.GetComponent<RectTransform>().sizeDelta = tileSize;
                            tile.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else{ //everything else
                            border = Instantiate(templates.Borders[mapUI.adjustedStorage[i, j]], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            mapExploration = border.GetComponent<MapExploration>();
                            mapExploration.xPos = i;
                            mapExploration.yPos = j;
                            mapExploration.xID = i + (roomMemory.roomLeft + 10);
                            mapExploration.yID = j + (roomMemory.roomDown + 10);
                            addTiles(mapExploration);
                            //roomMemory.roomID[i, j] = 1; //undiscovered empty room
                            border.transform.SetParent(gameObject.transform);
                            border.transform.localScale = new Vector3(0, 0, 0); //makes all rooms (other than spawn) invisible
                            if (mapUI.adjustedStorage[i, j] != 0){
                                //border.transform.localScale = new Vector3(1, 1, 1); //makes non empty rooms visible again (FOR DEBUG)
                                tile = Instantiate(templates.Tiles[1], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                                tile.transform.SetParent(border.transform);
                                tile.GetComponent<RectTransform>().sizeDelta = tileSize;
                                tile.transform.localScale = new Vector3(1, 1, 1);
                            }
                        }
                    }
                }
                revealTiles();
            }
        }
    }
}
