using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHubMap : MonoBehaviour
{
    public GameObject[] icons;
    GameObject playerShip;
    public GameObject mapMenu;
    PlayerScript playerScript;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
    }

    private void LateUpdate()
    {
        if (mapMenu.activeSelf == false)
        {
            if (
                playerScript.windowAlreadyOpen == false && playerScript.playerDead == false
                )
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    playerScript.windowAlreadyOpen = true;
                    mapMenu.SetActive(true);
                    UpdateUI();
                    Time.timeScale = 0;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M))
            {
                playerScript.windowAlreadyOpen = false;
                mapMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    int returnPlayerPos()
    {
        if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 0)) < 0.2f)
        {
            return 4;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, -20)) < 0.2f)
        {
            return 3;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(20, 0)) < 0.2f)
        {
            return 1;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(-20, 0)) < 0.2f)
        {
            return 6;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(-20, -20)) < 0.2f)
        {
            return 5;
        }
        else if(Vector2.Distance(Camera.main.transform.position, new Vector3(20, 20)) < 0.2f)
        {
            return 0;
        }
        else
        {
            return 2;
        }
    }

    public void UpdateUI()
    {
        int whatPos = returnPlayerPos();
        for(int i = 0; i < icons.Length; i++)
        {
            if(i != whatPos)
            {
                icons[i].GetComponent<HubMapIcon>().shipIcon.SetActive(false);
            }
            else
            {
                icons[i].GetComponent<HubMapIcon>().shipIcon.SetActive(true);
            }
        }
    }
}
