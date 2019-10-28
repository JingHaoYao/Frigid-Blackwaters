using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipWeaponScript : MonoBehaviour {
    public int whichSide = 1; //1 - front of ship, 2 - left side of ship, 3 - right side of ship
    public Sprite up, upleft, left, downleft, down;
    GameObject playerShip;
    PlayerScript playerScript;
    SpriteRenderer spriteRenderer;
    float angleOrientation;
    public GameObject musketSmoke;
    public float coolDownPeriod = 0;
    public float coolDownThreshold = 5;
    public bool onCooldown = false;
    public Image fillIcon, weaponIcon;
    public GameObject shipWeaponTemplate;
    ShipWeaponTemplate template;
    public int numberShots = 0;
    GameObject weaponTemplate;
    public bool noFireNormally = false, adminStopFire;
    public bool mouseHovering = false;
    public GameObject pointer;
    CursorTarget cursorTarget;

    public void swapTemplate(ShipWeaponTemplate newTemplate)
    {
        Destroy(weaponTemplate);
        shipWeaponTemplate = newTemplate.gameObject;
        weaponTemplate = Instantiate(newTemplate.gameObject);
        weaponTemplate.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped = this.gameObject;
    }

    public void setTemplate()
    {
        weaponTemplate.GetComponent<ShipWeaponTemplate>().shipWeaponEquipped = this.gameObject;
        ShipWeaponTemplate trueTemplate = weaponTemplate.GetComponent<ShipWeaponTemplate>();
        up = trueTemplate.up;
        upleft = trueTemplate.upleft;
        left = trueTemplate.left;
        downleft = trueTemplate.downleft;
        down = trueTemplate.down;
        coolDownThreshold = trueTemplate.coolDownTime;
        musketSmoke = trueTemplate.weaponFlare;
        weaponIcon.sprite = trueTemplate.coolDownIcon;
    }

    void setPosition()
    {
        if (whichSide == 1)
        {
            spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                transform.position = playerShip.transform.position + new Vector3(0.9f, 1.4f, 0) * 0.65f;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                transform.position = playerShip.transform.position + new Vector3(0, 2f, 0) * 0.65f;
                spriteRenderer.sprite = up;
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.9f, 1.4f, 0) * 0.65f;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                transform.position = playerShip.transform.position + new Vector3(-1.5f, 0.4f, 0) * 0.65f;
                spriteRenderer.sprite = left;
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.5f, -0.1f, 0) * 0.65f;
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                transform.position = playerShip.transform.position + new Vector3(0, -0.5f, 0) * 0.65f;
                spriteRenderer.sprite = down;
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                transform.position = playerShip.transform.position + new Vector3(0.5f, -0.1f, 0) * 0.65f;
                spriteRenderer.sprite = downleft;
            }
            else
            {
                transform.position = playerShip.transform.position + new Vector3(1.5f, 0.4f, 0) * 0.65f;
                spriteRenderer.sprite = left;
            }
        }
        else if (whichSide == 2)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                transform.position = playerShip.transform.position + new Vector3(0.6f, 0.4f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                transform.position = playerShip.transform.position + new Vector3(0.5f, 0.6f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(-0.7f, 0.7f, 0);
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                transform.position = playerShip.transform.position + new Vector3(0.1f, 0.7f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.4f, 0.5f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = up;
                transform.localScale = new Vector3(0.7f, 0.7f, 0);
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.3f, 0.4f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.5f, 0.3f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(0.7f, 0.7f, 0);
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.1f, 0.3f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                transform.localScale = new Vector3(-0.7f, 0.7f, 0);
                spriteRenderer.sprite = downleft;
            }
            else
            {
                transform.position = playerShip.transform.position + new Vector3(0.4f, 0.2f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = down;
                transform.localScale = new Vector3(0.7f, 0.7f, 0);
            }
        }
        else
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.1f, 0.7f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = upleft;
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.5f, 0.6f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(0.7f, 0.7f, 0);
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.6f, 0.4f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder - 3;
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                transform.position = playerShip.transform.position + new Vector3(-0.4f, 0.2f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = down;
                transform.localScale = new Vector3(0.7f, 0.7f, 0);
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                transform.position = playerShip.transform.position + new Vector3(0.1f, 0.3f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                transform.localScale = new Vector3(-0.7f, 0.7f, 0);
                spriteRenderer.sprite = downleft;
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                transform.position = playerShip.transform.position + new Vector3(0.5f, 0.3f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = left;
                transform.localScale = new Vector3(-0.7f, 0.7f, 0);
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                transform.position = playerShip.transform.position + new Vector3(0.3f, 0.4f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = upleft;
                transform.localScale = new Vector3(0.7f, 0.7f, 0);
            }
            else
            {
                transform.position = playerShip.transform.position + new Vector3(0.4f, 0.5f, 0) * 0.65f;
                spriteRenderer.sortingOrder = playerShip.GetComponent<SpriteRenderer>().sortingOrder + 3;
                spriteRenderer.sprite = up;
                transform.localScale = new Vector3(0.7f, 0.7f, 0);
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
                if (angleOrientation > 15 && angleOrientation <= 75)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 225));
                }
                else if (angleOrientation > 75 && angleOrientation <= 105)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
                }
                else if (angleOrientation > 105 && angleOrientation <= 165)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 315));
                }
                else if (angleOrientation > 165 && angleOrientation <= 195)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
                }
                else if (angleOrientation > 195 && angleOrientation <= 255)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 45));
                }
                else if (angleOrientation > 255 && angleOrientation <= 285)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
                }
                else if (angleOrientation > 285 && angleOrientation <= 345)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 135));
                }
                else
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
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
                }
            }
            else if (whichSide == 2)
            {
                if (angleOrientation > 15 && angleOrientation <= 75)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.196f, -0.21f, 0), Quaternion.Euler(0, 0, 135));
                }
                else if (angleOrientation > 75 && angleOrientation <= 105)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
                }
                else if (angleOrientation > 105 && angleOrientation <= 165)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.619f, 0.24f, 0), Quaternion.Euler(0, 0, 200));
                }
                else if (angleOrientation > 165 && angleOrientation <= 195)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
                }
                else if (angleOrientation > 195 && angleOrientation <= 255)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 315));
                }
                else if (angleOrientation > 255 && angleOrientation <= 285)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
                }
                else if (angleOrientation > 285 && angleOrientation <= 345)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 45));
                }
                else
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
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
                }
            }
            else if (whichSide == 3)
            {
                if (angleOrientation > 15 && angleOrientation <= 75)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.639f, 0.24f, 0), Quaternion.Euler(0, 0, 340));
                }
                else if (angleOrientation > 75 && angleOrientation <= 105)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
                }
                else if (angleOrientation > 105 && angleOrientation <= 165)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.189f, -0.15f, 0), Quaternion.Euler(0, 0, 45));
                }
                else if (angleOrientation > 165 && angleOrientation <= 195)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
                }
                else if (angleOrientation > 195 && angleOrientation <= 255)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 135));
                }
                else if (angleOrientation > 255 && angleOrientation <= 285)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
                }
                else if (angleOrientation > 285 && angleOrientation <= 345)
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 225));
                }
                else
                {
                    instant = Instantiate(musketSmoke, transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
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
                }
            }
        }
    }

    public void forceFire()
    {
        GameObject instant;
        if (whichSide == 1)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 225));
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 315));
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 45));
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 135));
            }
            else
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
            }
            coolDownPeriod = coolDownThreshold;
            onCooldown = true;
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
            }
        }
        else if (whichSide == 2)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.196f, -0.21f, 0), Quaternion.Euler(0, 0, 135));
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.619f, 0.24f, 0), Quaternion.Euler(0, 0, 200));
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 315));
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 45));
            }
            else
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
            }
            coolDownPeriod = coolDownThreshold;
            onCooldown = true;
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
            }
        }
        else if (whichSide == 3)
        {
            if (angleOrientation > 15 && angleOrientation <= 75)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.639f, 0.24f, 0), Quaternion.Euler(0, 0, 340));
            }
            else if (angleOrientation > 75 && angleOrientation <= 105)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.55f, -.05f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (angleOrientation > 105 && angleOrientation <= 165)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(-0.189f, -0.15f, 0), Quaternion.Euler(0, 0, 45));
            }
            else if (angleOrientation > 165 && angleOrientation <= 195)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0, -0.55f, 0), Quaternion.Euler(0, 0, 90));
            }
            else if (angleOrientation > 195 && angleOrientation <= 255)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.379f, -0.34f, 0), Quaternion.Euler(0, 0, 135));
            }
            else if (angleOrientation > 255 && angleOrientation <= 285)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.55f, -.05f, 0), Quaternion.Euler(0, 0, 180));
            }
            else if (angleOrientation > 285 && angleOrientation <= 345)
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0.339f, 0.24f, 0), Quaternion.Euler(0, 0, 225));
            }
            else
            {
                instant = Instantiate(musketSmoke, transform.position + new Vector3(0, 0.42f, 0), Quaternion.Euler(0, 0, 270));
            }
            coolDownPeriod = coolDownThreshold;
            onCooldown = true;
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
            }
        }
    }


	void Start () {
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        template = shipWeaponTemplate.GetComponent<ShipWeaponTemplate>();
        cursorTarget = FindObjectOfType<CursorTarget>();
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
