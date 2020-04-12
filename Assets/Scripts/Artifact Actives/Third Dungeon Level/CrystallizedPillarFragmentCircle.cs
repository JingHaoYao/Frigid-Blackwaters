using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallizedPillarFragmentCircle : MonoBehaviour
{
    [SerializeField] int initialScale = 35;
    [SerializeField] int endScale = 2;
    [SerializeField] float timeToScale = 6;
    [SerializeField] private GameObject[] crystals;
    float currentScale;
    float previousScale;
    Camera mainCamera;
    [SerializeField] AudioSource audioLoop;

    bool checkIfPositionIsValid(Vector3 pos)
    {
        return Mathf.Abs(pos.x - mainCamera.transform.position.x) < 8.5f && Mathf.Abs(pos.y - mainCamera.transform.position.y) < 8.5f;
    }

    private void Start()
    {
        transform.localScale = new Vector3(initialScale, initialScale);
        previousScale = initialScale;
        currentScale = initialScale;
        mainCamera = Camera.main;
        startCircleEffect();
    }

    void startCircleEffect()
    {
        audioLoop.Play();
        LeanTween.value(0, 0.6f, 0.5f).setOnUpdate((float val) => { audioLoop.volume = val; });
        LeanTween.scale(this.gameObject, new Vector3(endScale, endScale), timeToScale).setOnComplete(() => tearDown());
        StartCoroutine(spawnCrystalLoop());
    }

    void tearDown()
    {
        LeanTween.value(0.6f, 0f, 0.5f).setOnUpdate((float val) => { audioLoop.volume = val; });
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
    }

    IEnumerator spawnCrystalLoop()
    {
        while (true)
        {
            currentScale = transform.localScale.x;
            if (Mathf.Abs(currentScale - previousScale) * 0.7f > 1)
            {
                previousScale = currentScale;
                for (int i = 0; i < 8; i++)
                {
                    Vector3 position = transform.position + new Vector3(Mathf.Cos((i * 45) * Mathf.Deg2Rad), Mathf.Sin((i * 45) * Mathf.Deg2Rad)) * (transform.localScale.x * 0.7f);
                    if (checkIfPositionIsValid(position))
                    {
                        GameObject crystalInstant = Instantiate(crystals[Random.Range(0, crystals.Length)], position, Quaternion.identity);
                        if (Random.Range(0, 2) == 1)
                        {
                            float scale = crystalInstant.transform.localScale.x;
                            crystalInstant.transform.localScale = new Vector3(scale * -1, scale);
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
