using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanditRoom : MonoBehaviour {
    int tollAmount;
    public GameObject obstacleToolTip, yesIndicator, noIndicator;
    GameObject spawnedYI, spawnedNI, CWBanditShip, CCWBanditShip;
    public GameObject banditShip;
    public GameObject[] initShips;
    Text text;
    bool choiceMade = false;
    bool saidNo = false;
    Inventory inventory;
    AntiSpawnSpaceDetailer anti;

    void Start () {
        text = GetComponent<Text>();
        anti = GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
        tollAmount = Random.Range(4, 15) * 100;
        spawnedYI = Instantiate(yesIndicator, transform.position + new Vector3(-1, -7, 0), Quaternion.identity);
        spawnedNI = Instantiate(noIndicator, transform.position + new Vector3(1, -7, 0), Quaternion.identity);
        obstacleToolTip.GetComponentInChildren<Text>().text = text.text + tollAmount + " gold.";
        obstacleToolTip.SetActive(true);
    }
    
    void awakenShips()
    {
        CWBanditShip = Instantiate(banditShip, initShips[0].transform.position, Quaternion.identity);
        CWBanditShip.GetComponent<BanditShip>().cw = true;
        Destroy(initShips[0]);
        CCWBanditShip = Instantiate(banditShip, initShips[1].transform.position, Quaternion.identity);
        CCWBanditShip.GetComponent<BanditShip>().cw = false;
        Destroy(initShips[1]);
        anti.spawnDoorSeals();
        anti.trialDefeated = false;
    }

	void Update () {
        if (choiceMade == false)
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = true;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                obstacleToolTip.SetActive(false);
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                Destroy(spawnedYI);
                Destroy(spawnedNI);
                choiceMade = true;
                subtractToll();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                obstacleToolTip.SetActive(false);
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                Destroy(spawnedYI);
                Destroy(spawnedNI);
                choiceMade = true;
                saidNo = true;
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
                awakenShips();
                //spawn stuff
            }
        }
        else
        {
            if(CWBanditShip == null && CCWBanditShip == null && saidNo == true)
            {
                anti.trialDefeated = true;
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
                Destroy(this.gameObject);
            }
        }
    }

    void subtractToll()
    {
        int remainder = tollAmount;
        int index = 0;
        int listLength = inventory.itemList.Count;
        while (remainder > 0 && index < listLength)
        {
            while (inventory.itemList[index].GetComponent<DisplayItem>().goldValue == 0)
            {
                index++;
            }

            if (remainder >= inventory.itemList[index].GetComponent<DisplayItem>().goldValue)
            {
                remainder -= inventory.itemList[index].GetComponent<DisplayItem>().goldValue;
                GameObject item = inventory.itemList[index];
                inventory.itemList.Remove(inventory.itemList[index]);
                Destroy(item);
            }
            else
            {
                inventory.itemList[index].GetComponent<DisplayItem>().goldValue -= remainder;
                remainder = 0;
            }
        }
    }
}
