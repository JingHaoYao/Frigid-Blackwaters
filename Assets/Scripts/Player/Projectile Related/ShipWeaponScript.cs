using UnityEngine;
using UnityEngine.UI;

public class ShipWeaponScript : MonoBehaviour {
    public int whichSide = 1; //1 - front of ship, 2 - left side of ship, 3 - right side of ship
    public Sprite up, upleft, left, downleft, down;
    GameObject playerShip;
    PlayerScript playerScript;
    SpriteRenderer spriteRenderer;
    float angleOrientation;
    public GameObject weaponPlume;
    public float coolDownPeriod = 0;
    public float coolDownThreshold = 5;
    public bool onCooldown = false;
    public Image fillIcon, weaponIcon;
    public GameObject shipWeaponTemplate;
    ShipWeaponTemplate template;
    public int numberShots = 0;
    public bool noFireNormally = false, adminStopFire;
    public bool mouseHovering = false;
    public GameObject pointer;
    CursorTarget cursorTarget;
    public Text weaponNumberText;

    public void swapTemplate(ShipWeaponTemplate newTemplate, bool destroy = true)
    {
        if (destroy)
        {
            Destroy(shipWeaponTemplate);
        }
        shipWeaponTemplate = newTemplate.gameObject;
        shipWeaponTemplate = Instantiate(newTemplate.gameObject);
        shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped = this.gameObject;
    }

    public void setTemplate()
    {
        shipWeaponTemplate.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped = this.gameObject;
        ShipWeaponTemplate trueTemplate = shipWeaponTemplate.GetComponent<ShipWeaponTemplate>();
        up = trueTemplate.up;
        upleft = trueTemplate.upleft;
        left = trueTemplate.left;
        downleft = trueTemplate.downleft;
        down = trueTemplate.down;
        coolDownThreshold = trueTemplate.coolDownTime;
        weaponPlume = trueTemplate.weaponFlare;
        weaponIcon.sprite = trueTemplate.coolDownIcon;
        fillIcon.fillAmount = 1;
        weaponNumberText = weaponIcon.GetComponentInChildren<Text>();
        noFireNormally = false;
        shipWeaponTemplate.GetComponent<WeaponFireTemplate>().InitializeTextIcon(weaponNumberText);
    }

    public void setCoolDownPeriod(float coolDown)
    {
        this.coolDownPeriod = coolDown;
    }

    public bool isOnCooldown()
    {
        return onCooldown;
    }

    void setPosition()
    {
        if (whichSide == 1)
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 4;
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                transform.position = playerShip.transform.position + new Vector3(0.9f, 1.7f, 0) * 0.7f;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                transform.position = playerShip.transform.position + new Vector3(0, 2.3f, 0) * 0.65f;
                spriteRenderer.sprite = up;
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.9f, 1.5f, 0) * 0.7f;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                transform.position = playerShip.transform.position + new Vector3(-1.5f, 0.7f, 0) * 0.65f;
                spriteRenderer.sprite = left;
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.5f, 0.2f, 0) * 0.65f;
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                transform.position = playerShip.transform.position + new Vector3(0, -0.2f, 0) * 0.65f;
                spriteRenderer.sprite = down;
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                transform.position = playerShip.transform.position + new Vector3(0.5f, 0.2f, 0) * 0.65f;
                spriteRenderer.sprite = downleft;
            }
            else
            {
                transform.position = playerShip.transform.position + new Vector3(1.5f, 0.7f, 0) * 0.65f;
                spriteRenderer.sprite = left;
            }
        }
        else if (whichSide == 2)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                transform.position = playerShip.transform.position + new Vector3(0.6f, 0.7f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                transform.position = playerShip.transform.position + new Vector3(0.5f, 0.9f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(-0.0403f, 0.0403f, 0);
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                transform.position = playerShip.transform.position + new Vector3(0.1f, 1.0f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.4f, 0.9f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = up;
                transform.localScale = new Vector3(0.0403f, 0.0403f, 0);
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.3f, 0.7f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.5f, 0.6f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(0.0403f, 0.0403f, 0);
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.1f, 0.6f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                transform.localScale = new Vector3(-0.0403f, 0.0403f, 0);
                spriteRenderer.sprite = downleft;
            }
            else
            {
                transform.position = playerShip.transform.position + new Vector3(0.4f, 0.5f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = down;
                transform.localScale = new Vector3(0.0403f, 0.0403f, 0);
            }
        }
        else
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.1f, 1.0f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.5f, 0.9f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(0.0403f, 0.0403f, 0);
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.6f, 0.7f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.4f, 0.5f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = down;
                transform.localScale = new Vector3(0.0403f, 0.0403f, 0);
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                transform.position = playerShip.transform.position + new Vector3(0.1f, 0.6f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                transform.localScale = new Vector3(-0.0403f, 0.0403f, 0);
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                transform.position = playerShip.transform.position + new Vector3(0.5f, 0.6f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(-0.0403f, 0.0403f, 0);
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                transform.position = playerShip.transform.position + new Vector3(0.3f, 0.7f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = upleft;
                transform.localScale = new Vector3(0.0403f, 0.0403f, 0);
            }
            else
            {
                transform.position = playerShip.transform.position + new Vector3(0.4f, 0.8f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 2;
                spriteRenderer.sprite = up;
                transform.localScale = new Vector3(0.0403f, 0.0403f, 0);
            }
        }
    }

    void fireWeapon()
    {
        GameObject instant;
        if (mouseHovering == true && Input.GetMouseButtonDown(0))
        {
            if (whichSide == 1)
            {
                instant = shipWeaponTemplate.GetComponent<WeaponFireTemplate>().fireWeapon(whichSide, angleOrientation, weaponPlume);

                if (instant == null)
                {
                    return;
                }

                coolDownPeriod = coolDownThreshold;
                onCooldown = true;
                numberShots += 1;

                if (instant.GetComponent<WeaponFireScript>())
                {
                    instant.GetComponent<WeaponFireScript>().whichWeapon = 1;
                    instant.GetComponent<WeaponFireScript>().forceFired = false;
                }
                else if (instant.GetComponent<PlayerProjectile>())
                {
                    instant.GetComponent<PlayerProjectile>().whichWeaponFrom = 1;
                    instant.GetComponent<PlayerProjectile>().forceShot = false;
                    triggerArtifactFlags(instant);
                }
            }
            else if (whichSide == 2)
            {
                instant = shipWeaponTemplate.GetComponent<WeaponFireTemplate>().fireWeapon(whichSide, angleOrientation, weaponPlume);

                if (instant == null)
                {
                    return;
                }

                coolDownPeriod = coolDownThreshold;
                onCooldown = true;
                numberShots += 1;
                if (instant.GetComponent<WeaponFireScript>())
                {
                    instant.GetComponent<WeaponFireScript>().whichWeapon = 2;
                    instant.GetComponent<WeaponFireScript>().forceFired = false;
                }
                else if (instant.GetComponent<PlayerProjectile>())
                {
                    instant.GetComponent<PlayerProjectile>().whichWeaponFrom = 2;
                    instant.GetComponent<PlayerProjectile>().forceShot = false;
                    triggerArtifactFlags(instant);
                }
            }
            else if (whichSide == 3)
            {
                instant = shipWeaponTemplate.GetComponent<WeaponFireTemplate>().fireWeapon(whichSide, angleOrientation, weaponPlume);

                if (instant == null)
                {
                    return;
                }

                coolDownPeriod = coolDownThreshold;
                onCooldown = true;
                numberShots += 1;

                if (instant.GetComponent<WeaponFireScript>())
                {
                    instant.GetComponent<WeaponFireScript>().whichWeapon = 3;
                    instant.GetComponent<WeaponFireScript>().forceFired = false;
                }
                else if (instant.GetComponent<PlayerProjectile>())
                {
                    instant.GetComponent<PlayerProjectile>().whichWeaponFrom = 3;
                    instant.GetComponent<PlayerProjectile>().forceShot = false;
                    triggerArtifactFlags(instant);
                }
            }
        }
    }

    public void forceFire()
    {
        GameObject instant;
        if (whichSide == 1)
        {
            coolDownPeriod = coolDownThreshold;
            onCooldown = true;
            instant = shipWeaponTemplate.GetComponent<WeaponFireTemplate>().fireWeapon(whichSide, angleOrientation, weaponPlume);
            numberShots += 1;
            if (instant.GetComponent<WeaponFireScript>())
            {
                instant.GetComponent<WeaponFireScript>().whichWeapon = 1;
                instant.GetComponent<WeaponFireScript>().forceFired = true;
            }
            else if (instant.GetComponent<PlayerProjectile>())
            {
                instant.GetComponent<PlayerProjectile>().whichWeaponFrom = 1;
                instant.GetComponent<PlayerProjectile>().forceShot = true;
                triggerArtifactFlags(instant);
            }
        }
        else if (whichSide == 2)
        {
            coolDownPeriod = coolDownThreshold;
            onCooldown = true;
            instant = shipWeaponTemplate.GetComponent<WeaponFireTemplate>().fireWeapon(whichSide, angleOrientation, weaponPlume);
            numberShots += 1;
            if (instant.GetComponent<WeaponFireScript>())
            {
                instant.GetComponent<WeaponFireScript>().whichWeapon = 2;
                instant.GetComponent<WeaponFireScript>().forceFired = true;
            }
            else if (instant.GetComponent<PlayerProjectile>())
            {
                instant.GetComponent<PlayerProjectile>().whichWeaponFrom = 2;
                instant.GetComponent<PlayerProjectile>().forceShot = true;
                triggerArtifactFlags(instant);
            }
        }
        else if (whichSide == 3)
        {
            coolDownPeriod = coolDownThreshold;
            onCooldown = true;
            instant = shipWeaponTemplate.GetComponent<WeaponFireTemplate>().fireWeapon(whichSide, angleOrientation, weaponPlume);
            numberShots += 1;
            if (instant.GetComponent<WeaponFireScript>())
            {
                instant.GetComponent<WeaponFireScript>().whichWeapon = 3;
                instant.GetComponent<WeaponFireScript>().forceFired = true;
            }
            else if (instant.GetComponent<PlayerProjectile>())
            {
                instant.GetComponent<PlayerProjectile>().whichWeaponFrom = 3;
                instant.GetComponent<PlayerProjectile>().forceShot = true;
                triggerArtifactFlags(instant);
            }
        }
    }

    public void triggerArtifactFlags(GameObject instant)
    {
        Vector3 cursorPosition = PlayerProperties.cursorPosition;
        float angle = (360 + Mathf.Atan2(cursorPosition.y - transform.position.y, cursorPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;

        if (whichSide == 1)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedFrontWeapon(new GameObject[] { instant }, instant.transform.position, angle);
            }
        }
        else if (whichSide == 2)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedLeftWeapon(new GameObject[] { instant }, instant.transform.position, angle);
            }
        }
        else
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedRightWeapon(new GameObject[] { instant }, instant.transform.position, angle);
            }
        }
    }

	void Start () {

        playerShip = GameObject.Find("PlayerShip");
        playerScript = PlayerProperties.playerScript;
        spriteRenderer = GetComponent<SpriteRenderer>();
        template = shipWeaponTemplate.GetComponent<ShipWeaponTemplate>();
        cursorTarget = FindObjectOfType<CursorTarget>();
        setShipWeaponScript();
        playerScript.RegisterWeaponScript(this);
    }

    void setShipWeaponScript()
    {
        switch (whichSide)
        {
            case 1:
                PlayerProperties.frontWeapon = this;
                break;
            case 2:
                PlayerProperties.leftWeapon = this;
                break;
            case 3:
                PlayerProperties.rightWeapon = this;
                break;
        }
    }

	void Update () {
        angleOrientation = playerScript.angleOrientation;
        setPosition();

        if (onCooldown == true)
        {
            if (coolDownPeriod > 0)
            {
                coolDownPeriod -= Time.deltaTime;
            }
            else
            {
                coolDownPeriod = 0;
                onCooldown = false;
            }
        }
        fillIcon.fillAmount = coolDownPeriod / coolDownThreshold;

        if (onCooldown == false && noFireNormally == false && adminStopFire == false && playerScript.playerDead == false && playerScript.windowAlreadyOpen == false)
        {
            fireWeapon();
        }

        if (mouseHovering)
        {
            pointer.SetActive(true);
        }
        else
        {
            pointer.SetActive(false);
        }
	}
}
