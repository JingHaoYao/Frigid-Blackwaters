using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogGiantProjectile : MonoBehaviour
{
    Vector3 targetLocation = Vector3.zero;
    [SerializeField] private CircleCollider2D circCol;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource splatAudio;
    [SerializeField] GameObject waterSplash;
    [SerializeField] ProjectileParent projectileParent;
    private float totalTime;
    private float currProgress = 0;
    Vector3 unitVector;
    private Vector3 tempTransform = Vector3.zero;
    private float currentTime = 0;
    [SerializeField] bool isSplitting = false;
    int numberBounces;
    [SerializeField] GameObject spawnProjectile;
    Vector3 centerOfRoom;
    [SerializeField] GameObject smallBog;
    bool spawnSmallBog = false;

    public void Initialize(GameObject enemy, int numberBounces, Vector3 targetPosition, bool spawnSmallBog)
    {
        this.numberBounces = numberBounces;
        projectileParent.instantiater = enemy;
        targetLocation = targetPosition;
        this.spawnSmallBog = spawnSmallBog;
    }

    void Start()
    {
        circCol.enabled = false;
        totalTime = Vector2.Distance(targetLocation, transform.position) / speed;
        LeanTween.move(shadow, targetLocation, totalTime);
        LeanTween.value(0, 1, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseOutQuad().setOnComplete(() => { LeanTween.value(1, 0, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseInQuad(); });
        tempTransform = transform.position;
        unitVector = (targetLocation - transform.position).normalized;
        centerOfRoom = Camera.main.transform.position;
        StartCoroutine(mainLoop());
    }

    IEnumerator mainLoop()
    {
        while (true)
        {
            tempTransform += unitVector * Time.deltaTime * speed;
            shadow.transform.localScale = new Vector3(0.05f, 0.05f) * currProgress;
            transform.position = tempTransform + new Vector3(0, 5 * currProgress);

            currentTime += Time.deltaTime;

            circCol.enabled = currProgress <= 0.3f;

            if (currentTime >= totalTime)
            {
                break;

            }

            spriteRenderer.sortingOrder = (200 - (int)((transform.position.y - 5 * currProgress) * 10));

            yield return null;
        }

        if (isSplitting == false && numberBounces < 3)
        {
            Instantiate(waterSplash, transform.position, Quaternion.identity);
            GameObject bouncingInstant = Instantiate(spawnProjectile, transform.position, Quaternion.identity);

            float dirToCenter = Mathf.Atan2(centerOfRoom.y - transform.position.y, centerOfRoom.x - transform.position.x) + Random.Range(-Mathf.PI / 4, Mathf.PI / 4);
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(dirToCenter), Mathf.Sin(dirToCenter)) * Random.Range(4.0f, 7.5f);

            bouncingInstant.GetComponent<BogGiantProjectile>().Initialize(this.projectileParent.instantiater, numberBounces + 1, spawnPosition, numberBounces + 1 >= 3 ? true : false);

            Destroy(this.gameObject);
        }
        else
        {
            if(isSplitting == true)
            {
                float range = Random.Range(4.0f, 7.5f);
                for(int i = 0; i < 8; i++)
                {
                    float angle = i * 45 * Mathf.Deg2Rad;
                    GameObject instant = Instantiate(spawnProjectile, transform.position, Quaternion.identity);
                    Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * range;

                    instant.GetComponent<BogGiantProjectile>().Initialize(this.projectileParent.instantiater, 99, new Vector3(Mathf.Clamp(spawnPosition.x, centerOfRoom.x - 8f, centerOfRoom.x + 8f), Mathf.Clamp(spawnPosition.y, centerOfRoom.y - 8f, centerOfRoom.y + 8f)), false);
                }
            }

            // Callback to boggiant boss

            if (spawnSmallBog)
            {
                GameObject newBogSmall = Instantiate(smallBog, transform.position + Vector3.down * 0.5f, Quaternion.identity);

                projectileParent.instantiater.GetComponent<BogGiant>().addSmallBogGiant(newBogSmall.GetComponent<SmallBogGiant>());
            }

            animator.SetTrigger("Impact");
            splatAudio.Play();
            Destroy(this.gameObject, 0.75f);
        }
    }
}
