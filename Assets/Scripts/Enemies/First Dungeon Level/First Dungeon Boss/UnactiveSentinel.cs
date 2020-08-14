using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnactiveSentinel : MonoBehaviour
{
    Animator animator;
    Vector3 bossCamLocation = new Vector3(-780, 10);
    Vector3 playerLocation = new Vector3(-780, -10);
    bool activatedBoss = false;
    bool moveCameraBack = false;
    public GameObject sentinelBoss;
    Camera mainCamera;
    MoveCameraNextRoom cameraScript;
    [SerializeField] List<SentinelRotateRock> rocks;
    [SerializeField] GameObject rockStorm;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        cameraScript = mainCamera.GetComponent<MoveCameraNextRoom>();
        cameraScript.freeCam = true;
        cameraScript.trackPlayer = true;
        Camera.main.orthographicSize = 20;

        FindObjectOfType<AudioManager>().PlaySound("First Boss Background Music");
        
        Sound s = Array.Find(FindObjectOfType<AudioManager>().sounds, sound => sound.name == "Dungeon Ambiance");
        s.source.mute = true;

        StartCoroutine(bossWakeUpSequence());
    }

    IEnumerator wakeUpBoss()
    {
        animator.SetTrigger("WakeUp");
        this.GetComponents<AudioSource>()[0].Play();
        yield return new WaitForSeconds(8 / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(11f/12f);
    }

    IEnumerator bossWakeUpSequence()
    {
        LeanTween.move(mainCamera.gameObject, bossCamLocation, 2).setEaseOutCirc();
        rockStorm.SetActive(true);

        yield return new WaitForSeconds(2.1f);

        StartCoroutine(wakeUpBoss());
        foreach (SentinelRotateRock rock in rocks)
        {
            rock.rise();
        }

        yield return new WaitForSeconds(20 / 12f);

        LeanTween.move(mainCamera.gameObject, playerLocation, 2).setEaseOutCirc();

        yield return new WaitForSeconds(2f);

        mainCamera.transform.position = playerLocation;
        cameraScript.freeCam = false;
        //summonBoss;
        BossHealthBar healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.targetEnemy = sentinelBoss.GetComponent<Enemy>();
        healthBar.bossStartUp("Sentinel");
        sentinelBoss.SetActive(true);
        Destroy(this.gameObject);
    }
}
