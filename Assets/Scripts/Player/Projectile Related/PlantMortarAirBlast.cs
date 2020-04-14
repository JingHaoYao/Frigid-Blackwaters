using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMortarAirBlast : WeaponFireScript
{
    [SerializeField] bool spreadShotMortar;
    [SerializeField] int numberSpreads = 0;
    [SerializeField] GameObject spreadMortarProjectile;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    IEnumerator waitForAudio()
    {
        yield return new WaitForSeconds(animLength);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animLength = weaponFire.length;
        StartCoroutine(waitForAudio());
        initShipPos = PlayerProperties.playerShipPosition;
        initFirePos = transform.position;
        fireMortarBall();
    }

    void fireMortarBall()
    {
        List<GameObject> mortarBullets = new List<GameObject>();
        GameObject mortarBallInstant = Instantiate(bullet, transform.position, Quaternion.identity);
        mortarBallInstant.GetComponent<PlantMortarProjectile>().targetLocation = PlayerProperties.cursorPosition;
        mortarBullets.Add(mortarBallInstant);
        if (spreadShotMortar)
        {
            Vector3 basePos = PlayerProperties.cursorPosition;
            float angleIncrement = 360 / numberSpreads;
            for(int i = 0; i < numberSpreads; i++)
            {
                GameObject spreadMortarInstant = Instantiate(spreadMortarProjectile, transform.position, Quaternion.identity);
                spreadMortarInstant.GetComponent<PlantMortarProjectile>().targetLocation = basePos + (new Vector3(Mathf.Cos(i * angleIncrement * Mathf.Deg2Rad), Mathf.Sin(i * angleIncrement * Mathf.Deg2Rad)) * 2);
                mortarBullets.Add(spreadMortarInstant);

            }
        }
        triggerWeaponFireFlag(mortarBullets.ToArray(), transform.position, pickDirectionTravel());
    }

    float pickDirectionTravel()
    {
        Vector3 cursorPosition = PlayerProperties.cursorPosition;
        return (360 + Mathf.Atan2(cursorPosition.y - transform.position.y, cursorPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Update()
    {
        transform.position = initFirePos + (PlayerProperties.playerShipPosition - initShipPos);
        pickRendererLayer();
    }
}
