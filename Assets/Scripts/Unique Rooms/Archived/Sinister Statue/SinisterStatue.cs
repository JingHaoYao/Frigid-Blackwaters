using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinisterStatue : MonoBehaviour {
    Animator animator;
    GameObject playerShip, spawnedIndicator;
    public GameObject obstacleToolTip, indicator;
    bool toolTipActive = false;
    Text text;
    public GameObject yesIndicator, noIndicator;
    GameObject spawnedYI, spawnedNI;
    Chest artifactChest;
    bool sacrificedHealth = false;
    int sacrificeHealth = 0;
    public GameObject chest, particles;

    void summonParticles()
    {
      Instantiate(particles, playerShip.transform.position, Quaternion.identity);
    }

    IEnumerator summonChest()
    {
        animator.SetTrigger("Activated");
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(6f / 12f);
        Instantiate(chest, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        yield return new WaitForSeconds(11f / 12f);
        animator.SetTrigger("Stagnant");
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        text = this.GetComponent<Text>();
        sacrificeHealth = Random.Range(2, 6) * 100;
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
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
                    StartCoroutine(summonChest());
                    summonParticles();
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
                    obstacleToolTip.GetComponentInChildren<Text>().text = text.text + sacrificeHealth + " health.";
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
    }
}
