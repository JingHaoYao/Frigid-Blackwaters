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
        SceneManager.LoadScene(whichSceneLoad);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerShip" && ifLoadedSceneAlready == false)
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = true;
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = true;
            ifLoadedSceneAlready = true;
            StartCoroutine(fadeLoadScene());
        }
    }
}
