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

    void Start()
    {
        animator = GetComponent<Animator>();
        Camera.main.GetComponent<MoveCameraNextRoom>().freeCam = true;
        Camera.main.GetComponent<MoveCameraNextRoom>().trackPlayer = true;
        Camera.main.orthographicSize = 20;
        FindObjectOfType<AudioManager>().PlaySound("First Boss Background Music");
        
        Sound s = Array.Find(FindObjectOfType<AudioManager>().sounds, sound => sound.name == "Dungeon Ambiance");
        s.source.mute = true;
    }

    IEnumerator wakeUpBoss()
    {
        animator.SetTrigger("WakeUp");
        this.GetComponents<AudioSource>()[0].Play();
        yield return new WaitForSeconds(8 / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(11f/12f);
        moveCameraBack = true;
    }

    void Update()
    {
        if (moveCameraBack == false)
        {
            if (Vector2.Distance(Camera.main.transform.position, bossCamLocation) > 0.3f)
            {
                Camera.main.transform.position += (bossCamLocation - Camera.main.transform.position).normalized * 15 * Time.deltaTime;
            }
            else
            {
                if (activatedBoss == false)
                {
                    Camera.main.transform.position = bossCamLocation;
                    StartCoroutine(wakeUpBoss());
                    foreach(SentinelRotateRock rock in FindObjectsOfType<SentinelRotateRock>())
                    {
                        rock.rise();
                    }
                    activatedBoss = true;
                }
            }
        }
        else
        {
            if (Vector2.Distance(Camera.main.transform.position, playerLocation) > 0.3f)
            {
                Camera.main.transform.position += (playerLocation - Camera.main.transform.position).normalized * 15 * Time.deltaTime;
            }
            else
            {
                Camera.main.transform.position = playerLocation;
                Camera.main.GetComponent<MoveCameraNextRoom>().freeCam = false;
                //summonBoss;
                FindObjectOfType<BossHealthBar>().targetEnemy = sentinelBoss.GetComponent<Enemy>();
                FindObjectOfType<BossHealthBar>().bossStartUp("Sentinel");
                sentinelBoss.SetActive(true);
                Destroy(this.gameObject);
            }
        }

    }
}
