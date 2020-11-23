using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorMakerWave : WeaponFireScript
{
    [SerializeField] GameObject smallRift, mediumRift, largeRift;

    private void Start()
    {
        StartCoroutine(waveProcedure());
        initShipPos = PlayerProperties.playerShipPosition;
        initFirePos = transform.position;
        StartCoroutine(summonRifts(pickDirectionTravel() * Mathf.Deg2Rad, transform.position));
        spriteRenderer.enabled = false;
    }

    float pickDirectionTravel()
    {
        return (360 + Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    private void Update()
    {
        transform.position = initFirePos + PlayerProperties.playerShipPosition - initShipPos;
    }

    IEnumerator summonRifts(float angleInRad, Vector3 basePosition)
    {
        List<GameObject> instants = new List<GameObject>();
        for(int i = 0; i < 3; i++)
        {
            float summonAngle = ((angleInRad * Mathf.Rad2Deg) - 15 + 15 * i) * Mathf.Deg2Rad;
            GameObject riftInstant = Instantiate(smallRift, basePosition + new Vector3(Mathf.Cos(summonAngle), Mathf.Sin(summonAngle)) * 1f, Quaternion.identity);
            riftInstant.GetComponent<PlayerProjectile>().whichWeaponFrom = whichWeapon;
            instants.Add(riftInstant);
        }
        triggerWeaponFireFlag(instants.ToArray(), basePosition, angleInRad * Mathf.Rad2Deg);

        yield return new WaitForSeconds(0.2f);

        instants.Clear();
        for (int i = 0; i < 3; i++)
        {
            float summonAngle = ((angleInRad * Mathf.Rad2Deg) - 15 + 15 * i) * Mathf.Deg2Rad;
            GameObject riftInstant = Instantiate(mediumRift, basePosition + new Vector3(Mathf.Cos(summonAngle), Mathf.Sin(summonAngle)) * 2.5f, Quaternion.identity);
            riftInstant.GetComponent<PlayerProjectile>().whichWeaponFrom = whichWeapon;
            instants.Add(riftInstant);
        }
        triggerWeaponFireFlag(instants.ToArray(), basePosition, angleInRad * Mathf.Rad2Deg);

        yield return new WaitForSeconds(0.2f);

        instants.Clear();
        for (int i = 0; i < 3; i++)
        {
            float summonAngle = ((angleInRad * Mathf.Rad2Deg) - 15 + 15 * i) * Mathf.Deg2Rad;
            GameObject riftInstant = Instantiate(largeRift, basePosition + new Vector3(Mathf.Cos(summonAngle), Mathf.Sin(summonAngle)) * 4f, Quaternion.identity);
            riftInstant.GetComponent<PlayerProjectile>().whichWeaponFrom = whichWeapon;
            instants.Add(riftInstant);
        }
        triggerWeaponFireFlag(instants.ToArray(), basePosition, angleInRad * Mathf.Rad2Deg);
    }

    // going to need some projectile firing thing here

    IEnumerator waveProcedure()
    {
        transform.localScale = new Vector3(0.1f, 0.1f);
        LeanTween.value(0.1f, 0.3f, 0.5f).setOnUpdate((float val) => { transform.localScale = new Vector3(val, val); });
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
