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
        laserBeamInstant.GetComponent<SolarGolemBeam>().Initialize(initialAngle, initialAngle + 90, 3f, this.spriteRenderer.sortingOrder);
        laserBeamInstant2.GetComponent<SolarGolemBeam>().Initialize(initialAngle + 180, initialAngle + 270, 3f, this.spriteRenderer.sortingOrder);
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
        laserBeamInstant?.GetComponent<SolarGolemBeam>().forceShutDown();
        laserBeamInstant2?.GetComponent<SolarGolemBeam>().forceShutDown();
        LeanTween.moveLocalY(this.gameObject, transform.position.y - 0.5f, 1f);
    }

    public void startUp()
    {
        animator.SetTrigger("Awaken");
        LeanTween.moveLocalY(this.gameObject, transform.position.y + 0.5f, 1f);
    }
}
