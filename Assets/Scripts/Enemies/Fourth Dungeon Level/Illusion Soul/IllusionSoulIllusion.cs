using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionSoulIllusion : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] viewSprites;
    [SerializeField] GameObject projectile;
    [SerializeField] AudioSource damageAudio, attackAudio;
    int whatView = 0;
    int mirror = 1;
    IllusionSoul boss;
    [SerializeField] BoxCollider2D damageHitBox;
    [SerializeField] GameObject trace;
    Coroutine lookAtPlayerRoutine;

    public void Initialize(IllusionSoul boss, Vector3 position)
    {
        damageHitBox.enabled = false;
        animator.enabled = false;
        Vector3 lastPosition = transform.position;
        this.boss = boss;
        LeanTween.move(this.gameObject, position, 1f).setEaseOutCirc().setOnUpdate((float val) => {
            if (Vector2.Distance(lastPosition, transform.position) > 0.5f)
            {
                lastPosition = transform.position;
                Instantiate(trace, transform.position, Quaternion.identity);
            }
        } ).setOnComplete(() => { lookAtPlayerRoutine = StartCoroutine(lookAtPlayer()); damageHitBox.enabled = true; });
    }

    IEnumerator lookAtPlayer()
    {
        while (true)
        {
            setSpriteAndScale();
            yield return null;
        }
    }


    void setScale()
    {
        transform.localScale = new Vector3(4 * mirror, 4);
    }

    void setSpriteAndScale()
    {
        pickView(angleToShip);
        setScale();
        spriteRenderer.sprite = viewSprites[whatView - 1];
    }

    public void triggerIllusionAttack()
    {
        StartCoroutine(fireAndDisappear());
    }

    public void triggerIllusionDeath()
    {
        damageHitBox.enabled = false;
        animator.enabled = true;
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 11 / 12f);
    }

    IEnumerator fireAndDisappear()
    {
        StopCoroutine(lookAtPlayer());
        damageHitBox.enabled = false;
        animator.enabled = true;
        float attackingAngle = angleToShip;

        pickView(attackingAngle);

        animator.SetTrigger("Attack" + whatView);
        setScale();
        yield return new WaitForSeconds(6 / 12f);
        attackAudio.Play();
        GameObject instant = Instantiate(projectile, transform.position + new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad) + 1), Quaternion.identity);
        instant.GetComponent<BasicProjectile>().angleTravel = attackingAngle;
        instant.GetComponent<ProjectileParent>().instantiater = boss.gameObject;

        yield return new WaitForSeconds(6 / 12f);

        animator.SetTrigger("Death");

        yield return new WaitForSeconds(11 / 12f);
        Destroy(this.gameObject);
    }

    private float angleToShip
    {
        get
        {
            return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            boss.illusionHit();
        }
    }
}
