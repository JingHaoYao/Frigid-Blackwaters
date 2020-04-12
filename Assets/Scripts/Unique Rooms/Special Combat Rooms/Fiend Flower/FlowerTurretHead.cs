using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTurretHead : MonoBehaviour
{
    public GameObject podProjectile;
    [SerializeField] AudioSource fireAudio;
    [SerializeField] Animator animator;
    public GameObject fiendFlowerBoss;
    [SerializeField] SpriteRenderer spriteRenderer;
    public SpriteRenderer stemSpriteRenderer;
    public FlowerTurretStem stem;
    [SerializeField] private bool blueFlower = false;
    Enemy flowerFiend;

    float attackPeriod = 0;

    float angleToShip
    {
        get
        {
            return (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, angleToShip);
        this.spriteRenderer.sortingOrder = stemSpriteRenderer.sortingOrder + 3;
    }

    public void initializeFlowerHead()
    {
        StartCoroutine(spitOutPod());
        spriteRenderer.enabled = true;
        flowerFiend = fiendFlowerBoss.GetComponent<FiendFlower>();
        StartCoroutine(updateLoop());
    }


    IEnumerator updateLoop()
    {
        while (true)
        {
            transform.rotation = Quaternion.Euler(0, 0, angleToShip);
            this.spriteRenderer.sortingOrder = stemSpriteRenderer.sortingOrder + 3;
            if (flowerFiend.health <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(destroyProcedure());
            }
            yield return null;
        }
    }

    IEnumerator spitOutPod()
    {
        yield return new WaitForSeconds(0.333f);
        for (int i = 0; i < 3; i++)
        {
            animator.SetTrigger("Attack");
            fireAudio.Play();
            yield return new WaitForSeconds(4 / 12f);

            if (blueFlower == false)
            {
                GameObject podInstant = Instantiate(podProjectile, transform.position + new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * 1.25f, Quaternion.identity);
                podInstant.GetComponent<BasicProjectile>().angleTravel = angleToShip;
                podInstant.GetComponent<ProjectileParent>().instantiater = fiendFlowerBoss;
            }
            else
            {
                for (int k = 0; k < 3; k++)
                {
                    float angle = angleToShip - 10 + 10 * k;
                    GameObject podInstant = Instantiate(podProjectile, transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 1.25f, Quaternion.identity);
                    podInstant.GetComponent<BasicProjectile>().angleTravel = angle;
                    podInstant.GetComponent<ProjectileParent>().instantiater = fiendFlowerBoss;
                }
            }
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(destroyProcedure());
    }

    IEnumerator destroyProcedure()
    {
        animator.SetTrigger("Shrink");
        yield return new WaitForSeconds(0.333f);
        spriteRenderer.enabled = false;
        stem.destroyStem();
    }
}
