using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncyclopedia : MonoBehaviour
{
    [SerializeField]
    OpenBookMenu openMenu;
    [SerializeField]
    ClosedBookMenu closedMenu;
    [SerializeField]
    GameObject raycastBlocker;
    MenuSlideAnimation menuAnimation;
    bool encyclopediaActive = false;

    void Start()
    {
        openMenu.gameObject.SetActive(false);
        closedMenu.gameObject.SetActive(false);
        raycastBlocker.SetActive(false);
        menuAnimation = new MenuSlideAnimation();
        menuAnimation.SetCloseAnimation(Vector3.zero, new Vector3(0, 600, 0), 0.25f);
        menuAnimation.SetOpenAnimation(new Vector3(0, 600, 0), Vector3.zero, 0.25f);
        openMenu.Initialize(this);
        closedMenu.Initialize(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && menuAnimation.IsAnimating == false && PlayerProperties.playerScript.playerDead == false)
        {
            if (encyclopediaActive == false && PlayerProperties.playerScript.windowAlreadyOpen == false)
            {
                PlayerProperties.playerScript.windowAlreadyOpen = true;
                encyclopediaActive = true;
                Time.timeScale = 0;
                CloseDungeonPage();
                raycastBlocker.SetActive(true);
                menuAnimation.PlayOpeningAnimation(gameObject);
            }
            else if (encyclopediaActive == true)
            {
                PlayerProperties.playerScript.windowAlreadyOpen = false;
                encyclopediaActive = false;
                Time.timeScale = 1;
                menuAnimation.PlayEndingAnimation(gameObject, ()=> {
                    openMenu.gameObject.SetActive(false);
                    closedMenu.gameObject.SetActive(false);
                    raycastBlocker.SetActive(false);
                });
            }
        }
    }

    public OpenBookMenu GetOpenMenu
    {
        get { return openMenu; }
    }

    public ClosedBookMenu GetClosedBookMenu
    {
        get { return closedMenu; }
    }

    public void OpenDungeonPage(int dungeonLevel)
    {
        openMenu.gameObject.SetActive(true);
        closedMenu.gameObject.SetActive(false);
        openMenu.SetOpenBookMenu(dungeonLevel, false);
    }

    public void CloseDungeonPage()
    {
        openMenu.gameObject.SetActive(false);
        closedMenu.gameObject.SetActive(true);
    }
}