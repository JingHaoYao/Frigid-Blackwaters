using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRoom : MonoBehaviour {
    public int whichSceneLoad = 1;
    public GameObject blackWindow;
    bool ifLoadedSceneAlready = false;
    public bool findWindow = false;

    private void Start()
    {
        if (findWindow)
        {
            blackWindow = GameObject.Find("SceneTransitioner");
        }
    }

    IEnumerator fadeLoadScene()
    {
        blackWindow.SetActive(true);
        blackWindow.GetComponent<Animator>().GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        if (whichSceneLoad != 1)
        {
            SceneManager.LoadScene(whichSceneLoad);
        }
        else
        {
            if (MiscData.dungeonLevelUnlocked == 3)
            {
                SceneManager.LoadScene(5);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerShip" && ifLoadedSceneAlready == false)
        {
            PlayerProperties.playerScript.addRootingObject();
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = true;
            ifLoadedSceneAlready = true;
            StartCoroutine(fadeLoadScene());
        }
    }
}
