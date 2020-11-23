using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmeltingLaserUpgradeManager : WeaponFireTemplate
{
    ShipWeaponScript weaponScript;
    ShipWeaponTemplate weaponTemplate;
    private float origCoolDownTime;
    private int prevNumberUpgrades;
    [SerializeField] GameObject smeltingLaser;
    [SerializeField] GameObject plume;
    [SerializeField] GameObject metalChunk;
    bool upgradeBeam = true;

    int currentFragments = 0;
    int currentTier = 0;

    [SerializeField] Sprite[] upgradeSprites;
    [SerializeField] GameObject indicatorArrow;
    private GameObject arrowInstant;
    private SpriteRenderer arrowRenderer;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool focus = false;
    [SerializeField] Sprite tier2Upgrade, tier3Upgrade;

    int[] fragmentsPerTier = { 6, 8 };

    void applyUpgrades()
    {
        upgradeBeam = false;
        if (PlayerUpgrades.smeltingLaserUpgrades.Count >= 3)
        {
            weaponTemplate.coolDownTime = Mathf.Round(origCoolDownTime * 0.85f * 100f) / 100f;
            if (PlayerUpgrades.smeltingLaserUpgrades.Count > 3)
            {
                if (!(PlayerUpgrades.smeltingLaserUpgrades[3] == "focusing_laser_upgrade"))
                {
                    if(PlayerUpgrades.smeltingLaserUpgrades.Count == 4)
                    {
                        fragmentsPerTier = new int[] { 14, 16 };
                    }
                    else if(PlayerUpgrades.smeltingLaserUpgrades.Count == 5)
                    {
                        fragmentsPerTier = new int[] { 10, 12 };
                    }
                    else
                    {
                        fragmentsPerTier = new int[] { 6, 8 };
                    }
                    upgradeBeam = true;
                    weaponScript.weaponNumberText.enabled = true;
                    weaponScript.weaponNumberText.text = currentFragments.ToString() + "/" + fragmentsPerTier[currentTier].ToString();
                }
            }
        }
        else if (PlayerUpgrades.smeltingLaserUpgrades.Count <= 2)
        {
            weaponTemplate.coolDownTime = origCoolDownTime;
        }
    }

    public void SpawnMetalChunk(Vector3 position)
    {
        if (currentTier < 2 && upgradeBeam)
        {
            Instantiate(metalChunk, position, Quaternion.identity);
        }
    }

    public void addFragment()
    {
        if (currentTier < 2 && upgradeBeam)
        {
            currentFragments++;
            if (currentFragments >= fragmentsPerTier[currentTier])
            {
                currentTier++;
                weaponTemplate.up = upgradeSprites[currentTier];
                weaponTemplate.upleft = upgradeSprites[currentTier];
                weaponTemplate.left = upgradeSprites[currentTier];
                weaponTemplate.downleft = upgradeSprites[currentTier];
                weaponTemplate.down = upgradeSprites[currentTier];
                if (currentTier == 1)
                {
                    weaponTemplate.coolDownIcon = tier2Upgrade;
                }
                else
                {
                    weaponTemplate.coolDownIcon = tier3Upgrade;
                }
                currentFragments = 0;
                weaponScript.setTemplate();
            }

            if (currentTier < 2)
            {
                weaponScript.weaponNumberText.text = currentFragments.ToString() + "/" + fragmentsPerTier[currentTier].ToString();
            }
        }
    }

    void Start()
    {
        prevNumberUpgrades = PlayerUpgrades.smeltingLaserUpgrades.Count;
        weaponScript = this.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped.GetComponent<ShipWeaponScript>();
        weaponTemplate = GetComponent<ShipWeaponTemplate>();
        origCoolDownTime = weaponTemplate.coolDownTime;
        applyUpgrades();
        weaponScript.setTemplate();
        currentFragments = 0;
        arrowInstant = Instantiate(indicatorArrow, PlayerProperties.cursorPosition, Quaternion.identity);
        arrowRenderer = arrowInstant.GetComponent<SpriteRenderer>();
        arrowRenderer.enabled = false;
    }

    public override void InitializeTextIcon(Text text)
    {
        text.enabled = upgradeBeam && currentTier < 2;
        weaponScript.noFireNormally = true;
    }

    void Update()
    {
        if (prevNumberUpgrades != PlayerUpgrades.smeltingLaserUpgrades.Count)
        {
            prevNumberUpgrades = PlayerUpgrades.smeltingLaserUpgrades.Count;
            applyUpgrades();
            weaponScript.setTemplate();
        }

        if(weaponScript.onCooldown == false && weaponScript.mouseHovering && PlayerProperties.playerScript.playerDead == false && PlayerProperties.playerScript.windowAlreadyOpen == false)
        {
            if(Input.GetMouseButtonDown(0))
            {
                startPos = PlayerProperties.cursorPosition;
                arrowInstant.transform.position = startPos;
                arrowRenderer.enabled = true;
                focus = true;
            }
        }

        if (focus)
        {
            if (Input.GetMouseButton(0))
            {
                float angleToCursor = Mathf.Atan2(PlayerProperties.cursorPosition.y - arrowInstant.transform.position.y, PlayerProperties.cursorPosition.x - arrowInstant.transform.position.x);
                arrowInstant.transform.rotation = Quaternion.Euler(0, 0, angleToCursor * Mathf.Rad2Deg);
                arrowRenderer.size = new Vector3(Mathf.Clamp(Vector2.Distance(PlayerProperties.cursorPosition, arrowInstant.transform.position) / 3, 0, (5 + currentTier * 1.5f) / 3), 0.36f);
                endPos = startPos + new Vector3(Mathf.Cos(angleToCursor), Mathf.Sin(angleToCursor)) * Mathf.Clamp(Vector2.Distance(PlayerProperties.cursorPosition, arrowInstant.transform.position), 0, 5 + currentTier * 1.5f);

                if (Vector2.Distance(startPos, endPos) < 1.5f)
                {
                    arrowRenderer.color = new Color(1, 1, 1, 0.35f);
                }
                else
                {
                    arrowRenderer.color = Color.white;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Vector2.Distance(startPos, endPos) >= 1.5f)
                {
                    fireLaser(startPos, endPos);
                }
                focus = false;
                LeanTween.alpha(arrowInstant, 0, 0.5f).setOnComplete(() => { arrowRenderer.enabled = false; });
            }
        }
    }

    public void fireLaser(Vector3 startPosition, Vector3 toPosition)
    {
        weaponScript.onCooldown = true;
        weaponScript.coolDownPeriod = weaponScript.coolDownThreshold * (1 - 0.3f * currentTier);
        weaponScript.numberShots++;

        GameObject plumeInstant = Instantiate(plume, weaponScript.transform.position + Vector3.up, Quaternion.Euler(0, 0, 90));
        if (PlayerUpgrades.smeltingLaserUpgrades.Contains("focusing_laser_upgrade"))
        {
            plumeInstant.GetComponent<Animator>().Play("Blue Laser Plume");
        }
        LeanTween.value(0, 1, 0.4f).setOnUpdate((float val) => { plumeInstant.transform.position = weaponScript.transform.position + Vector3.up; });

        int damageBonus = currentTier * 5;
        float timePeriod = Vector2.Distance(toPosition, startPosition) / (8 + currentTier * 2);

        GameObject laserInstant = Instantiate(smeltingLaser, startPosition, Quaternion.identity);
        laserInstant.GetComponent<PlayerProjectile>().whichWeaponFrom = weaponScript.whichSide;
        laserInstant.GetComponent<SmeltingLaserProjectile>().Initialize(startPosition, toPosition, timePeriod, damageBonus, this);
        weaponScript.triggerArtifactFlags(laserInstant);
    }

    public override GameObject fireWeapon(int whichSide, float angleOrientation, GameObject weaponPlume)
    {
        return null;
    }
}
