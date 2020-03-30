using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticPodlingBouncePod : MonoBehaviour
{
    public Vector3 targetLocation = Vector3.zero;
    public GameObject waterSplash;
    [SerializeField] private CircleCollider2D circCol;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float totalTime;
    private float currProgress = 0;
    Vector3 unitVector;
    private Vector3 tempTransform = Vector3.zero;
    private float currentTime = 0;
    public GameObject bouncePod;
    Camera mainCamera;
    private float angleTravel;
    [SerializeField] private ProjectileParent projectileParent;

    void Start()
    {
        circCol.enabled = false;
        totalTime = Vector2.Distance(targetLocation, transform.position) / speed;
        LeanTween.move(shadow, targetLocation, totalTime);
        LeanTween.value(0, 1, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseOutQuad().setOnComplete(() => { LeanTween.value(1, 0, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseInQuad(); });
        tempTransform = transform.position;
        unitVector = (targetLocation - transform.position).normalized;
        angleTravel = Mathf.Atan2(unitVector.y, unitVector.x);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        tempTransform += unitVector * Time.deltaTime * speed;
        shadow.transform.localScale = new Vector3(0.05f, 0.05f) * currProgress;
        transform.position = tempTransform + new Vector3(0, 3 * currProgress);

        currentTime += Time.deltaTime;

        circCol.enabled = currProgress <= 0.3f;

        if (currentTime >= totalTime)
        {
            summonNextBouncePod();
            Destroy(this.gameObject);
            Instantiate(waterSplash, transform.position, Quaternion.identity);
        }

        spriteRenderer.sortingOrder = (200 - (int)((transform.position.y - 3 * currProgress) * 10));
    }

    void summonNextBouncePod()
    {
        if(Mathf.Abs(transform.position.x - mainCamera.transform.position.x) > 8 || Mathf.Abs(transform.position.y - mainCamera.transform.position.y) > 8)
        {
            return;
        }

        Vector3 nextPodLocation = transform.position + unitVector * 3.5f;

        while(Mathf.Abs(nextPodLocation.x - mainCamera.transform.position.x) > 8.5f || Mathf.Abs(nextPodLocation.y - mainCamera.transform.position.y) > 8.5f)
        {
            nextPodLocation -= unitVector * 0.3f;
        }

        GameObject podInstant = Instantiate(bouncePod, transform.position, Quaternion.identity);
        podInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.instantiater;
        podInstant.GetComponent<GiganticPodlingBouncePod>().targetLocation = nextPodLocation;
    }
}
