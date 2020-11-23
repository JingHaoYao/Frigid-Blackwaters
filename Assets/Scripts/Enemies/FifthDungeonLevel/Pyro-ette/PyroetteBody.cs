using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroetteBody : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    int whatView = 0;
    int mirror = 1;
    int prevView = 0;
    [SerializeField] AudioSource attackAudio;

    [SerializeField] GameObject spiralProjectile;
    private Enemy enemy;
    private SpriteRenderer enemySpriteRenderer;

    float angleOffset = 0;

    void spawnFireBalls()
    {
        if (enemy.stopAttacking == false)
        {
            attackAudio.Play();
            for (int i = 0; i < 4; i++)
            {
                float angle = (i * 90)*Mathf.Deg2Rad;
                GameObject projectileInstant = Instantiate(spiralProjectile, transform.position + Vector3.up * 0.5f + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 0.5f, Quaternion.identity);
                projectileInstant.GetComponent<PyroettePyrotheumProjectile>().Initialize(transform.position + Vector3.up * 0.5f, enemy.gameObject);
            }
        }
    }

    public void Initialize(Enemy baseEnemy)
    {
        this.enemy = baseEnemy;
        enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
        StartCoroutine(pickViewLoop());
        StartCoroutine(spawnSpiralProjectiles());
    }

    IEnumerator spawnSpiralProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            spawnFireBalls();
        }
    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = -1;
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
        transform.localScale = new Vector3(mirror * 4, 4);
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    public void HitFrame()
    {
        StartCoroutine(hitFrame());
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public void Death()
    {
        StopAllCoroutines();
        spriteRenderer.color = Color.white;
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 10 / 12f);
    }

    IEnumerator pickViewLoop()
    {
        while (true)
        {
            spriteRenderer.sortingOrder = enemySpriteRenderer.sortingOrder + 1;
            angleOffset += Time.deltaTime * 2;
            if(angleOffset > Mathf.PI * 2)
            {
                angleOffset = 0;
            }

            transform.position = enemy.transform.position + Vector3.up * 0.5f + new Vector3(Mathf.Cos(angleOffset), Mathf.Sin(angleOffset)) * 0.5f;


            pickView(angleToShip());

            if (prevView != whatView)
            {
                animator.SetTrigger("Idle" + whatView);
                prevView = whatView;
            }

            yield return null;
        }
    }
}
