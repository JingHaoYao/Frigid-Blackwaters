using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBomberFloatingProjectile : MonoBehaviour
{
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] GameObject explosion;
    public SkeletonBomber skeletonBomber;

    bool dontExplode = false;

    private void Start()
    {
        skeletonBomber.addBomb(this);
        if (skeletonBomber.bomberUnderLight)
        {
            LeanTween.alpha(this.gameObject, 0, 0.5f);
        }
    }

    public void fadeAway()
    {
        dontExplode = true;
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
    }

    public void activateBomb()
    {
        if (!dontExplode)
        {
            GameObject explosionInstant = Instantiate(explosion, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            explosionInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;
            Destroy(this.gameObject);
        }
    }
}
