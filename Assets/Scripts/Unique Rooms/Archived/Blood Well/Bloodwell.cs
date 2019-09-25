using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bloodwell : MonoBehaviour {
    Animator animator;
    GameObject playerShip, spawnedIndicator;
    public GameObject obstacleToolTip, indicator;
    bool toolTipActive = false;
    Text text;
    public int sacrificeHealth;
    public GameObject yesIndicator, noIndicator;
    GameObject spawnedYI, spawnedNI;
    Chest artifactChest;
    bool sacrificedHealth = false;
    bool openedChest = false;

    IEnumerator exposeChest()
    {
        animator.SetTrigger("Activated");
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.75f);
        artifactChest.enabled = true;
        artifactChest.extraArtifactChance = 100;
        artifactChest.betterArtifactChance = sacrificeHealth % 100 * 2;
        animator.SetTrigger("ChestClosed");
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        text = this.GetComponent<Text>();
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
        sacrificeHealth = 100 * Random.Range(1, 7);
        artifactChest = this.GetComponent<Chest>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Vector2.Distance(playerShip.transform.position, transform.position) < 3f && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && sacrificedHealth == false)
        {
            if (toolTipActive == false)
            {
                if (spawnedIndicator == null)
                {
                    spawnedIndicator = Instantiate(indicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                    spawnedIndicator.GetComponent<ExamineIndicator>().parentObject = this.gameObject;
                    Destroy(spawnedYI);
                    Destroy(spawnedNI);
                }
            }
            else
            {
                if (spawnedIndicator != null)
                {
                    Destroy(spawnedIndicator);
                    spawnedYI = Instantiate(yesIndicator, transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
                    spawnedNI = Instantiate(noIndicator, transform.position + new Vector3(1, 1, 0), Quaternion.identity);
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    sacrificedHealth = true;
                    obstacleToolTip.SetActive(false);
                    toolTipActive = false;
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().trueDamage += sacrificeHealth;
                    Destroy(spawnedYI);
                    Destroy(spawnedNI);
                    StartCoroutine(exposeChest());
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    obstacleToolTip.SetActive(false);
                    toolTipActive = false;
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                    Destroy(spawnedYI);
                    Destroy(spawnedNI);
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
                    obstacleToolTip.GetComponentInChildren<Text>().text = text.text + sacrificeHealth + " health?";
                    obstacleToolTip.SetActive(true);
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = true;
                }
            }
        }
        else
        {
            if(spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }
        }

        if(Vector2.Distance(playerShip.transform.position, transform.position) < 2.5f && Input.GetKeyDown(KeyCode.F) && artifactChest.enabled == true)
        {
            if(openedChest == false)
            {
                openedChest = true;
                animator.SetTrigger("ChestOpened");
            }
        }
    }
}
