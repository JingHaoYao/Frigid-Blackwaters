using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluxShrineUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    [SerializeField] GameObject regularLightBall1, regularLightBall2, spreadBall1, spreadBall2, spreadBall3, wavesBall1, wavesBall2, wavesBall3;
    GameObject empoweredWeaponFlare;
    [SerializeField] GameObject summonExplosionYellow, summonExplosionGreen;
    private SpriteRenderer weaponScriptRenderer, explosionRenderer;
    GameObject summoningExplosionInstant;

    int prevNumberUpgrades;
    float origCoolDownTime;

    [SerializeField] float maxPolluxTimer = 5;
    private float polluxTimer;

    bool isBlinking = false;

    void applyUpgrades()
    {
        GameObject explosionToSummon = summonExplosionYellow;
        weaponTemplate.weaponFlare = regularLightBall1;
        if (PlayerUpgrades.polluxShrineUpgrades.Count == 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
        }
        else if (PlayerUpgrades.polluxShrineUpgrades.Count == 2)
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
            weaponTemplate.weaponFlare = regularLightBall2;
        }
        else if (PlayerUpgrades.polluxShrineUpgrades.Count > 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.polluxShrineUpgrades[3] == "unlock_waves_light_balls")
            {
                explosionToSummon = summonExplosionGreen;
                if (PlayerUpgrades.polluxShrineUpgrades.Count == 4)
                {
                    empoweredWeaponFlare = wavesBall1;
                }
                else if (PlayerUpgrades.polluxShrineUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = wavesBall2;
                }
                else
                {
                    empoweredWeaponFlare = wavesBall3;
                }
            }
            else
            {
                if (PlayerUpgrades.polluxShrineUpgrades.Count == 4)
                {
                    empoweredWeaponFlare = spreadBall1;
                }
                else if (PlayerUpgrades.polluxShrineUpgrades.Count == 5)
                {
                    empoweredWeaponFlare = spreadBall2;
                }
                else
                {
                    empoweredWeaponFlare = spreadBall3;
                }
            }
            weaponTemplate.weaponFlare = empoweredWeaponFlare;
        }
        else
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
        }
        Destroy(summoningExplosionInstant);
        summoningExplosionInstant = Instantiate(explosionToSummon, weaponScript.transform.position + Vector3.up * 0.75f, Quaternion.identity);
        explosionRenderer = summoningExplosionInstant.GetComponent<SpriteRenderer>();
    }

    IEnumerator blinkExplosionIndicator()
    {
        while (isBlinking)
        {
            explosionRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            explosionRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.polluxShrineUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScriptRenderer = weaponScript.gameObject.GetComponent<SpriteRenderer>();
        weaponScript.setTemplate();
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.polluxShrineUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.polluxShrineUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if (!weaponScript.isOnCooldown())
        {
            if (polluxTimer < maxPolluxTimer)
            {
                polluxTimer += Time.deltaTime;
            }
            else
            {
                polluxTimer = maxPolluxTimer;
            }
            summoningExplosionInstant.transform.localScale = new Vector3(3 * whatPolluxTimerToReturn(), 3 * whatPolluxTimerToReturn());
            if(whatPolluxTimerToReturn() == 1 && isBlinking == false)
            {
                isBlinking = true;
                StartCoroutine(blinkExplosionIndicator());
            }
        }
        else
        {
            summoningExplosionInstant.transform.localScale = Vector3.zero;
            if (polluxTimer != 0)
            {
                isBlinking = false;
                polluxTimer = 0;
                explosionRenderer.color = Color.white;
            }
        }
        summoningExplosionInstant.transform.position = weaponScript.transform.position + Vector3.up * 0.75f;
        explosionRenderer.sortingOrder = weaponScriptRenderer.sortingOrder + 10;
    }

    float whatPolluxTimerToReturn()
    {
        if(Mathf.Abs(polluxTimer - maxPolluxTimer) < 0.001)
        {
            return 1;
        }
        else
        {
            return Mathf.Clamp(polluxTimer / maxPolluxTimer, 0.1f, 1);
        }
    }

    public override GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume)
    {
        GameObject instant = Instantiate(weaponPlume, weaponScript.transform.position + Vector3.up * 0.3f, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        instant.GetComponent<PolluxLightBall>().Initialize(whatPolluxTimerToReturn());
        return instant;
    }

    private void OnDestroy()
    {
        Destroy(summoningExplosionInstant);
    }
}
