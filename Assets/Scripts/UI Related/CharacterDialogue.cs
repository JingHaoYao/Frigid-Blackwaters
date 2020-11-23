using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] characterSpriteList;
    
    public bool moveFromRight = true;
    public bool isMoving = false;
    public float stopInPosition = 0, stopOutPosition = 0;
    public bool toggleLeft = false, toggleRight = false;
    public bool inPresentScene = false;
    int spriteIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterSpriteList[0];
    }

    void moveLeft(float stopInPosition, float stopOutPosition)
    {
        if (moveFromRight)
        {
            if (transform.position.x > stopInPosition + Camera.main.transform.position.x)
            {
                inPresentScene = true;
                isMoving = true;
                transform.position += Vector3.left * 25 * Time.deltaTime;
            }
            else
            {
                isMoving = false;
                inPresentScene = true;
            }
        }
        else
        {
            if (transform.position.x > stopOutPosition + Camera.main.transform.position.x)
            {
                inPresentScene = false;
                isMoving = true;
                transform.position += Vector3.left * 25 * Time.deltaTime;
            }
            else
            {
                inPresentScene = false;
                isMoving = false;
            }
        }
    }

    void moveRight(float stopInPosition, float stopOutPosition)
    {
        if (moveFromRight)
        {
            if (transform.position.x < stopOutPosition + Camera.main.transform.position.x)
            {
                inPresentScene = false;
                isMoving = true;
                transform.position += Vector3.right * 25 * Time.deltaTime;
            }
            else
            {
                inPresentScene = false;
                isMoving = false;
            }
        }
        else
        {
            if(transform.position.x < stopInPosition + Camera.main.transform.position.x)
            {
                inPresentScene = true;
                isMoving = true;
                transform.position += Vector3.right * 25 * Time.deltaTime;
            }
            else
            {
                inPresentScene = true;
                isMoving = false;
            }
        }
    }

    void LateUpdate()
    {
        if (toggleLeft)
        {
            moveLeft(stopInPosition, stopOutPosition);
        }

        if (toggleRight)
        {
            moveRight(stopInPosition, stopOutPosition);
        }

        if(isMoving == false)
        {
            toggleLeft = false;
            toggleRight = false;
        }
    }

    public void updateSprite()
    {
        if (inPresentScene == true)
        {
            if (spriteIndex < characterSpriteList.Length - 1)
            {
                spriteIndex++;
                spriteRenderer.sprite = characterSpriteList[spriteIndex];
            }
        }
    }
}
