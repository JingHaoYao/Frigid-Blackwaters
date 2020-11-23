using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHubMap : MonoBehaviour
{
    public GameObject[] icons;
    GameObject playerShip;
    public GameObject mapMenu;
    PlayerScript playerScript;

    MenuSlideAnimation menuSlideAnimation = new MenuSlideAnimation();

    void SetAnimation()
    {
        menuSlideAnimation.SetOpenAnimation(new Vector3(0, -585, 0), new Vector3(0, 0, 0), 0.25f);
        menuSlideAnimation.SetCloseAnimation(new Vector3(0, 0, 0), new Vector3(0, -585, 0), 0.25f);
    }

    void Start()
    {
        playerShip = PlayerProperties.playerShip;
        playerScript = playerShip.GetComponent<PlayerScript>();
        SetAnimation();
    }

    public void PlayOpeningAnimation()
    {
        menuSlideAnimation.PlayOpeningAnimation(mapMenu);
    }

    private void LateUpdate()
    {
        if (mapMenu.activeSelf == false)
        {
            if (
                playerScript.windowAlreadyOpen == false && playerScript.playerDead == false
                )
            {
                if (Input.GetKeyDown(KeyCode.M) && menuSlideAnimation.IsAnimating == false)
                {
                    playerScript.windowAlreadyOpen = true;
                    mapMenu.SetActive(true);
                    menuSlideAnimation.PlayOpeningAnimation(mapMenu);
                    UpdateUI();
                    Time.timeScale = 0;
                }
            }
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M)) && menuSlideAnimation.IsAnimating == false)
            {
                playerScript.windowAlreadyOpen = false;
                menuSlideAnimation.PlayEndingAnimation(mapMenu, () => { mapMenu.SetActive(false); });
                Time.timeScale = 1;
            }
        }
    }

    int returnPlayerPos()
    {
        if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 0)) < 0.2f)
        {
            return 3;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, -20)) < 0.2f)
        {
            return 2;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(20, 0)) < 0.2f)
        {
            return 0;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(-20, 0)) < 0.2f)
        {
            return 5;
        }
        else if (Vector2.Distance(Camera.main.transform.position, new Vector3(-20, -20)) < 0.2f)
        {
            return 4;
        }
        else
        {
            return 1;
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
