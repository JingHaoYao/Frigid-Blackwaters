using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFireScript : MonoBehaviour {
    public GameObject bullet;
    Animator animator;
    public AnimationClip weaponFire;
    float animLength = 0;
    Vector3 initShipPos, initFirePos;
    SpriteRenderer spriteRenderer;
    PlayerScript playerScript;
    public bool forceFired = false;
    public int whichWeapon;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        animLength = weaponFire.length;
        Destroy(this.gameObject, animLength/3f);
        if (playerScript.stopRotatePeriod == 0)
        {
            playerScript.stopRotatePeriod += animLength / 3f;
        }
        else if (playerScript.stopRotatePeriod < animLength / 3f)
        {
            playerScript.stopRotatePeriod = animLength / 3f;
        }
        initShipPos = GameObject.Find("PlayerShip").transform.position;
        initFirePos = transform.position;
        GameObject instant = Instantiate(bullet, transform.position, Quaternion.identity);
        if (instant.GetComponent<PlayerProjectile>())
        {
            instant.GetComponent<PlayerProjectile>().whichWeaponFrom = whichWeapon;
        }

        if (instant.GetComponent<CannonRound>())
        {
            instant.GetComponent<CannonRound>().forceShot = forceFired;
        }

        if (whichWeapon == 1)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedFrontWeapon(new GameObject[1] { instant });
            }
        }
        else if (whichWeapon == 2)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedLeftWeapon(new GameObject[1] { instant });
            }
        }
        else
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedRightWeapon(new GameObject[1] { instant });
            }
        }
    }

	void Update () {
        transform.position = initFirePos + (GameObject.Find("PlayerShip").transform.position - initShipPos);
        pickRendererLayer();
	}
}
