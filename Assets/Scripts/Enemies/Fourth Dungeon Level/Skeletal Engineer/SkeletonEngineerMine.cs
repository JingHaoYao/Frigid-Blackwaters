using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEngineerMine : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] ProjectileParent projectileParent;
    bool shouldExplode = true;
    public SkeletonEngineer skeletonEngineer;

    private void Start()
    {
        StartCoroutine(waitUntilDecay());
    }

    IEnumerator waitUntilDecay()
    {
        yield return new WaitForSeconds(10f);
        goAway();
    }

    public void goAway()
    {
        shouldExplode = false;
        skeletonEngineer?.removeMine(this.gameObject);
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == PlayerProperties.playerShip && shouldExplode)
        {
            GameObject explosionInstant = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;
            skeletonEngineer.removeMine(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
