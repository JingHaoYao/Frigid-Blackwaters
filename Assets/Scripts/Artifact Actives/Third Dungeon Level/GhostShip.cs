using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShip : MonoBehaviour
{
    List<Vector3> shipPositions = new List<Vector3>();
    public Sprite upLeft, up, left, downLeft, down;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PolygonCollider2D[] hitBoxes;
    float previousTravelAngle;
    bool canBeDamaged = true;
    public SemiDimensionalFlower flowerScript;

    void pickHitBox(int index)
    {
        for(int i = 0; i < 5; i++)
        {
            if(index == i)
            {
                hitBoxes[i].enabled = true;
            }
            else
            {
                hitBoxes[i].enabled = false;
            }
        }
    }

    void pickSprite(float angleOrientation)
    {
        if (angleOrientation > 15 && angleOrientation <= 75)
        {
            spriteRenderer.sprite = upLeft;
            transform.localScale = new Vector3(-2.6f, 2.6f, 0);
            pickHitBox(2);
        }
        else if (angleOrientation > 75 && angleOrientation <= 105)
        {
            spriteRenderer.sprite = up;
            transform.localScale = new Vector3(2.6f, 2.6f, 0);
            pickHitBox(3);
        }
        else if (angleOrientation > 105 && angleOrientation <= 165)
        {
            spriteRenderer.sprite = upLeft;
            transform.localScale = new Vector3(2.6f, 2.6f, 0);
            pickHitBox(2);
        }
        else if (angleOrientation > 165 && angleOrientation <= 195)
        {
            spriteRenderer.sprite = left;
            transform.localScale = new Vector3(2.6f, 2.6f, 0);
            pickHitBox(1);
        }
        else if (angleOrientation > 195 && angleOrientation <= 255)
        {
            spriteRenderer.sprite = downLeft;
            transform.localScale = new Vector3(2.6f, 2.6f, 0);
            pickHitBox(0);
        }
        else if (angleOrientation > 255 && angleOrientation <= 285)
        {
            spriteRenderer.sprite = down;
            transform.localScale = new Vector3(2.6f, 2.6f, 0);
            pickHitBox(4);
        }
        else if (angleOrientation > 285 && angleOrientation <= 345)
        {
            spriteRenderer.sprite = downLeft;
            transform.localScale = new Vector3(-2.6f, 2.6f, 0);
            pickHitBox(0);
        }
        else
        {
            spriteRenderer.sprite = left;
            transform.localScale = new Vector3(-2.6f, 2.6f, 0);
            pickHitBox(1);
        }
    }

    float currentTravelAngle()
    {
        return Mathf.Atan2(PlayerProperties.shipTravellingVector.y, PlayerProperties.shipTravellingVector.x);
    }

    private void OnEnable()
    {
        previousTravelAngle = currentTravelAngle();
        pickHitBox(6);
        shipPositions.Clear();
    }

    void Update()
    {
        if (Mathf.Abs(previousTravelAngle - currentTravelAngle()) > 0.001f)
        {
            shipPositions.Add(PlayerProperties.playerShipPosition);
            previousTravelAngle = currentTravelAngle();
        }

        float speed = PlayerProperties.playerScript.totalShipSpeed * 0.9f;

        if (shipPositions.Count > 0)
        {
            Vector3 velocityVector = (shipPositions[0] - transform.position).normalized * speed;
            pickSprite((360 + Mathf.Atan2(velocityVector.y, velocityVector.x) * Mathf.Rad2Deg) % 360);

            transform.position += velocityVector * Time.deltaTime;

            if (Vector2.Distance(transform.position, shipPositions[0]) < 0.1f)
            {
                shipPositions.Remove(shipPositions[0]);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 0.1f)
            {
                Vector3 velocityVector = (PlayerProperties.playerShipPosition - transform.position).normalized * speed;
                pickSprite((360 + Mathf.Atan2(velocityVector.y, velocityVector.x) * Mathf.Rad2Deg) % 360);

                transform.position += velocityVector * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeDamaged && collision.gameObject.layer == 13)
        {
            PlayerProperties.playerScript.dealTrueDamageToShip(100 + flowerScript.numberHits * 125);
            flowerScript.numberHits++;
            StartCoroutine(delayBeforeCanBeDamagedAgain());
            StartCoroutine(hitFrame());
        }
    }

    IEnumerator hitFrame()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator delayBeforeCanBeDamagedAgain()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(0.1f);
        canBeDamaged = true;
    }
}
