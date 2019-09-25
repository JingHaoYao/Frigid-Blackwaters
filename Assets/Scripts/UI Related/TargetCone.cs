using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCone : MonoBehaviour
{
    public Color frontColor, leftColor, rightColor;
    GameObject playerShip;
    PlayerScript playerScript;
    public ShipWeaponScript frontWeapon, leftWeapon, rightWeapon;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        playerShip = playerScript.gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (SavedKeyBindings.targetConesEnabled == true)
        {
            transform.position = playerShip.transform.position;
            if (frontWeapon.mouseHovering == true)
            {
                transform.rotation = Quaternion.Euler(0, 0, playerScript.angleOrientation);
                spriteRenderer.enabled = true;
                spriteRenderer.color = frontColor;
            }
            else if (leftWeapon.mouseHovering == true)
            {
                transform.rotation = Quaternion.Euler(0, 0, playerScript.angleOrientation - 90);
                spriteRenderer.enabled = true;
                spriteRenderer.color = leftColor;
            }
            else if (rightWeapon.mouseHovering == true)
            {
                transform.rotation = Quaternion.Euler(0, 0, playerScript.angleOrientation + 90);
                spriteRenderer.enabled = true;
                spriteRenderer.color = rightColor;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
