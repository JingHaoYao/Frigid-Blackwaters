using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonWaveProjectile : MonoBehaviour
{
    [SerializeField] CircleCollider2D circCol;
    [SerializeField] GameObject foam;
    Coroutine foamRoutine;

    IEnumerator spawnFoam(float travelAngle)
    {
        while (true)
        {
            Instantiate(foam, transform.position, Quaternion.Euler(0, 0, travelAngle + 90));
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Start()
    {
        InitializeProjectile();
    }

    void InitializeProjectile()
    {
        StartCoroutine(mainLoop(PlayerProperties.playerShipPosition));
    }

    IEnumerator mainLoop(Vector3 targetPosition)
    {
        circCol.enabled = false;
        yield return new WaitForSeconds(5 / 12f);
        circCol.enabled = true;

        foamRoutine = StartCoroutine(spawnFoam(Mathf.Atan2((targetPosition - transform.position).y, (targetPosition - transform.position).x) * Mathf.Rad2Deg));

        while (Vector2.Distance(targetPosition, transform.position) > 0.75f)
        {
            transform.position += (targetPosition - transform.position).normalized * 5 * Time.deltaTime;

            yield return null;
        }

        circCol.enabled = false;

        StopCoroutine(foamRoutine);

        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => { Destroy(this.gameObject); });
    }

}
