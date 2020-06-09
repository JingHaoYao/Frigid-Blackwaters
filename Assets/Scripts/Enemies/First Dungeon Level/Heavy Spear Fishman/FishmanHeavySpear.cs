using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishmanHeavySpear : MonoBehaviour
{
    PolygonCollider2D polyCol;
    Animator animator;
    SpriteRenderer spriteRenderer;
    GameObject playerShip;
    public float travelAngle;
    public float spearSpeed = 20;
    bool hitShip;
    Vector3 hitPos;
    Vector3 hitShipPos;

    void destroySpear()
    {
        animator.SetTrigger("Break");
        Destroy(this.gameObject, 4f / 12f);
    }

    IEnumerator slowShip(float duration)
    {
        PlayerScript playerScript = FindObjectOfType<PlayerScript>();

        playerScript.enemySpeedModifier -= 2;

        yield return new WaitForSeconds(duration);
        playerScript.enemySpeedModifier += 2;
        destroySpear();
    }

    void Start()
    {
        polyCol = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerShip = GameObject.Find("PlayerShip");
        transform.rotation = Quaternion.Euler(0, 0, travelAngle);
    }

    void Update()
    {
        if (hitShip == false)
        {
            transform.position += new Vector3(Mathf.Cos(travelAngle * Mathf.Deg2Rad), Mathf.Sin(travelAngle * Mathf.Deg2Rad), 0) * Time.deltaTime * spearSpeed;
        }
        else
        {
            transform.position = hitPos + (playerShip.transform.position - hitShipPos);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        polyCol.enabled = false;

        if (collision.gameObject.tag == "playerHitBox" && hitShip == false)
        {
            PlayerProperties.playerScript.dealDamageToShip(150, this.gameObject);
            hitShip = true;
            spearSpeed = 0;
            polyCol.enabled = false;
            hitShipPos = playerShip.transform.position;
            hitPos = transform.position;
            StartCoroutine(slowShip(6));
        }
        else
        {
            if (hitShip == false)
            {
                spearSpeed = 0;
                animator.SetTrigger("Break");
                Destroy(this.gameObject, 4f / 12f);
            }
        }
    }
}
