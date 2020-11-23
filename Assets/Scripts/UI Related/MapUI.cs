using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour{
    public GameObject mapUI;
    public bool loadingMap = false;
    public int mapWidth;
    public int mapHeight;
    public RoomMemory roomMemory;
    public int[,] adjustedStorage = new int[39, 39]; //adjusted roomMemory, unlooping array
    public bool mapLoaded = false;
    public bool mKey;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(0, -585, 0), 0.25f);
    }

    public void PlayOpenAnimation()
    {
        menuSlideAnimation.PlayOpeningAnimation(mapUI);
    }

    void Start(){
        mapUI.transform.localScale = new Vector3(0, 0, 0);
        SetAnimation();
    }

    void LateUpdate(){
        if (PlayerProperties.playerScript.playerDead == false && menuSlideAnimation.IsAnimating == false){
            if (mapUI.transform.localScale == new Vector3 (0, 0, 0)){
                if (Input.GetKeyDown(KeyCode.M) && GetComponent<PlayerScript>().windowAlreadyOpen == false){
                    GetComponent<PlayerScript>().windowAlreadyOpen = true;
                    mapUI.transform.localScale = new Vector3(1, 1, 1);
                    menuSlideAnimation.PlayOpeningAnimation(mapUI);
                    Time.timeScale = 0;
                }
            }
            else{
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M)) {
                    GetComponent<PlayerScript>().windowAlreadyOpen = false;
                    Time.timeScale = 1;
                    menuSlideAnimation.PlayEndingAnimation(mapUI, () => { mapUI.transform.localScale = Vector3.zero; });
                }
            }
        }
        loadMap();
    }

    void loadMap(){
        if (loadingMap == true){ //through testing, it appears that it triggers this conditional twice
            loadingMap = false;
            roomMemory = GameObject.Find("RoomMemory").GetComponent<RoomMemory>();
            mapWidth = roomMemory.roomRight - roomMemory.roomLeft + 1; //determines size of map tiles for future reference
            mapHeight = roomMemory.roomUp - roomMemory.roomDown + 1;
            for (int i = 0; i < 39; i++){ //this serves to make the roomMemory more readable (without it, array loops around)
                for (int j = 0; j < 39; j++){
                    if (i < 39 + roomMemory.roomLeft && j < 39 + roomMemory.roomDown){
                        adjustedStorage[i - roomMemory.roomLeft, j - roomMemory.roomDown] = roomMemory.roomLayOut[i, j];
                    }
                    else if (i >= 39 + roomMemory.roomLeft && j < 39 + roomMemory.roomDown){
                        adjustedStorage[i - roomMemory.roomLeft - 39, j - roomMemory.roomDown] = roomMemory.roomLayOut[i, j];
                    }
                    else if (i < 39 + roomMemory.roomLeft && j >= 39 + roomMemory.roomDown){
                        adjustedStorage[i - roomMemory.roomLeft, j - roomMemory.roomDown - 39] = roomMemory.roomLayOut[i, j];
                    }
                    else{
                        adjustedStorage[i - roomMemory.roomLeft - 39, j - roomMemory.roomDown - 39] = roomMemory.roomLayOut[i, j];
                    }
                }
            }
            mapLoaded = true; //map visualization- triggers function in MapSpawn
        }
    }
}
