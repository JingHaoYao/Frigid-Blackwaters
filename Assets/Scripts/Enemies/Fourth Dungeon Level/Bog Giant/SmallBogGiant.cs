using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBogGiant : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider2D damageCollider;
    [SerializeField] GameObject obstacleCollider;
    [SerializeField] PickRendererLayer pickRendererLayer;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] viewSprites;
    private int whatView = 0;
    private int mirror = 1;
    bool isAttacking = false;
    [SerializeField] GameObject bogProjectile;
    Coroutine mainRoutine;
    BogGiant bogGiant;
    Vector3 centerPosition = Vector3.zero;

    public void Initialize(BogGiant bogGiant)
    {
        this.bogGiant = bogGiant;
        centerPosition = Camera.main.transform.position;
        StartCoroutine(awaken());
    }

    IEnumerator awaken()
    {
        animator.SetTrigger("SpawnGiant");
        pickRendererLayer.enabled = true;
        yield return new WaitForSeconds(1.083f);
        animator.enabled = false;
        damageCollider.enabled = true;
        obstacleCollider.SetActive(true);
        mainRoutine = StartCoroutine(mainAttackLoop());
    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
            mirror = 1;
        }
        else
        {
            whatView = 2;
            mirror = 1;
        }
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
    }

    void pickSprite()
    {
        pickView(angleToShip);
        transform.localScale = new Vector3(3f * mirror, 3f);
        spriteRenderer.sprite = viewSprites[whatView - 1];
    }

    IEnumerator mainAttackLoop()
    {
        while (true)
        {
            if (isAttacking == false)
            {
                pickSprite();
            }

            yield return null;
        }
    }

    IEnumerator throwBogBall()
    {
        animator.enabled = true;
        isAttacking = true;
        pickView(angleToShip);
        float randAngle = Random.Range(0, Mathf.PI * 2);

        Vector3 throwPosition = PlayerProperties.playerShipPosition;
        if (Random.Range(0, 3) <= 1)
        {
            throwPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * Random.Range(3.0f, 6.0f);

            while (Physics2D.OverlapCircle(throwPosition, 1f, 12))
            {
                randAngle = Random.Range(0, Mathf.PI * 2);
                throwPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * Random.Range(3.0f, 6.0f);
            }
        }

        animator.SetTrigger("Throw" + whatView);

        yield return new WaitForSeconds(7 / 12f);

        GameObject bogProjectileInstant = Instantiate(bogProjectile, transform.position, Quaternion.identity);
        bogProjectileInstant.GetComponent<BogGiantProjectile>().Initialize(this.gameObject, 99, new Vector3(Mathf.Clamp(throwPosition.x, centerPosition.x - 7.5f, centerPosition.x + 7.5f), Mathf.Clamp(throwPosition.y, centerPosition.y - 7.5f, centerPosition.y + 7.5f)), false);

        yield return new WaitForSeconds(4 / 12f);

        isAttacking = false;
        animator.enabled = false;
    }

    public void triggerAttack()
    {
        StartCoroutine(throwBogBall());
    }

    public void triggerDisappear()
    {
        StopAllCoroutines();
        animator.enabled = true;
        animator.SetTrigger("Dissolve");
        Destroy(this.gameObject, 1.667f);
        damageCollider.enabled = false;
        obstacleCollider.SetActive(false);
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DamageAmount>())
        {
            bogGiant.dealDamage(collision.GetComponent<DamageAmount>().damage);
            StartCoroutine(hitFrame());
        }
    }
}
