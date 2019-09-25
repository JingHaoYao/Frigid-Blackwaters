using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorTarget : MonoBehaviour
{
    PlayerScript playerScript;
    public ShipWeaponScript weapon1, weapon2, weapon3;
    public Color frontColor, leftColor, rightColor;
    public Image circleFill;

    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
    }

    void Update()
    {
        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        transform.position = new Vector2(Mathf.Clamp(mouseX, Camera.main.transform.position.x - Camera.main.orthographicSize, Camera.main.transform.position.x + Camera.main.orthographicSize), Mathf.Clamp(mouseY, Camera.main.transform.position.y - Camera.main.orthographicSize, Camera.main.transform.position.y + Camera.main.orthographicSize));
        circleFill.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        setHovering(playerScript.angleOrientation + 360);
    }

    void setHovering(float angleOrientation)
    {
        float angleFromShip = ((360 + Mathf.Atan2(transform.position.y - playerScript.gameObject.transform.position.y, transform.position.x - playerScript.gameObject.transform.position.x) * Mathf.Rad2Deg) % 360) + 360;
        if(angleOrientation > angleFromShip + 180)
        {
            angleOrientation -= 360;
        }
        else if(angleFromShip > angleOrientation + 180)
        {
            angleFromShip -= 360;
        }

        if (Vector2.Distance(transform.position, playerScript.gameObject.transform.position) > 1.5f)
        {
            if (angleFromShip < angleOrientation + 45f && angleFromShip > angleOrientation - 45f)
            {
                weapon1.mouseHovering = true;
                weapon2.mouseHovering = false;
                weapon3.mouseHovering = false;
                circleFill.enabled = true;
                this.GetComponent<SpriteRenderer>().color = frontColor;
                circleFill.color = frontColor;
                circleFill.fillAmount = 1 - (weapon1.coolDownPeriod / weapon1.coolDownThreshold);
            }
            else if (angleFromShip < angleOrientation - 90 + 45f && angleFromShip > angleOrientation - 90 - 45f)
            {
                weapon2.mouseHovering = true;
                weapon1.mouseHovering = false;
                weapon3.mouseHovering = false;
                circleFill.enabled = true;
                this.GetComponent<SpriteRenderer>().color = leftColor;
                circleFill.color = leftColor;
                circleFill.fillAmount = 1 - (weapon2.coolDownPeriod / weapon2.coolDownThreshold);
            }
            else if (angleFromShip < angleOrientation + 90 + 45f && angleFromShip > angleOrientation + 90 - 45f)
            {
                weapon2.mouseHovering = false;
                weapon1.mouseHovering = false;
                weapon3.mouseHovering = true;
                this.GetComponent<SpriteRenderer>().color = rightColor;
                circleFill.enabled = true;
                circleFill.color = rightColor;
                circleFill.fillAmount = 1 - (weapon3.coolDownPeriod / weapon3.coolDownThreshold);
            }
            else
            {
                weapon2.mouseHovering = false;
                weapon1.mouseHovering = false;
                weapon3.mouseHovering = false;
                this.GetComponent<SpriteRenderer>().color = Color.white;
                circleFill.enabled = false;
            }
        }
        else
        {
            weapon2.mouseHovering = false;
            weapon1.mouseHovering = false;
            weapon3.mouseHovering = false;
            this.GetComponent<SpriteRenderer>().color = Color.white;
            circleFill.enabled = false;
        }

        if (playerScript.windowAlreadyOpen == true || Mathf.Abs(Camera.main.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x) > 10)
        {
            circleFill.gameObject.SetActive(false);
            Cursor.visible = true;
        }
        else
        {
            circleFill.gameObject.SetActive(true);
            Cursor.visible = false;
        }
    }
}
