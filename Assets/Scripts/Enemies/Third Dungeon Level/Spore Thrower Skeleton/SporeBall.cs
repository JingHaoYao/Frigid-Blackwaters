using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeBall : MonoBehaviour
{
    public Vector3 targetLocation = Vector3.zero;
    public GameObject waterSplash;
    [SerializeField] private CircleCollider2D circCol;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ProjectileParent projectileParent;
    private float totalTime;
    private float currProgress = 0;
    Vector3 unitVector;
    private Vector3 tempTransform = Vector3.zero;
    private float currentTime = 0;

    public GameObject smallMushroom;
    public GameObject bigMushroom;

    bool splashed = false;

    void Start()
    {
        circCol.enabled = false;
        totalTime = Vector2.Distance(targetLocation, transform.position) / speed;
        LeanTween.move(shadow, targetLocation, totalTime);
        LeanTween.value(0, 1, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseOutQuad().setOnComplete(() => { LeanTween.value(1, 0, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseInQuad(); });
        tempTransform = transform.position;
        unitVector = (targetLocation - transform.position).normalized;
    }

    private void Update()
    {
        if (splashed == false)
        {
            tempTransform += unitVector * Time.deltaTime * speed;
            shadow.transform.localScale = new Vector3(0.05f, 0.05f) * currProgress;
            transform.position = tempTransform + new Vector3(0, 5 * currProgress);

            currentTime += Time.deltaTime;

            circCol.enabled = currProgress <= 0.2f;

            if (currentTime >= totalTime && splashed == false)
            {
                StartCoroutine(spawnMushrooms());
                splashed = true;
                Instantiate(waterSplash, transform.position, Quaternion.identity);
            }
        }

        spriteRenderer.sortingOrder = (200 - (int)((transform.position.y - 5 * currProgress)));
    }

    IEnumerator spawnMushrooms()
    {
        spriteRenderer.enabled = false;
        circCol.enabled = false;
        for(int i = 0; i < 6; i++)
        {
            float angleToConsider = (60 * i) * Mathf.Deg2Rad;
            GameObject mushRoomInstant = Instantiate(bigMushroom, transform.position + new Vector3(Mathf.Cos(angleToConsider), Mathf.Sin(angleToConsider)) * 0.75f, Quaternion.identity);
            mushRoomInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.instantiater;

            if (Random.Range(0, 2) == 1)
            {
                mushRoomInstant.transform.localScale = new Vector3(-3, 3);
            }
        }
        yield return new WaitForSeconds(0.75f);
        for (int i = 0; i < 8; i++)
        {
            float angleToConsider = (45 * i) * Mathf.Deg2Rad;
            GameObject mushRoomInstant = Instantiate(smallMushroom, transform.position + new Vector3(Mathf.Cos(angleToConsider), Mathf.Sin(angleToConsider)) * 1.4f, Quaternion.identity);
            mushRoomInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.instantiater;

            if (Random.Range(0, 2) == 1)
            {
                mushRoomInstant.transform.localScale = new Vector3(-3, 3);
            }
        }
        Destroy(this.gameObject);
    }
}
