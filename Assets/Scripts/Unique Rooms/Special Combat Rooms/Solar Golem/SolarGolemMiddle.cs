using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarGolemMiddle : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    public GameObject laserBeam;
    public GameObject baseOfGolem;
    GameObject laserBeamInstant, laserBeamInstant2;

    public void Initialize(int baseSortingLayer)
    {
        spriteRenderer.sortingOrder = baseSortingLayer + 1;
    }

    float angleToShip
    {
        get
        {
            return (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;
        }
    }

    IEnumerator spawnLaserBeam()
    {
        animator.SetTrigger("Attack");
        float initialAngle = angleToShip + 90;
        laserBeamInstant = Instantiate(laserBeam, transform.position + Vector3.up * 2, Quaternion.Euler(0, 0, initialAngle));
        laserBeamInstant2 = Instantiate(laserBeam, transform.position + Vector3.up * 2, Quaternion.Euler(0, 0, initialAngle + 180));

        float toAngle1;
        float toAngle2;

        if(Random.Range(0, 2) == 1)
        {
            toAngle1 = initialAngle + 90;
            toAngle2 = initialAngle + 270;
        }
        else
        {
            toAngle1 = initialAngle - 90;
            toAngle2 = initialAngle + 90;
        }

        laserBeamInstant.GetComponent<SolarGolemBeam>().Initialize(initialAngle, toAngle1, 3f, this.spriteRenderer.sortingOrder);
        laserBeamInstant2.GetComponent<SolarGolemBeam>().Initialize(initialAngle + 180, toAngle2, 3f, this.spriteRenderer.sortingOrder);
        laserBeamInstant.GetComponent<ProjectileParent>().instantiater = baseOfGolem;
        laserBeamInstant2.GetComponent<ProjectileParent>().instantiater = baseOfGolem;
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("Idle");
    }

    public void laserBeamAttack()
    {
        StartCoroutine(spawnLaserBeam());
    }

    public void dieDown()
    {
        StopAllCoroutines();
        animator.SetTrigger("Death");

        if (laserBeamInstant != null)
        {
            laserBeamInstant.GetComponent<SolarGolemBeam>().forceShutDown();
        }

        if (laserBeamInstant2 != null)
        {
            laserBeamInstant2.GetComponent<SolarGolemBeam>().forceShutDown();
        }
        spriteRenderer.color = Color.white;
        float yPosition = transform.position.y - 0.5f;
        LeanTween.moveY(this.gameObject, yPosition, 1f);
    }

    public void startUp()
    {
        animator.SetTrigger("Startup");
        float yPosition = transform.position.y + 0.5f;
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
