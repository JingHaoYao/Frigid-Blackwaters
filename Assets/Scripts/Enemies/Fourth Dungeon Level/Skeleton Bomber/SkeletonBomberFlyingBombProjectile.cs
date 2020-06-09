using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBomberFlyingBombProjectile : MonoBehaviour
{
    Vector3 targetLocation = Vector3.zero;
    public GameObject waterSplash;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject floatingBomb;
    [SerializeField] private ProjectileParent projectileParent;
    private float totalTime;
    private float currProgress = 0;
    Vector3 unitVector;
    private Vector3 tempTransform = Vector3.zero;
    private float currentTime = 0;
    private SkeletonBomber bomber;

    public void Initialize(SkeletonBomber skeletonBomber, Vector3 targetPosition)
    {
        this.projectileParent.instantiater = skeletonBomber.gameObject;
        this.bomber = skeletonBomber;
        targetLocation = targetPosition;
    }

    void Start()
    {
        totalTime = Vector2.Distance(targetLocation, transform.position) / speed;
        LeanTween.move(shadow, targetLocation, totalTime);
        LeanTween.value(0, 1, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseOutQuad().setOnComplete(() => { LeanTween.value(1, 0, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseInQuad(); });
        tempTransform = transform.position;
        unitVector = (targetLocation - transform.position).normalized;
    }

    private void Update()
    {
        tempTransform += unitVector * Time.deltaTime * speed;
        shadow.transform.localScale = new Vector3(0.05f, 0.05f) * currProgress;
        transform.position = tempTransform + new Vector3(0, 5 * currProgress);

        currentTime += Time.deltaTime;

        if (currentTime >= totalTime)
        {
            Destroy(this.gameObject);
            Instantiate(waterSplash, transform.position, Quaternion.identity);
            GameObject bombInstant = Instantiate(floatingBomb, transform.position, Quaternion.identity);
            bombInstant.GetComponent<SkeletonBomberFloatingProjectile>().skeletonBomber = bomber;
            bombInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;
        }

        spriteRenderer.sortingOrder = (200 - (int)((transform.position.y - 5 * currProgress) * 10));
    }
}
