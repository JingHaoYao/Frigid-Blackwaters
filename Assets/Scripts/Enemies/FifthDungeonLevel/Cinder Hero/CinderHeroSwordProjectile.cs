using System.Collections;
using UnityEngine;

public class CinderHeroSwordProjectile : MonoBehaviour
{
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Collider2D boxCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    private int numberBounces = 0;
    Vector3 travelVector;
    GameObject targetEnemy;
    float currentAngle = 0;
    bool flashed = false;

    public void Initialize(GameObject bossEnemy, Vector3 positionToMove, float angle)
    {
        projectileParent.instantiater = bossEnemy;
        LeanTween.move(this.gameObject, positionToMove, 0.5f).setEaseOutQuad().setOnComplete(() => StartCoroutine(mainLoop()));
        boxCollider.enabled = false;
        travelVector = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        targetEnemy = bossEnemy;
        currentAngle = angle;
    }

    IEnumerator mainLoop()
    {
        StartCoroutine(mainRotateLoop());
        float period = 0;
        float rotateSpeed = 0;
        boxCollider.enabled = true;

        while(period < 5)
        {
            period += Time.deltaTime;
            currentAngle += Time.deltaTime * rotateSpeed;
            rotateSpeed += Time.deltaTime;
            transform.position = targetEnemy.transform.position + (Vector3.up * 0.5f) + new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));

            if(period >= 4.4f & flashed == false)
            {
                StartCoroutine(blinkWhite());
            }

            yield return null;
        }

        LeanTween.move(this.gameObject, transform.position + new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * 20, 1f).setEaseInQuad().setOnComplete(() => Destroy(this.gameObject));
        targetEnemy.GetComponent<CinderHero>().RemoveSword(this);
    }

    IEnumerator blinkWhite()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 1);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator mainRotateLoop()
    {
        LeanTween.rotateZ(this.gameObject, currentAngle * Mathf.Rad2Deg - 90, 0.25f);

        yield return new WaitForSeconds(0.25f);
        while (true)
        {
            transform.rotation = Quaternion.Euler(0, 0, currentAngle * Mathf.Rad2Deg - 90);
            yield return null;
        }
    }
}
