using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GambleTowers : MonoBehaviour {
    GameObject playerShip, spawnedIndicator;
    public GameObject obstacleToolTip, indicator;
    bool toolTipActive = false;
    Text text;
    bool towersActivated = false;
    PlayerScript playerScript;
    public GameObject ring;

    IEnumerator activateTowers()
    {
        ChildTower[] childTowers = GetComponentsInChildren<ChildTower>();
        playerScript.shipRooted = true;
        int loseWin = Random.Range(0, 2);
        if(loseWin == 0)
        {
            foreach (ChildTower element in childTowers)
            {
                element.activateWin();
            }
        }
        else
        {
            foreach(ChildTower element in childTowers)
            {
                element.activateLose();
            }
        }
        yield return new WaitForSeconds(11f / 12f);
        this.GetComponents<AudioSource>()[0].Play();
        this.GetComponents<AudioSource>()[1].Play();

        if (loseWin == 0)
        {
            playerScript.trueDamage = 0;
        }
        else
        {
            playerScript.trueDamage += (playerScript.shipHealthMAX - playerScript.trueDamage) / 2;
        }
        yield return new WaitForSeconds(16 / 12f);

        foreach (ChildTower element in childTowers)
        {
            element.activateDormant();
        }
        playerScript.shipRooted = false;
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        text = this.GetComponent<Text>();
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
        playerScript = playerShip.GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 5f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && towersActivated == false)
        {
            if (toolTipActive == false)
            {
                if (spawnedIndicator == null)
                {
                    spawnedIndicator = Instantiate(indicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
                }
            }
            else
            {
                if (spawnedIndicator != null)
                {
                    Destroy(spawnedIndicator);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (obstacleToolTip.activeSelf == true)
                {
                    obstacleToolTip.SetActive(false);
                    toolTipActive = false;
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                }
                else
                {
                    toolTipActive = true;
                    obstacleToolTip.GetComponentInChildren<Text>().text = text.text;
                    obstacleToolTip.SetActive(true);
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = true;
                }
            }
        }
        else
        {
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }

        if(Vector2.Distance(playerShip.transform.position, transform.position) < 1f && towersActivated == false)
        {
            towersActivated = true;
            StartCoroutine(activateTowers());
            ring.GetComponent<Animator>().SetTrigger("Disperse");
            Destroy(ring, 0.417f);
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }
    }
}
