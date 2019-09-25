using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpPortal : MonoBehaviour {
    public GameObject yesIndicator, noIndicator, examineIndicator;
    bool toolTipActive = false;
    Animator animator;
    GameObject playerShip;
    Text text;
    public GameObject obstacleToolTip;
    GameObject spawnedYI, spawnedNI, spawnedIndicator;
    bool warped = false;

    IEnumerator warpPlayerShip()
    {
        animator.SetTrigger("Activated");
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(11f / 12f);
        AntiSpawnSpaceDetailer[] rooms = FindObjectsOfType<AntiSpawnSpaceDetailer>();
        Vector3 selectedRoom = rooms[Random.Range(0, rooms.Length)].gameObject.transform.position;
        Camera.main.transform.position = selectedRoom;
        playerShip.transform.position = selectedRoom;
        animator.SetTrigger("Stagnant");
        warped = false;
    }

    void Start () {
        animator = GetComponent<Animator>();
        playerShip = GameObject.Find("PlayerShip");
        text = GetComponent<Text>();
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
    }

	void Update () {
		if(Vector2.Distance(transform.position, playerShip.transform.position) < 3 && playerShip.GetComponent<PlayerScript>().enemiesDefeated == true && warped == false)
        {
            if (toolTipActive == false)
            {
                if (spawnedIndicator == null)
                {
                    spawnedIndicator = Instantiate(examineIndicator, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
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
                    warped = true;
                    obstacleToolTip.SetActive(false);
                    toolTipActive = false;
                    GameObject.Find("PlayerShip").GetComponent<PlayerScript>().shipRooted = false;
                    Destroy(spawnedYI);
                    Destroy(spawnedNI);
                    StartCoroutine(warpPlayerShip());
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
                toolTipActive = false;
                Destroy(spawnedIndicator);
            }
        }
	}
}
