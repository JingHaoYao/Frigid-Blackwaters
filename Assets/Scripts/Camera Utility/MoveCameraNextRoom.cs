using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraNextRoom : MonoBehaviour {
    GameObject playerShip;
    public MapUI mapUI;
    public RoomMemory roomMemory;
    public MapSpawn mapSpawn;
    public MapExploration mapExploration;
    MissionManager missionManager;

    //used for camera to track the player
    public Vector2 topRightBoundary, bottomLeftBoundary;
    public bool trackPlayer = false;
    public bool freeCam = false;

    private void moveCamera(int whichDirection)
    {
        if(whichDirection == 1)
        {
            transform.position += new Vector3(-20, 0, 0);
        }
        else if(whichDirection == 2)
        {
            transform.position += new Vector3(20, 0, 0);
        }
        else if(whichDirection == 3)
        {
            transform.position += new Vector3(0, 20, 0);
        }
        else
        {
            transform.position += new Vector3(0, -20, 0);
        }

        PlayerProperties.mainCameraPosition = transform.position;

        foreach(GameObject artifact in PlayerProperties.playerArtifacts.activeArtifacts)
        {
            artifact.GetComponent<ArtifactEffect>()?.cameraMovedPosition(this.transform.position);
        }
    }

    void hasPlayerMovedRooms()
    {
        if (playerShip.transform.position.y < transform.position.y - 10)
        {
            moveCamera(4);
            if (roomMemory != null)
            {
                roomMemory.playerRoom = roomMemory.playerRoom + new Vector2(0, -1);
                if (missionManager.bossInitiated == false)
                {
                    updateTile();
                }
            }
        }
        else if (playerShip.transform.position.y > transform.position.y + 10)
        {
            moveCamera(3);
            if (roomMemory != null)
            {
                roomMemory.playerRoom = roomMemory.playerRoom + new Vector2(0, 1);
                if (missionManager.bossInitiated == false)
                {
                    updateTile();
                }
            }
        }
        else if (playerShip.transform.position.x < transform.position.x - 10)
        {
            moveCamera(1);
            if (roomMemory != null)
            {
                roomMemory.playerRoom = roomMemory.playerRoom + new Vector2(-1, 0);
                if (missionManager.bossInitiated == false)
                {
                    updateTile();
                }
            }
        }
        else if (playerShip.transform.position.x > transform.position.x + 10)
        {
            moveCamera(2);
            if (roomMemory != null)
            {
                roomMemory.playerRoom = roomMemory.playerRoom + new Vector2(1, 0);
                if (missionManager.bossInitiated == false)
                {
                    updateTile();
                }
            }
        }
    }

    void updateTile(){
        for (int i = 0; i < mapUI.mapWidth * mapUI.mapHeight; i++){
            mapExploration = mapSpawn.transform.GetChild(i).gameObject.GetComponent<MapExploration>();
            mapExploration.updateNeeded = true;
        }
    }

    void Start () {
        playerShip = GameObject.Find("PlayerShip");
        mapUI = GameObject.Find("PlayerShip").GetComponent<MapUI>();
        missionManager = FindObjectOfType<MissionManager>();
        if (FindObjectOfType<RoomMemory>())
        {
            roomMemory = GameObject.Find("RoomMemory").GetComponent<RoomMemory>();
            mapSpawn = GameObject.Find("TileBorders").GetComponent<MapSpawn>();
        }
    }

	void Update () {
        if (freeCam == false)
        {
            if (trackPlayer == false)
            {
                hasPlayerMovedRooms();
            }
            else
            {
                transform.position = returnTrackCamPosition();
                PlayerProperties.mainCameraPosition = transform.position;
            }
        }
	}

    public Vector3 returnTrackCamPosition()
    {
        return new Vector3(Mathf.Clamp(playerShip.transform.position.x, bottomLeftBoundary.x, topRightBoundary.x), Mathf.Clamp(playerShip.transform.position.y, bottomLeftBoundary.y, topRightBoundary.y));
    }
}
