using System.Collections;
using UnityEngine;

public class FloorFire : MonoBehaviour
{
    [SerializeField] Collider2D damagingCollider;
    Coroutine fireLoopInstant;
    Camera mainCamera;
    int xPosition, yPosition;

    private void Start()
    {
        fireLoopInstant = StartCoroutine(FireLoop());
        StartCoroutine(WaitAndFadeOut());
        mainCamera = Camera.main;

        xPosition = Mathf.RoundToInt(transform.position.x  - mainCamera.transform.position.x - 0.5f) + 8;
        yPosition = Mathf.RoundToInt(transform.position.y - mainCamera.transform.position.y - 0.5f) + 8;

        EnemyPool.floorFireSpawner.AddFloorFire(xPosition, yPosition);
    }

    IEnumerator FireLoop()
    {
        while (true)
        {
            damagingCollider.enabled = true;
            yield return new WaitForSeconds(0.25f);
            damagingCollider.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator WaitAndFadeOut()
    {
        yield return new WaitForSeconds(2.5f);
        StopCoroutine(fireLoopInstant);
        damagingCollider.enabled = false;
        LeanTween.alpha(this.gameObject, 0, 0.75f).setOnComplete(() => { Destroy(this.gameObject); EnemyPool.floorFireSpawner.RemoveFloorFire(xPosition, yPosition); });
    }
}
