using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour {
    //General stats/assets required for the ship
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
    private Image redBarImage;
    public int shipHealth = 1000;
    public int shipHealthMAX = 1000;
    public int whichWeapon = 1;
    public float defenseModifier = 1;

    [SerializeField] Animator animator;
    [SerializeField] Animator shadowAnimator;
    private int previousAnimationOrientation = -1;

    List<ShipWeaponScript> allShipWeaponScripts = new List<ShipWeaponScript>();

    UnityAction deathOverrideAction;
    bool deathOverride = false;

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
    Vector3 damageColLocation;
    public int numberHits = 0;

    //some toggles for certain mechanics
    List<GameObject> itemsGrantingDamageImmunity = new List<GameObject>();
    private bool shipRooted = false;
    public bool damageAbsorb = false;

    //consumable bonuses
    public float conSpeedBonus = 0;
    public float conDefenseBonus = 0;
    public int conAttackBonus = 0;
    public int conHealthBonus = 0;

    //upgrade bonuses
    public int upgradeSpeedBonus = 0, upgradeHealthBonus = 0;
    public float upgradeDefenseBonus = 0;

    private bool shipMoving = false;

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
    Vector3 momentumVector = Vector3.zero;
    Vector3 enemyMomentumVector = Vector3.zero;
    float momentumMagnitude = 0;
    float momentumDuration = 0;
    float enemyMomentumMagnitude = 0;
    float enemyMomentumDuration = 0;

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

    Coroutine healthBarTweenInstance;
    float targetHealthBarFill = 1;
    Coroutine flashWhiteRoutine;

    List<string> playerHubNames = new List<string>() { "Player Hub", "Willow's Hideout", "Ylva's Hideout", "Nymph Village", "Surtr's Springs" };

    private Text healthBarText;
    private bool isInPlayerHub;

    public void OverrideDeathAction(UnityAction unityEvent)
    {
        deathOverrideAction = unityEvent;
        deathOverride = true;
    }

    public bool IsInPlayerHub()
    {
        return playerHubNames.Contains(SceneManager.GetActiveScene().name);
    }

    public List<ShipWeaponScript> GetShipWeaponScripts()
    {
        return allShipWeaponScripts;
    }

    public void RegisterWeaponScript(ShipWeaponScript script)
    {
        allShipWeaponScripts.Add(script);
    }

    public void SetAnimationShipType(int whatShipType)
    {
        animator.SetInteger("ShipType", whatShipType);
        animator.SetTrigger("Left");
        transform.localScale = new Vector3(-2.6f, 2.6f);
    }

    public void setPlayerMomentum(Vector3 momentumVector, float duration)
    {
        this.momentumVector = momentumVector;
        momentumMagnitude = momentumVector.magnitude;
        this.momentumDuration = duration;
    }

    public void setPlayerEnemyMomentum(Vector3 momentumVector, float duration)
    {
        this.enemyMomentumDuration = duration;
        this.enemyMomentumVector = momentumVector;
        this.enemyMomentumMagnitude = momentumVector.magnitude;
    }

    public bool isShipMoving()
    {
        return shipMoving;
    }

    public bool isShipRooted()
    {
        return shipRooted;
    }

    public void addRootingObject()
    {
        shipRooted = true;
    }

    public void removeRootingObject()
    {
        shipRooted = false;
    }

    public void addImmunityItem(GameObject item)
    {
        if (!itemsGrantingDamageImmunity.Contains(item))
        {
            itemsGrantingDamageImmunity.Add(item);
        }
    }

    public void removeImmunityItem(GameObject item)
    {
        if (itemsGrantingDamageImmunity.Contains(item))
        {
            itemsGrantingDamageImmunity.Remove(item);
        }
    }

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
            goldLoss += 2.6f;
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

    public void loadPrevItems()
    {
        if (playerHubNames.Contains(SceneManager.GetActiveScene().name)) {
            HubProperties.storeGold += PlayerItems.totalGoldAmount;

            FindObjectOfType<ReturnNotifications>().updateGoldDeposited(PlayerItems.totalGoldAmount);

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
                    newItem.GetComponent<ArtifactEffect>()?.artifactEquipped();
                    artifacts.activeArtifacts.Add(newItem);
                }
            }
        }

        inventory.UpdateUI();

        artifacts.UpdateUI();

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
        for (int i = 0; i < 3; i++)
        {
            redBarImage.enabled = true;
            spriteRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            redBarImage.enabled = false;
            spriteRenderer.material.color = Color.white;
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
            if (foamTimer >= 0.05f)
            {
                foamTimer = 0;
                float angle = Mathf.Atan2(directionMove.y, directionMove.x);
                float yOffset = 0;
                float magnitude = 1.25f;

                if(directionMove.y > 0)
                {   
                    yOffset = -0.5f;   
                }
                else if (directionMove.y < 0)
                {
                    if (Mathf.Abs(directionMove.x) > 0)
                    {
                        yOffset = -0.5f;
                        magnitude = 1;
                    }
                }
                else
                {
                    yOffset = -0.25f;
                }

                Instantiate(waterFoam, transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) + yOffset) * magnitude, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90));
            }
            return true;
        }
        return false;
    }

    void pickSprite()
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            if (previousAnimationOrientation != 1)
            {
                previousAnimationOrientation = 1;
                shadowAnimator.SetTrigger("UpLeft");
                animator.SetTrigger("UpLeft");
                transform.localScale = new Vector3(-2.6f, 2.6f, 0);
            }
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            if (previousAnimationOrientation != 2)
            {
                previousAnimationOrientation = 2;
                shadowAnimator.SetTrigger("Up");
                animator.SetTrigger("Up");
                transform.localScale = new Vector3(2.6f, 2.6f, 0);
            }
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            if (previousAnimationOrientation != 3)
            {
                previousAnimationOrientation = 3;
                shadowAnimator.SetTrigger("UpLeft");
                animator.SetTrigger("UpLeft");
                transform.localScale = new Vector3(2.6f, 2.6f, 0);
            }
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            if (previousAnimationOrientation != 4)
            {
                previousAnimationOrientation = 4;
                shadowAnimator.SetTrigger("Left");
                animator.SetTrigger("Left");
                transform.localScale = new Vector3(2.6f, 2.6f, 0);
            }
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            if (previousAnimationOrientation != 5)
            {
                previousAnimationOrientation = 5;
                shadowAnimator.SetTrigger("DownLeft");
                animator.SetTrigger("DownLeft");
                transform.localScale = new Vector3(2.6f, 2.6f, 0);
            }
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            if (previousAnimationOrientation != 6)
            {
                previousAnimationOrientation = 6;
                shadowAnimator.SetTrigger("Down");
                animator.SetTrigger("Down");
                transform.localScale = new Vector3(2.6f, 2.6f, 0);
            }
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            if (previousAnimationOrientation != 7)
            {
                previousAnimationOrientation = 7;
                shadowAnimator.SetTrigger("DownLeft");
                animator.SetTrigger("DownLeft");
                transform.localScale = new Vector3(-2.6f, 2.6f, 0);
            }
        }
        else
        {
            if (previousAnimationOrientation != 8)
            {
                previousAnimationOrientation = 8;
                shadowAnimator.SetTrigger("Left");
                animator.SetTrigger("Left");
                transform.localScale = new Vector3(-2.6f, 2.6f, 0);
            }
        }
    }

    IEnumerator SleepHit(float duration)
    {
        Time.timeScale = 0.001f;

        yield return new WaitForSeconds(duration * Time.timeScale);

        Time.timeScale = 1;
    }


    IEnumerator flashWhite(float duration)
    {
        float timer = 0;

        float increment = 1 / (duration / 2);

        while(timer < duration / 2)
        {
            timer += Time.deltaTime;
            spriteRenderer.material.SetFloat("_FlashAmount", timer * increment);
            yield return null;
        }

        timer = duration / 2;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.material.SetFloat("_FlashAmount", timer * increment);
            yield return null;
        }
    }

    public void FlashWhitePickup()
    {
        if(flashWhiteRoutine != null)
        {
            StopCoroutine(flashWhiteRoutine);
        }

        flashWhiteRoutine = StartCoroutine(flashWhite(0.5f));
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

        healthBarText = healthBarFill.transform.parent.GetComponentInChildren<Text>().GetComponentsInChildren<Text>()[1];
    }

    void Start() {
        LeanTween.init(1000);
        itemTemplates = FindObjectOfType<ItemTemplates>();
        inventory = GetComponent<Inventory>();
        artifacts = GetComponent<Artifacts>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hullUpgradeManager = GetComponent<HullUpgradeManager>();

        
        redBarImage = healthBarFill.GetComponentsInChildren<Image>()[1];
       
        redBarImage.enabled = false;
        
        PlayerProperties.spriteRenderer = this.spriteRenderer;

        if (SceneManager.GetActiveScene().name != "Tutorial" && SceneManager.GetActiveScene().name != "Demo Level")
        {
            if (!playerHubNames.Contains(SceneManager.GetActiveScene().name))
            {
                loadPrevItems();
                isInPlayerHub = false;
            }
            else
            {
                isInPlayerHub = true;
            }
        }

        damageNumbers = FindObjectOfType<DamageNumbers>();
        audioManager = FindObjectOfType<AudioManager>();
        healNumbers = FindObjectOfType<PlayerHealNumbers>();
        cameraShake = FindObjectOfType<CameraShake>();

        artifacts.artifactsUI.SetActive(false);
        inventory.inventory.SetActive(false);

        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            shipHealth = shipHealthMAX;
        }

        CheckAndUpdateHealth();
    }

    void updateShipHealthMAX()
    {
        shipHealthMAX = 1000 + healthBonus + upgradeHealthBonus + conHealthBonus;
    }

    public void healPlayer(int amountHealing)
    {
        if (amountHealing > 0)
        {
            healNumbers.showHealing(amountHealing, shipHealthMAX);
            shipHealth += amountHealing;

            if(shipHealth > shipHealthMAX)
            {
                shipHealth = shipHealthMAX;
            }
        }

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
        PlayerProperties.playerShipPosition = transform.position;

        if (playerDead == false)
        {
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

            if (stopRotate == false)
            {
                angleOrientation = (360 + whatAngleTraveled) % 360;
                pickSprite();
            }

            if (moveShip() == false)
            {
                shipMoving = false;
                rigidBody2D.velocity = Vector3.zero + momentumVector + enemyMomentumVector;
            }
            else
            {
                shipMoving = true;
                if (shipRooted == false)
                {
                    rigidBody2D.velocity = directionMove * Mathf.Clamp((boatSpeed + speedBonus + conSpeedBonus + upgradeSpeedBonus + enemySpeedModifier), 1, int.MaxValue) + momentumVector + enemyMomentumVector; //speed bonus
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

            updateShipHealthMAX();

            pickRendererLayer();
            defenseModifier = Mathf.Clamp(1 - defenseBonus - conDefenseBonus - upgradeDefenseBonus, 0.05f, float.MaxValue);
        }
        else
        {
            momentumVector = Vector3.zero;
            enemyMomentumVector = Vector3.zero;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
#endif

        updateHealthBar();
        updateHealthBarText();
        PlayerProperties.armorIndicator.updateShieldEffect();
    }

    public void CheckAndUpdateHealth()
    {
        updateShipHealthMAX();
        if(shipHealth > shipHealthMAX)
        {
            shipHealth = shipHealthMAX;
        }
        updateHealthBar();
    }

    public void dealDamageToShip(int amountDamage, GameObject damagingObject)
    {
        int amountBeingDamaged = Mathf.RoundToInt(amountDamage * defenseModifier);

        if (damagingObject != null)
        {
            if (damagingObject.GetComponent<DisplayItem>())
            {
                dealTrueDamageToShip(amountDamage);
                return;
            }

            if (itemsGrantingDamageImmunity.Count > 0)
            {
                amountBeingDamaged = 0;
            }

            if (damageAbsorb == true)
            {
                healPlayer(Mathf.RoundToInt(amountBeingDamaged));
                amountBeingDamaged = 0;
            }
            // need to correct here

            if (hitBufferPeriod == false)
            {
                float angle = Mathf.Atan2(damagingObject.transform.position.y - transform.position.y, damagingObject.transform.position.x - transform.position.x);
                if (Vector2.Distance(transform.position, damagingObject.transform.position) < 1f)
                {
                    damageNumbers.showDamage((int)(amountBeingDamaged), shipHealthMAX, damagingObject.transform.position);
                }
                else
                {
                    damageNumbers.showDamage((int)(amountBeingDamaged), shipHealthMAX, transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)));
                }

                cameraShake.shakeCamFunction(0.2f, 0.1f + ((float)amountBeingDamaged / shipHealthMAX) * 0.2f);
            }
            else
            {
                return;
            }
        }

        StartCoroutine(SleepHit(0.07f));

        if (amountBeingDamaged == 0)
        {
            return;
        }

        StartCoroutine(bufferHit(0.5f));

        shipHealth -= amountBeingDamaged;

        if (damagingObject != null)
        {
            foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                {
                    if (damagingObject.GetComponent<ProjectileParent>())
                    {
                        ProjectileParent projectileParent = damagingObject.GetComponent<ProjectileParent>();
                        if (projectileParent.instantiater != null)
                        {
                            slot.displayInfo.GetComponent<ArtifactEffect>().tookDamage(amountBeingDamaged, damagingObject.GetComponent<ProjectileParent>().instantiater?.GetComponent<Enemy>());
                        }
                    }
                    else if (damagingObject.transform.parent != null)
                    {
                        slot.displayInfo.GetComponent<ArtifactEffect>().tookDamage(amountBeingDamaged, damagingObject.transform.parent.GetComponent<Enemy>());
                    }
                    else
                    {
                        slot.displayInfo.GetComponent<ArtifactEffect>().tookDamage(amountBeingDamaged, damagingObject.GetComponent<Enemy>());
                    }
                }
            }

            foreach (ShipWeaponScript script in allShipWeaponScripts)
            {
                if (damagingObject.GetComponent<ProjectileParent>())
                {
                    ProjectileParent projectileParent = damagingObject.GetComponent<ProjectileParent>();
                    if (projectileParent.instantiater != null)
                    {
                        script.shipWeaponTemplate.GetComponent<WeaponFireTemplate>().TookDamage(amountBeingDamaged, projectileParent.instantiater.GetComponent<Enemy>());
                    }
                }
                else if (damagingObject.transform.parent != null)
                {
                    script.shipWeaponTemplate.GetComponent<WeaponFireTemplate>().TookDamage(amountBeingDamaged, damagingObject.transform.parent.GetComponent<Enemy>());
                }
                else
                {
                    script.shipWeaponTemplate.GetComponent<WeaponFireTemplate>().TookDamage(amountBeingDamaged, damagingObject.GetComponent<Enemy>());
                }
            }
        }

        damagingObject = null;

        CheckPlayerDeath();

        updateHealthBar();
    }

    void CheckPlayerDeath()
    {
        if (shipHealth <= 0 && playerDead == false)
        {
            shipHealth = 0;
            playerDead = true;
            rigidBody2D.velocity = Vector3.zero;

            if (healthBarTweenInstance != null)
            {
                StopCoroutine(healthBarTweenInstance);
            }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            spriteRenderer.enabled = false;
            Instantiate(deathSplash, transform.position, Quaternion.identity);

            foreach (ArtifactSlot slot in PlayerProperties.playerArtifacts.artifactSlots)
            {
                if (slot.displayInfo != null)
                {
                    slot.displayInfo.GetComponent<ArtifactEffect>()?.playerDied();
                }
            }

            audioManager.PlaySound("Player Death");

            if (!deathOverride)
            {
                if (numberLives > 0)
                {
                    numberLives--;
                    StartCoroutine(respawnShip());
                }
                else
                {
                    applyInventoryLoss();
                    blackFadeOut.GetComponent<Animator>().SetTrigger("FadeOut");
                    MiscData.playerDied = true;
                    windowAlreadyOpen = true;
                    StartCoroutine(setDeathGraphicActive(1f));
                    SaveSystem.SaveGame();
                }
            }
            else
            {
                deathOverrideAction.Invoke();
            }
        }
    }

    public void dealTrueDamageToShip(int damage)
    {
        shipHealth -= damage;
        StartCoroutine(hitFrame(spriteRenderer));
        damageNumbers.showDamage(damage, shipHealthMAX, transform.position + new Vector3(0, 1.5f, 0));
        numberHits++;

        updateHealthBar();

        CheckPlayerDeath();
    }

    public float totalShipSpeed {
        get
        {
            return boatSpeed + speedBonus + conSpeedBonus + upgradeSpeedBonus + enemySpeedModifier;
        }
    }

    public void SetShipBackToNormal()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        animator.SetTrigger("Left");
        shadowAnimator.SetTrigger("Left");
        transform.localScale = new Vector3(2.6f, 2.6f, 0);
        shipHealth = shipHealthMAX;
        playerDead = false;
        spriteRenderer.enabled = true;
    }

    IEnumerator respawnShip()
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlaySound("Respawn Sound");
        Instantiate(respawnEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(6f / 12f);
        animator.SetTrigger("Left");
        shadowAnimator.SetTrigger("Left");
        transform.localScale = new Vector3(2.6f, 2.6f, 0);
        StartCoroutine(bufferHit(1f));
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1 / 12f);
        shipHealth = shipHealthMAX;
        playerDead = false;
        spriteRenderer.enabled = true;
    }

    void updateHealthBar()
    {
        if (Mathf.Abs(((float)shipHealth / shipHealthMAX) - targetHealthBarFill) > 0.0001f)
        {
            if (playerHubNames.Contains(SceneManager.GetActiveScene().name))
            {
                shipHealth = shipHealthMAX;
            }
            targetHealthBarFill = (float)shipHealth / shipHealthMAX;
            if (healthBarTweenInstance != null)
            {
                StopCoroutine(healthBarTweenInstance);
            }

            healthBarTweenInstance = StartCoroutine(healthBarTween(healthBarFill.fillAmount, targetHealthBarFill, 0.4f));
            updateHealthBarText();
        }
    }

    IEnumerator healthBarTween(float currentHealthBarFillAmount, float newHealthBarFillAmount, float time)
    {
        float increment = (newHealthBarFillAmount - currentHealthBarFillAmount) / time;

        float timer = 0;

        bool adjustFlammableUI = PlayerProperties.flammableController != null;

        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            healthBarFill.fillAmount = currentHealthBarFillAmount + timer * increment;
            if (adjustFlammableUI)
            {
                PlayerProperties.flammableController.UpdateFireyBar();
            }
            yield return null;
        }
    }

    void updateHealthBarText()
    {
        healthBarText.text = shipHealth.ToString() + "/" + shipHealthMAX.ToString();
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
            && itemsGrantingDamageImmunity.Count == 0
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

        if (previousAnimationOrientation == 5 || previousAnimationOrientation == 7)
        {
            cols[0].enabled = true;
        }
        else if (previousAnimationOrientation == 4 || previousAnimationOrientation == 8)
        {
            cols[1].enabled = true;
        }
        else if (previousAnimationOrientation == 6)
        {
            cols[2].enabled = true;
        }
        else if (previousAnimationOrientation == 2)
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
