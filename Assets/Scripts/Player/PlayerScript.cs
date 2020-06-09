using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    //General stats/assets required for the ship
    public Sprite downLeft, left, down, up, upLeft;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    public float boatSpeed = 4;
    private Vector3 directionMove;
    public GameObject waterFoam;
    private float foamTimer = 0;
    public float angleOrientation;
    public bool stopRotate = false;
    public float whatAngleTraveled;
    public Image healthBarFill;
    public int shipHealth = 1000;
    public int trueDamage = 0;
    public int shipHealthMAX = 1000;
    public bool shipRooted = false;
    public int whichWeapon = 1;
    public float defenseModifier = 1;

    //artifact bonuses
    public float speedBonus = 0;
    public float defenseBonus = 0;
    public int attackBonus = 0;
    public int healthBonus = 0;
    public int periodicHealing = 0;

    //spawning help
    public int numRoomsSinceLastArtifact = 0;
    public int numRoomsVisited = 0;
    public bool enemiesDefeated = true;

    //restricts to using one active at a time
    public bool activeEnabled = false;
    public int angleEffect = 0;
    Vector3 damageColLocation;
    public int numberHits = 0;

    //some toggles for certain mechanics
    public bool damageImmunity = false;
    public bool damageAbsorb = false;

    //consumable bonuses
    public float conSpeedBonus = 0;
    public float conDefenseBonus = 0;
    public int conAttackBonus = 0;

    //upgrade bonuses
    public int upgradeSpeedBonus = 0, upgradeHealthBonus = 0;
    public float upgradeDefenseBonus = 0;

    //toggles whether the ship can be hit
    bool hitBufferPeriod = false;

    //If a UI menu is already open, sets to true
    public bool windowAlreadyOpen = false;

    //tool tip when examining obstacles
    public GameObject obstacleToolTip;

    //when firing certain weapons, we want to prevent the ship from moving
    //for a certain period
    public float stopRotatePeriod = 0;

    //important members used to spawn items/manage inventory
    ItemTemplates itemTemplates;
    Inventory inventory;
    Artifacts artifacts;

    //when you want to apply a force to the ship
    //eg. the cannon's momentum blast
    public Vector3 momentumVector = Vector3.zero;
    public Vector3 enemyMomentumVector = Vector3.zero;
    public float momentumMagnitude = 0;
    public float momentumDuration = 0;
    public float enemyMomentumMagnitude = 0;
    public float enemyMomentumDuration = 0;

    //debuffs
    public float enemySpeedModifier = 0;

    //hull upgrade reference
    HullUpgradeManager hullUpgradeManager;
    public GameObject spikeHitBox;

    //Death stuff
    public bool playerDead = false;
    public GameObject deathSplash, blackFadeOut, respawnEffect;
    public int numberLives = 0;
    public GameObject deathGraphic;

    public bool reApplyHealth = false;

    private AudioManager audioManager;
    private DamageNumbers damageNumbers;
    private CameraShake cameraShake;
    private PlayerHealNumbers healNumbers;

    private int healthBarTweenID;

    List<string> playerHubNames = new List<string>() { "Player Hub", "Willow's Hideout", "Ylva's Hideout" };

    private Text healthBarText;

    public void applyInventoryLoss()
    {
        float goldLoss = 0.25f;
        int numArtifactsSave = 0;

        if (PlayerUpgrades.safeUpgrades.Count == 1)
        {
            numArtifactsSave = 1;
        }
        else if (PlayerUpgrades.safeUpgrades.Count == 2)
        {
            goldLoss += 0.15f;
            numArtifactsSave = 1;
        }
        else if (PlayerUpgrades.safeUpgrades.Count == 3)
        {
            goldLoss += 0.25f;
            numArtifactsSave = 2;
        }

        PlayerItems.inventoryItemsIDs.Clear();
        for (int i = 0; i < 3; i++)
        {
            PlayerItems.activeArtifactsIDs[i] = null;
        }
        PlayerItems.totalGoldAmount = Mathf.RoundToInt(PlayerItems.totalGoldAmount * goldLoss);
        List<GameObject> numActiveArtifacts = new List<GameObject>();
        List<GameObject> numInventoryArtifacts = new List<GameObject>();
        List<GameObject> numActiveSoulBoundArtifacts = new List<GameObject>();
        List<GameObject> numInventorySoulBoundArtifacts = new List<GameObject>();

        for (int i = 0; i < inventory.itemList.Count; i++)
        {
            DisplayItem info = inventory.itemList[i].GetComponent<DisplayItem>();
            if (info.isArtifact == true)
            {
                if (info.soulBound == false)
                {
                    numInventoryArtifacts.Add(inventory.itemList[i]);
                }
                else
                {
                    numInventorySoulBoundArtifacts.Add(inventory.itemList[i]);
                }
            }
        }

        for (int i = 0; i < artifacts.activeArtifacts.Count; i++)
        {
            DisplayItem info = artifacts.activeArtifacts[i].GetComponent<DisplayItem>();
            if (info.soulBound == false)
            {
                numActiveArtifacts.Add(artifacts.activeArtifacts[i]);
            }
            else
            {
                numActiveSoulBoundArtifacts.Add(artifacts.activeArtifacts[i]);
            }
        }

        int artifactsNeededSaving = numArtifactsSave;
        if (artifactsNeededSaving > 0)
        {
            if (numActiveArtifacts.Count != 0 && numInventoryArtifacts.Count != 0)
            {
                if (numActiveArtifacts.Count == 0 && numInventoryArtifacts.Count == 1)
                {
                    PlayerItems.inventoryItemsIDs.Add(numInventoryArtifacts[0].name);
                }
                else if (numActiveArtifacts.Count == 1 && numInventoryArtifacts.Count == 0)
                {
                    PlayerItems.activeArtifactsIDs[0] = numActiveArtifacts[0].name;
                }
                else
                {
                    while (artifactsNeededSaving > 0)
                    {
                        if (Random.Range(1, 11) > 7 && numInventoryArtifacts.Count != 0)
                        {
                            GameObject savedObject = numInventoryArtifacts[Random.Range(0, numInventoryArtifacts.Count)];
                            numInventoryArtifacts.Remove(savedObject);
                            PlayerItems.inventoryItemsIDs.Add(savedObject.name);
                            artifactsNeededSaving--;
                        }
                        else
                        {
                            if (numActiveArtifacts.Count != 0)
                            {
                                GameObject savedObject = numActiveArtifacts[Random.Range(0, numActiveArtifacts.Count)];
                                numActiveArtifacts.Remove(savedObject);
                                for (int i = 0; i < 3; i++)
                                {
                                    if (PlayerItems.activeArtifactsIDs[i] == null)
                                    {
                                        PlayerItems.activeArtifactsIDs[i] = savedObject.name;
                                        break;
                                    }
                                }
                                artifactsNeededSaving--;
                            }
                        }
                    }
                }
            }
        }

        foreach (GameObject item in numInventorySoulBoundArtifacts)
        {
            PlayerItems.inventoryItemsIDs.Add(item.name);
        }

        foreach (GameObject item in numActiveSoulBoundArtifacts)
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerItems.activeArtifactsIDs[i] == null)
                {
                    PlayerItems.activeArtifactsIDs[i] = item.name;
                    break;
                }
            }
        }
    }

    IEnumerator setDeathGraphicActive(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (deathGraphic.activeSelf == false)
        {
            deathGraphic.SetActive(true);
        }
    }

    void applyMomentum()
    {
        if (momentumVector.magnitude > 0)
        {
            momentumVector -= (momentumVector.normalized * momentumMagnitude / momentumDuration * Time.deltaTime);
        }
        else
        {
            momentumVector = Vector3.zero;
        }
    }

    void applyEnemyMomentum()
    {
        if (enemyMomentumVector.magnitude > 0)
        {
            enemyMomentumVector -= (enemyMomentumVector.normalized * enemyMomentumMagnitude / enemyMomentumDuration * Time.deltaTime);
        }
        else
        {
            enemyMomentumVector = Vector3.zero;
        }
    }

    void loadPrevItems()
    {
        if (playerHubNames.Contains(SceneManager.GetActiveScene().name))
        {
            HubProperties.storeGold += PlayerItems.totalGoldAmount;
            if (GameObject.Find("Gold Deposit Notifier"))
            {
                if (PlayerItems.totalGoldAmount > 0)
                {
                    GameObject.Find("Gold Deposit Notifier").GetComponent<NotificationBell>().startNotification(PlayerItems.totalGoldAmount.ToString() + " gold deposited.");
                }
                else
                {
                    GameObject.Find("Gold Deposit Notifier").SetActive(false);
                }
            }
            PlayerItems.totalGoldAmount = 0;

            foreach (string id in PlayerItems.inventoryItemsIDs.ToArray())
            {
                if (id == "GoldItem" || id == "GoldItem(Clone)")
                {
                    PlayerItems.inventoryItemsIDs.Remove(id);
                }
            }
        }

        foreach (string item in PlayerItems.inventoryItemsIDs)
        {
            if (item != null && itemTemplates.dbContainsID(item))
            {
                GameObject loadedItem = itemTemplates.loadItem(item);
                if (loadedItem != null)
                {
                    GameObject newItem = Instantiate(loadedItem);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform; //bookkeeping
                    inventory.itemList.Add(newItem);
                }
            }
        }

        foreach (string item in PlayerItems.activeArtifactsIDs)
        {
            if (item != null)
            {
                GameObject loadedItem = itemTemplates.loadItem(item);
                if (loadedItem != null)
                {
                    GameObject newItem = Instantiate(loadedItem);
                    newItem.transform.parent = GameObject.Find("PresentItems").transform;
                    newItem.GetComponent<DisplayItem>().isEquipped = true;
                    artifacts.activeArtifacts.Add(newItem);
                }
            }
        }

        if (reApplyHealth == true)
        {
            trueDamage = PlayerItems.playerDamage;
        }
        else
        {
            PlayerItems.playerDamage = 0;
        }
        SaveSystem.SaveGame();
    }

    public void periodicHeal()
    {
        healPlayer(periodicHealing);
        healthBarFill.fillAmount = (float)shipHealth / shipHealthMAX;
    }

    IEnumerator bufferHit(float duration)
    {
        hitBufferPeriod = true;
        yield return new WaitForSeconds(duration);
        hitBufferPeriod = false;
    }

    IEnumerator hitFrame(SpriteRenderer spriteRenderer)
    {
        audioManager.PlaySound("Player Hit");
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    bool moveShip()
    {
        bool hasPressedButton = false;
        directionMove = Vector3.zero;
        if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveUp)))
        {
            directionMove.y = 1;
            hasPressedButton = true;
        }

        if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveLeft)))
        {
            directionMove.x = -1;
            hasPressedButton = true;
        }

        if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveDown)))
        {
            directionMove.y = -1;
            hasPressedButton = true;
        }

        if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveRight)))
        {
            directionMove.x = 1;
            hasPressedButton = true;
        }

        if (hasPressedButton == true)
        {
            angleEffect = (int)(360 + Mathf.Atan2(directionMove.y, directionMove.x) * Mathf.Rad2Deg) % 360;
            if (foamTimer >= 0.05f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngleTraveled + 90));
            }
            return true;
        }
        return false;
    }

    void pickSprite()
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = upLeft;
            transform.localScale = new Vector3(-0.15f, 0.15f, 0);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = up;
            transform.localScale = new Vector3(0.15f, 0.15f, 0);
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            spriteRenderer.sprite = upLeft;
            transform.localScale = new Vector3(0.15f, 0.15f, 0);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            spriteRenderer.sprite = left;
            transform.localScale = new Vector3(0.15f, 0.15f, 0);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = downLeft;
            transform.localScale = new Vector3(0.15f, 0.15f, 0);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            spriteRenderer.sprite = down;
            transform.localScale = new Vector3(0.15f, 0.15f, 0);
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            spriteRenderer.sprite = downLeft;
            transform.localScale = new Vector3(-0.15f, 0.15f, 0);
        }
        else
        {
            spriteRenderer.sprite = left;
            transform.localScale = new Vector3(-0.15f, 0.15f, 0);
        }
    }

    private void loadDebugConsoleIfNotInstantiated()
    {
        if (Debug.isDebugBuild == true)
        {
            if (FindObjectOfType<ConsoleCommands>() == null)
            {
                GameObject debugConsole = Resources.Load<GameObject>("DebugConsole");
                Instantiate(debugConsole);
            }
        }
    }

    private void Awake()
    {
        loadDebugConsoleIfNotInstantiated();
        PlayerProperties.playerScript = this;
        PlayerProperties.playerShip = this.gameObject;
    }

    void Start() {
        itemTemplates = FindObjectOfType<ItemTemplates>();
        inventory = GetComponent<Inventory>();
        artifacts = GetComponent<Artifacts>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hullUpgradeManager = GetComponent<HullUpgradeManager>();

        PlayerProperties.playerArtifacts = artifacts;
        PlayerProperties.playerInventory = inventory;
        PlayerProperties.spriteRenderer = this.spriteRenderer;

        if (SceneManager.GetActiveScene().name != "Tutorial" && SceneManager.GetActiveScene().name != "Demo Level")
        {
            loadPrevItems();
        }

        healthBarText = healthBarFill.transform.parent.GetComponentInChildren<Text>().GetComponentsInChildren<Text>()[1];

        damageNumbers = FindObjectOfType<DamageNumbers>();
        audioManager = FindObjectOfType<AudioManager>();
        healNumbers = FindObjectOfType<PlayerHealNumbers>();
        cameraShake = FindObjectOfType<CameraShake>();

        inventory.UpdateUI();
        inventory.inventory.SetActive(false);

        artifacts.UpdateUI();
        artifacts.artifactsUI.SetActive(false);

        shipHealth = Mathf.RoundToInt(shipHealthMAX - trueDamage);

        updateHealthBarText();
    }

    public void healPlayer(int amountHealing)
    {
        if (amountHealing > 0)
        {
            healNumbers.showHealing(amountHealing, shipHealthMAX);
            Mathf.Clamp(trueDamage -= amountHealing, 0, int.MaxValue);
        }

        shipHealth = Mathf.RoundToInt(shipHealthMAX - trueDamage);

        foreach (GameObject artifact in artifacts.activeArtifacts)
        {
            ArtifactEffect effect = artifact.GetComponent<ArtifactEffect>();
            if (effect != null)
            {
                effect.healed(amountHealing);
            }
        }

        updateHealthBar();
    }

    void OnGUI()
    {
        if (Time.timeScale > 0)
        {
            GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString() + "FPS");
        }
    }

    void Update() {
        if (playerDead == false)
        {
            PlayerProperties.playerShipPosition = transform.position;

            applyMomentum();
            applyEnemyMomentum();
            foamTimer += Time.deltaTime;

            if (stopRotatePeriod > 0)
            {
                stopRotate = true;
                stopRotatePeriod -= Time.deltaTime;
            }
            else
            {
                stopRotatePeriod = 0;
                stopRotate = false;
            }

            if (stopRotate == false && shipRooted == false)
            {
                angleOrientation = (360 + whatAngleTraveled) % 360;
                pickSprite();
            }

            if (moveShip() == false)
            {
                rigidBody2D.velocity = Vector3.zero + momentumVector + enemyMomentumVector;
            }
            else
            {
                if (shipRooted == false)
                {
                    rigidBody2D.velocity = directionMove * Mathf.Clamp((boatSpeed + speedBonus + conSpeedBonus + upgradeSpeedBonus + enemySpeedModifier), 0, int.MaxValue) + momentumVector + enemyMomentumVector; //speed bonus
                    PlayerProperties.currentPlayerTravelDirection = (Mathf.Atan2(directionMove.y, directionMove.x) * Mathf.Rad2Deg + 360) % 360;
                    PlayerProperties.shipTravellingVector = rigidBody2D.velocity;
                }
                else
                {
                    PlayerProperties.shipTravellingVector = Vector3.zero;
                    rigidBody2D.velocity = Vector3.zero;
                }
                whatAngleTraveled = Mathf.Atan2(directionMove.y, directionMove.x) * Mathf.Rad2Deg;
            }

            if (rigidBody2D.velocity.magnitude > 1.5f)
            {
                audioManager.UnMuteSound("Idle Ship Movement");
            }
            else
            {
                audioManager.MuteSound("Idle Ship Movement");
            }

            shipHealthMAX = 1000 + healthBonus + upgradeHealthBonus;

            shipHealth = Mathf.RoundToInt(shipHealthMAX - trueDamage);

            pickRendererLayer();
            defenseModifier = Mathf.Clamp(1 - defenseBonus - conDefenseBonus - upgradeDefenseBonus, 0.05f, float.MaxValue);
        }
        else
        {
            momentumVector = Vector3.zero;
            enemyMomentumVector = Vector3.zero;
        }

        updateHealthBar();
        updateHealthBarText();
        PlayerProperties.armorIndicator.updateShieldEffect();
    }

    public void dealDamageToShip(int amountDamage, GameObject damagingObject)
    {
        int amountBeingDamaged = amountDamage;

        if (damagingObject.GetComponent<DisplayItem>())
        {
            dealTrueDamageToShip(amountDamage);
            return;
        }

        if (damageImmunity == true)
        {
            amountBeingDamaged = 0;
        }

        if (damageAbsorb == true)
        {
            healPlayer(Mathf.RoundToInt(amountDamage * defenseModifier));
            amountBeingDamaged = 0;
        }

        foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
        {
            if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
            {
                if (damagingObject.GetComponent<ProjectileParent>())
                {
                    slot.displayInfo.GetComponent<ArtifactEffect>().tookDamage(amountDamage, damagingObject.GetComponent<ProjectileParent>().instantiater.GetComponent<Enemy>());
                }
                else if (damagingObject.transform.parent != null)
                {
                    slot.displayInfo.GetComponent<ArtifactEffect>().tookDamage(amountDamage, damagingObject.transform.parent.GetComponent<Enemy>());
                }
                else
                {
                    slot.displayInfo.GetComponent<ArtifactEffect>().tookDamage(amountDamage, damagingObject.GetComponent<Enemy>());
                }
            }
        }

        if (hitBufferPeriod == false)
        {
            float angle = Mathf.Atan2(damagingObject.transform.position.y - transform.position.y, damagingObject.transform.position.x - transform.position.x);
            if (Vector2.Distance(transform.position, damagingObject.transform.position) < 1f)
            {
                damageNumbers.showDamage((int)(amountBeingDamaged* defenseModifier), shipHealthMAX, damagingObject.transform.position);
            }
            else
            {
                damageNumbers.showDamage((int)(amountBeingDamaged * defenseModifier), shipHealthMAX, transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)));
            }

            cameraShake.shakeCamFunction(0.1f, 0.3f * ((amountDamage * defenseModifier) / shipHealthMAX));
        }
        else
        {
            return;
        }


        StartCoroutine(bufferHit(0.5f));

        if (amountBeingDamaged == 0)
        {
            return;
        }

        trueDamage += (int)(amountDamage * defenseModifier);
        PlayerItems.playerDamage = trueDamage;
        damagingObject = null;

        shipHealth = Mathf.RoundToInt(shipHealthMAX - trueDamage);

        if (shipHealth <= 0 && playerDead == false)
        {
            shipHealth = 0;
            playerDead = true;
            rigidBody2D.velocity = Vector3.zero;

            LeanTween.cancel(healthBarTweenID);

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            spriteRenderer.enabled = false;
            Instantiate(deathSplash, transform.position, Quaternion.identity);

            if (numberLives > 0)
            {
                numberLives--;
                audioManager.PlaySound("Player Death");
                StartCoroutine(respawnShip());
            }
            else
            {
                applyInventoryLoss();
                blackFadeOut.GetComponent<Animator>().SetTrigger("FadeOut");
                audioManager.PlaySound("Player Death");
                MiscData.playerDied = true;
                windowAlreadyOpen = true;
                StartCoroutine(setDeathGraphicActive(1f));
                SaveSystem.SaveGame();
            }
        }

        updateHealthBar();
    }

    public void dealTrueDamageToShip(int damage)
    {
        trueDamage += damage;
        StartCoroutine(hitFrame(spriteRenderer));
        damageNumbers.showDamage(damage, shipHealthMAX, transform.position + new Vector3(0, 1.5f, 0));
        numberHits++;

        updateHealthBar();
    }

    public float totalShipSpeed {
        get
        {
            return boatSpeed + speedBonus + conSpeedBonus + upgradeSpeedBonus + enemySpeedModifier;
        }
    }

    IEnumerator respawnShip()
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlaySound("Respawn Sound");
        Instantiate(respawnEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(6f / 12f);
        spriteRenderer.sprite = left;
        transform.localScale = new Vector3(0.15f, 0.15f, 0);
        StartCoroutine(bufferHit(1f));
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1 / 12f);
        trueDamage = 0;
        playerDead = false;
        spriteRenderer.enabled = true;
    }

    void updateHealthBar()
    {
        if (Mathf.Abs((float)shipHealth / shipHealthMAX - healthBarFill.fillAmount) > 0.01)
        {
            LeanTween.cancel(healthBarTweenID);
            healthBarTweenID = LeanTween.value(healthBarFill.fillAmount, (float)shipHealth / shipHealthMAX, 0.4f).setOnUpdate((float val) => { healthBarFill.fillAmount = val; }).id;
            updateHealthBarText();
        }
    }

    void updateHealthBarText()
    {
        healthBarText.text = (shipHealthMAX - trueDamage).ToString() + "/" + shipHealthMAX.ToString();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            collision.gameObject.tag != "MeleeEnemy"
            && collision.gameObject.tag != "RangedEnemy"
            && collision.gameObject.tag != "EnemyShield"
            && collision.gameObject.tag != "DormantEnemy"
            && collision.gameObject.tag != "StrongEnemy"
            && collision.gameObject.name != "AntiSpawnSpaceDetailer"
            && collision.gameObject.tag != "RoomSpawn"
            && playerDead == false
            && damageImmunity == false
            && collision.gameObject.layer != 21
           )
        {
            StartCoroutine(hitFrame(spriteRenderer));
            damageColLocation = collision.transform.position;
            numberHits++;

            if (hullUpgradeManager.spikesEnabled)
            {
                StartCoroutine(spikeTick());
            }
        }
    }

    IEnumerator spikeTick()
    {
        Collider2D[] cols = spikeHitBox.GetComponents<Collider2D>();

        if (spriteRenderer.sprite == downLeft)
        {
            cols[0].enabled = true;
        }
        else if (spriteRenderer.sprite == left)
        {
            cols[1].enabled = true;
        }
        else if (spriteRenderer.sprite == down)
        {
            cols[2].enabled = true;
        }
        else if (spriteRenderer.sprite == up)
        {
            cols[3].enabled = true;
        }
        else
        {
            cols[4].enabled = true;
        }

        yield return new WaitForSeconds(0.2f);

        foreach(Collider2D col in cols)
        {
            if(col.enabled == true)
            {
                col.enabled = false;
            }
        }
    }
}
