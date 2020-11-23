using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCloneEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite upLeft, up, left, downLeft, down;

    public void Initialize(float angleTravel, int sortingLayer)
    {
        transform.position = PlayerProperties.playerShipPosition;
        spriteRenderer.sortingOrder = sortingLayer + 10;
        this.gameObject.SetActive(true);
        spriteRenderer.color = new Color(0.3632f, 1, 0.97003f, 0.5f);
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => { this.gameObject.SetActive(false); });
        pickSpriteAndScale(angleTravel);
    }

    void pickSpriteAndScale(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = upLeft;
            transform.localScale = new Vector3(-2.5f, 2.5f, 0);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = up;
            transform.localScale = new Vector3(2.5f, 2.5f, 0);
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            spriteRenderer.sprite = upLeft;
            transform.localScale = new Vector3(2.5f, 2.5f, 0);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            spriteRenderer.sprite = left;
            transform.localScale = new Vector3(2.5f, 2.5f, 0);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = downLeft;
            transform.localScale = new Vector3(2.5f, 2.5f, 0);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            spriteRenderer.sprite = down;
            transform.localScale = new Vector3(2.5f, 2.5f, 0);
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            spriteRenderer.sprite = downLeft;
            transform.localScale = new Vector3(-2.5f, 2.5f, 0);
        }
        else
        {
            spriteRenderer.sprite = left;
            transform.localScale = new Vector3(-2.5f, 2.5f, 0);
        }
    }
}
