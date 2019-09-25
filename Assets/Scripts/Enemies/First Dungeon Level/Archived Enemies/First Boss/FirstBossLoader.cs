using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossLoader : MonoBehaviour
{
    public GameObject blackWindow;
    bool ifLoadedSceneAlready = false;
    public bool findWindow = false;
    public GameObject serenityDialogue;
    public GameObject serenityFadeOut;

    private void Start()
    {
        if (findWindow)
        {
            blackWindow = GameObject.Find("First Boss Manager").GetComponent<FirstBossManager>().sceneTransitioner;
            serenityDialogue = GameObject.Find("First Boss Manager").GetComponent<FirstBossManager>().serenitysDialogue;
            serenityFadeOut = GameObject.Find("First Boss Manager").GetComponent<FirstBossManager>().serenityBlackWindow;
        }
    }

    IEnumerator fadeLoadScene()
    {
        FindObjectOfType<DungeonSoundMonitor>().enabled = false;
        FindObjectOfType<AudioManager>().FadeOut("Dungeon Ambiance", 0.1f);
        FindObjectOfType<AudioManager>().StopSound("Dungeon Waves");
        FindObjectOfType<AudioManager>().StopSound("Idle Ship Movement");
        this.GetComponent<AudioSource>().Play();
        blackWindow.SetActive(true);
        blackWindow.GetComponent<Animator>().GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        Camera.main.transform.position = new Vector3(-800, 0, 0);
        GameObject.Find("PlayerShip").transform.position = new Vector3(-800,-4, 0);
        serenityFadeOut.SetActive(true);
        serenityDialogue.SetActive(true);
        blackWindow.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerShip" && ifLoadedSceneAlready == false)
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().playerDead = true;
            ifLoadedSceneAlready = true;
            StartCoroutine(fadeLoadScene());
        }
    }
}
