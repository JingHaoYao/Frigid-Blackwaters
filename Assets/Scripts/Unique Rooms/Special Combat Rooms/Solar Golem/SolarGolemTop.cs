using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarGolemTop : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    public GameObject baseOfGolem;
    public GameObject pellet;

    public void Initialize(int baseSortingLayer)
    {
        spriteRenderer.sortingOrder = baseSortingLayer + 2;
    }

    float angleToShip
    {
        get
        {
            return (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;
        }
    }

    IEnumerator spawnPellets()
    {
        animator.SetTrigger("Attack");
        float initialAngle = angleToShip + Random.Range(0, 2) == 1 ? 90 : -90;
        
        if(Random.Range(0,2) == 1) {
            for (int i = 0; i < 36; i++)
            {
                float angleToConsider = initialAngle + i * 10;
                GameObject pelletInstant = Instantiate(pellet, transform.position + Vector3.up * 0.4f, Quaternion.identity);
                pelletInstant.GetComponent<BasicProjectile>().angleTravel = angleToConsider;
                pelletInstant.GetComponent<ProjectileParent>().instantiater = baseOfGolem;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 35; i >= 0; i--)
            {
                float angleToConsider = initialAngle + i * 10;
                GameObject pelletInstant = Instantiate(pellet, transform.position + Vector3.up * 0.4f, Quaternion.identity);
                pelletInstant.GetComponent<BasicProjectile>().angleTravel = angleToConsider;
                pelletInstant.GetComponent<ProjectileParent>().instantiater = baseOfGolem;
                yield return new WaitForSeconds(0.1f);
            }
        }

        animator.SetTrigger("Idle");
    }

    public void pelletAttack()
    {
        StartCoroutine(spawnPellets());
    }

    public void dieDown()
    {
        StopAllCoroutines();
        animator.SetTrigger("Death");
        spriteRenderer.color = Color.white;
        float yPosition = transform.position.y - 1f;
        LeanTween.moveY(this.gameObject, yPosition, 1f);
    }

    public void startUp()
    {
        animator.SetTrigger("Startup");
        float yPosition = transform.position.y + 1f;
        LeanTween.moveY(this.gameObject, yPosition, 1f);
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public void toggleHitFrame()
    {
        StartCoroutine(hitFrame());
    }
}
