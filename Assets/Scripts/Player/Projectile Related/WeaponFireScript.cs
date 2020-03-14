using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponFireScript : MonoBehaviour {
    public GameObject bullet;
    public Animator animator;
    public AnimationClip weaponFire;
    public float animLength = 0;
    public Vector3 initShipPos, initFirePos;
    public SpriteRenderer spriteRenderer;
    PlayerScript playerScript;
    public bool forceFired = false;
    public int whichWeapon;

    public UnityAction fireAction;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    IEnumerator waitForAudio()
    {
        yield return new WaitForSeconds(animLength / 3f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        animLength = weaponFire.length;
        StartCoroutine(waitForAudio());
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

        triggerWeaponFireFlag(new GameObject[1] { instant }, transform.position, pickDirectionTravel());
    }

    float pickDirectionTravel()
    {
        Vector3 cursorPosition = PlayerProperties.cursorPosition;
        return (360 + Mathf.Atan2(cursorPosition.y - transform.position.y, cursorPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    public void triggerWeaponFireFlag(GameObject[] instants, Vector3 whichPositionFiredFrom, float angleTravel)
    {
        if (whichWeapon == 1)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedFrontWeapon(instants, whichPositionFiredFrom, angleTravel);
            }
        }
        else if (whichWeapon == 2)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedLeftWeapon(instants, whichPositionFiredFrom, angleTravel);
            }
        }
        else
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedRightWeapon(instants, whichPositionFiredFrom, angleTravel);
            }
        }
    }

	void Update () {
        transform.position = initFirePos + (GameObject.Find("PlayerShip").transform.position - initShipPos);
        pickRendererLayer();
	}
}
