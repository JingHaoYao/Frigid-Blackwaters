using System.Collections;
using UnityEngine;

public class FrogMasterDeflectionBox : MonoBehaviour
{
    [SerializeField] GameObject lightningEffect;
    public FrogMaster frogMaster;
    [SerializeField] GameObject deflectionProjectile;
    [SerializeField] CircleCollider2D collider;
    Coroutine projectileRoutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 16)
        {
            Instantiate(lightningEffect, collider.ClosestPoint(collision.transform.position), Quaternion.identity);
        }
    }

    IEnumerator spawnIdleProjectiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
            float offset = Random.Range(0, 90);
            for(int i = 0; i < 3; i++)
            {
                float angle = i * 120 + offset;

                summonProjectiles(collider.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 1.5f, transform.position);
            }
        }
    }

    void summonProjectiles(Vector3 spawnPos, Vector3 center)
    {
        Instantiate(lightningEffect, spawnPos, Quaternion.identity);
        GameObject instant = Instantiate(deflectionProjectile, spawnPos, Quaternion.identity);
        instant.GetComponent<ProjectileParent>().instantiater = frogMaster.gameObject;
        instant.GetComponent<FrogMasterProjectile>().Initialize(center);
    }

    private void OnEnable()
    {
        projectileRoutine = StartCoroutine(spawnIdleProjectiles());
    }

    private void OnDisable()
    {
        StopCoroutine(projectileRoutine);
    }
}
