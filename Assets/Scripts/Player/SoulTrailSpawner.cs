using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulTrailSpawner : MonoBehaviour
{
    [SerializeField] GameObject soulTrail;
    [SerializeField] GameObject runeMark;
    List<GameObject> spawnedSoulTrails = new List<GameObject>();
    List<GameObject> spawnedRuneMarks = new List<GameObject>();
    RoomTemplates roomTemplates;

    private int previousArtifactKills = 0;

    private void Update()
    {
        if(previousArtifactKills != PlayerItems.numberArtifactKills)
        {
            if(PlayerItems.numberArtifactKills < previousArtifactKills)
            {
                useArtifactKills(previousArtifactKills - PlayerItems.numberArtifactKills);
            }
            previousArtifactKills = PlayerItems.numberArtifactKills;
        }
    }

    private void Start()
    {
        roomTemplates = FindObjectOfType<RoomTemplates>();
    }

    private void Awake()
    {
        PlayerProperties.soulTrailSpawner = this;
    }

    public void SpawnEnemyDeathSouls(int numberSouls, Vector3 position)
    {
        StartCoroutine(spawnMultiple(numberSouls, position));
    }

    IEnumerator spawnMultiple(int numberSouls, Vector3 position)
    {
        for(int i = 0; i < numberSouls; i++)
        {
            SpawnSoulTrail(position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void SpawnSoulTrail(Vector3 position)
    {
        foreach(GameObject soulTrailInstant in spawnedSoulTrails)
        {
            if (soulTrailInstant.activeSelf == false)
            {
                soulTrailInstant.transform.position = position;
                soulTrailInstant.SetActive(true);
                StartCoroutine(followRoutine(soulTrailInstant));
                return;
            }
        }

        GameObject newSoulTrail = Instantiate(soulTrail, position, Quaternion.identity);
        spawnedSoulTrails.Add(newSoulTrail);
        StartCoroutine(followRoutine(newSoulTrail));
    }

    IEnumerator followRoutine(GameObject trail)
    {
        TrailRenderer trailRenderer = trail.GetComponent<TrailRenderer>();
        yield return new WaitForEndOfFrame();
        trailRenderer.enabled = true;
        trailRenderer.startWidth = 0.4f;
        float speed = Random.Range(8, 12);

        while(Vector2.Distance(trail.transform.position, PlayerProperties.playerShipPosition) > 0.5f)
        {
            float angle = Mathf.Atan2(PlayerProperties.playerShipPosition.y - trail.transform.position.y, PlayerProperties.playerShipPosition.x - trail.transform.position.x);
            trail.transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed * Time.deltaTime;
            speed += Time.deltaTime * 2;
            yield return null;
        }

        LeanTween.value(trailRenderer.startWidth, 0, 0.25f).setOnUpdate((float val) => { trailRenderer.startWidth = val; });
        PlayerProperties.playerScript.FlashWhitePickup();
        trail.GetComponent<Animator>().SetTrigger("Dissapear");
        trail.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);

        trailRenderer.enabled = false;
        trail.SetActive(false);
    }

    Vector3 pickRandomPositionAroundShip()
    {
        float randAngle = Random.Range(0, Mathf.PI * 2);
        return PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * Random.Range(1.0f, 1.5f);
    }

    void useArtifactKills(int numberKills)
    {
        for (int i = 0; i < numberKills; i++)
        {
            SpawnUseArtifactSoulTrail();
        }
    }

    public void spawnRuneMarks(Vector3 position)
    {
        int numberRuneMarks = 12;

        int numberRuneMarksSpawned = 0;

        float offset = 30 * Random.Range(0, 7);

        for (int i = 0; i < numberRuneMarks; i++)
        {
            float angle = ((360 / numberRuneMarks) * i + Random.Range(-10, 10) + offset) * Mathf.Deg2Rad;

            Vector3 positionSpawn = position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(1.75f, 2.5f);
            LayerMask mask = new LayerMask();
            mask.value = 12;

            if (Physics2D.OverlapCircle(positionSpawn, 0.5f, mask));
            {
                GameObject newRuneMark = Instantiate(runeMark, positionSpawn, Quaternion.Euler(0, 0, Random.Range(0, 4) * 90));
                spawnedRuneMarks.Add(newRuneMark);

                if(spawnedRuneMarks.Count > 150)
                {
                    Destroy(spawnedRuneMarks[0]);
                    spawnedRuneMarks.Remove(spawnedRuneMarks[0]);
                }

                numberRuneMarksSpawned++;
                if(numberRuneMarksSpawned >= 3)
                {
                    return;
                }
            }
        }
    }

    private void SpawnUseArtifactSoulTrail()
    {
        PlayerProperties.playerScript.FlashWhitePickup();

        foreach (GameObject soulTrailInstant in spawnedSoulTrails)
        {
            if (soulTrailInstant.activeSelf == false)
            {
                soulTrailInstant.transform.position = PlayerProperties.playerShipPosition;
                soulTrailInstant.SetActive(true);
                StartCoroutine(randomFollowRoutine(soulTrailInstant, pickRandomPositionAroundShip()));
                return;
            }
        }

        GameObject newSoulTrail = Instantiate(soulTrail, PlayerProperties.playerShipPosition, Quaternion.identity);
        spawnedSoulTrails.Add(newSoulTrail);
        StartCoroutine(randomFollowRoutine(newSoulTrail, pickRandomPositionAroundShip()));
    }

    IEnumerator randomFollowRoutine(GameObject trail, Vector3 position)
    {
        TrailRenderer trailRenderer = trail.GetComponent<TrailRenderer>();
        trailRenderer.startWidth = 0.4f;
        float startSpeed = Random.Range(2, 10);
        float speed = startSpeed;

        LeanTween.value(startSpeed, 20, 3f).setOnUpdate((float val) => { speed = val; }).setEaseOutCirc();

        while (Vector2.Distance(trail.transform.position, position) > 0.5f)
        {
            float angle = Mathf.Atan2(position.y - trail.transform.position.y, position.x - trail.transform.position.x) + Mathf.PI/2;
            trail.transform.position += (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) + (position - trail.transform.position)).normalized * speed * Time.deltaTime;
            yield return null;
        }

        LeanTween.value(trailRenderer.startWidth, 0, 0.25f).setOnUpdate((float val) => { trailRenderer.startWidth = val; });
        trail.GetComponent<Animator>().SetTrigger("Dissapear");

        yield return new WaitForSeconds(0.5f);

        trail.SetActive(false);
    }
}
