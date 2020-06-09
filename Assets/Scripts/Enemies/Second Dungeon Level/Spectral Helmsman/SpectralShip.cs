using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralShip : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject damagingSplash;
    [SerializeField] AudioSource firingAudio;
    public SpectralHelmsman spectralHelmsman;
    Coroutine shipRoutineInstant;

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        shipRoutineInstant = StartCoroutine(shipRoutine());
    }

    public void sink()
    {
        StopCoroutine(shipRoutineInstant);
        animator.SetTrigger("Sink");
        Destroy(this.gameObject, 7/12f);
    }

    IEnumerator shipRoutine()
    {
        yield return new WaitForSeconds(8 / 12f);

        List<int> cannonPositions = new List<int>();
        cannonPositions.Add(Random.Range(-7, 7));
        int pos2 = Random.Range(-7, 7);
        while (cannonPositions.Contains(pos2))
        {
            pos2 = Random.Range(-7, 7);
        }
        cannonPositions.Add(pos2);

        int pos3 = Random.Range(-7, 7);
        while (cannonPositions.Contains(pos3))
        {
            pos3 = Random.Range(-7, 7);
        }
        cannonPositions.Add(pos3);

        if (Random.Range(0, 2) == 1)
        {
            for (int i = -6; i <= 4; i++)
            {
                animator.SetTrigger("Fire");
                firingAudio.Play();
                shootLine(i * 1.25f, cannonPositions[0], cannonPositions[1], cannonPositions[2]);
                yield return new WaitForSeconds(0.6f);
            }
        }
        else
        {
            for (int i = 4; i >= -6; i--)
            {
                animator.SetTrigger("Fire");
                firingAudio.Play();
                shootLine(i * 1.25f, cannonPositions[0], cannonPositions[1], cannonPositions[2]);
                yield return new WaitForSeconds(0.6f);
            }
        }

        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(7 / 12f);
        Destroy(this.gameObject);
    }

    void shootLine(float whatIndexFromCamera, int cannonPos1, int cannonPos2, int cannonPos3)
    {
        GameObject instant = Instantiate(damagingSplash, mainCamera.transform.position + new Vector3(cannonPos1, whatIndexFromCamera), Quaternion.identity);
        instant.GetComponent<ProjectileParent>().instantiater = spectralHelmsman.gameObject;
        instant = Instantiate(damagingSplash, mainCamera.transform.position + new Vector3(cannonPos2, whatIndexFromCamera), Quaternion.identity);
        instant.GetComponent<ProjectileParent>().instantiater = spectralHelmsman.gameObject;
        instant = Instantiate(damagingSplash, mainCamera.transform.position + new Vector3(cannonPos3, whatIndexFromCamera), Quaternion.identity);
        instant.GetComponent<ProjectileParent>().instantiater = spectralHelmsman.gameObject;
    }
}
