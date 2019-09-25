using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianShrine : MonoBehaviour {
    Chest chest;
    bool activated = false;
    bool wokenUp = false;
    public GameObject[] guardianGolems;
    bool middlePos = false;
    bool middleOfMovement = false, roomDone = false;
    AntiSpawnSpaceDetailer anti;

    void pickRendererLayer()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 200 - (int)((transform.position.y + transform.parent.transform.position.y) * 10);
    }

    IEnumerator waitForWakeUp()
    {
        foreach (GameObject guardianGolem in guardianGolems)
        {
            guardianGolem.GetComponent<GuardianGolem>().activateGolem();
            guardianGolem.tag = "RangedEnemy";
        }
        yield return new WaitForSeconds(9f / 12f);
        wokenUp = true;
    }

    IEnumerator waitForMovementAndFiring(float secondsWait)
    {
        yield return new WaitForSeconds(secondsWait);
        middleOfMovement = false;
    }

	void Start () {
        chest = GetComponent<Chest>();
        anti = transform.parent.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
	}
	
	void Update () {
        if (chest.chestItems[0] == null && activated == false)
        {
            activated = true;
            StartCoroutine(waitForWakeUp());
            anti.trialDefeated = false;
            anti.spawnDoorSeals();
        }

        if(wokenUp == true && middleOfMovement == false && roomDone == false)
        {
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
            int cw = Random.Range(0, 2);
            middleOfMovement = true;
            if(middlePos == false)
            {
                if (Random.Range(0, 2) == 1)
                {
                    if (cw == 1)
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 1;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 2;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    StartCoroutine(waitForMovementAndFiring(13f / 6f + 10f / 12f + 0.1f));
                }
                else
                {
                    if (cw == 1)
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 3;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 4;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    middlePos = true;
                    StartCoroutine(waitForMovementAndFiring(6.5f / 6f + 10f / 12f + 0.1f));
                }
            }
            else
            {
                if (Random.Range(0, 2) == 1)
                {
                    if (cw == 1)
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 5;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 6;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    StartCoroutine(waitForMovementAndFiring(9.1924f / 6f + 10f / 12f + 0.1f));
                }
                else
                {
                    if (cw == 1)
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 3;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject guardianGolem in guardianGolems)
                        {
                            if (guardianGolem != null)
                            {
                                guardianGolem.GetComponent<GuardianGolem>().whatMovement = 4;
                                guardianGolem.GetComponent<GuardianGolem>().isMoving = true;
                            }
                        }
                    }
                    StartCoroutine(waitForMovementAndFiring(6.5f / 6f + 10f / 12f + 0.1f));
                    middlePos = false;
                }
            }

            if(guardianGolems[0] == null && guardianGolems[1] == null && guardianGolems[2] == null && guardianGolems[3] == null)
            {
                roomDone = true;
                anti.trialDefeated = true;
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
            }
        }
        pickRendererLayer();
	}
}
