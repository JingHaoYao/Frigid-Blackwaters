using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamantoiseLeg : MonoBehaviour
{
    [SerializeField] private AudioSource splashAudio;
    [SerializeField] private GameObject damageHitBox1, damageHitBox2;
    [SerializeField] private Animator animator1, animator2;
    [SerializeField] GameObject rockProjectile;
    public GameObject adamantoiseBoss;
    [SerializeField] LayerMask layermask;
    public Camera mainCamera;
    public CameraShake cameraShake;


    IEnumerator smashProcedure()
    {
        animator1.SetTrigger("Smash");
        animator2.SetTrigger("Smash");
        yield return new WaitForSeconds(5 / 12f);
        splashAudio.Play();
        damageHitBox1.SetActive(true);
        damageHitBox2.SetActive(true);
        StartCoroutine(rockFallProcedure());
        yield return new WaitForSeconds(1 / 12f);
        damageHitBox1.SetActive(false);
        damageHitBox2.SetActive(false);
        yield return new WaitForSeconds(4 / 12f);
    }

    public void smash()
    {
        StartCoroutine(smashProcedure());
    }

    public void stopAllCoroutines()
    {
        this.StopAllCoroutines();
        damageHitBox1.SetActive(false);
        damageHitBox2.SetActive(false);
    }

    IEnumerator rockFallProcedure()
    {
        cameraShake.shakeCamFunction(0.5f, 0.2f);
        for(int i = 0; i < 3; i++)
        {
            GameObject instant = Instantiate(rockProjectile, PlayerProperties.playerShipPosition, Quaternion.identity);
            instant.GetComponent<ProjectileParent>().instantiater = adamantoiseBoss;
            for (int k = 0; k < 4; k++)
            {
                instant = Instantiate(rockProjectile, pickRandomPosition(), Quaternion.identity);
                instant.GetComponent<ProjectileParent>().instantiater = adamantoiseBoss;
            }
            yield return new WaitForSeconds(0.75f);
        }
    }

    Vector3 pickRandomPosition()
    {
        Vector3 randPos = new Vector3(Random.Range(mainCamera.transform.position.x - 8.5f, mainCamera.transform.position.x + 8.5f), Random.Range(mainCamera.transform.position.y - 8.5f, mainCamera.transform.position.y + 8.5f));
        while (Physics2D.OverlapCircle(randPos, 0.5f, layermask))
        {
            randPos = new Vector3(Random.Range(mainCamera.transform.position.x - 8.5f, mainCamera.transform.position.x + 8.5f), Random.Range(mainCamera.transform.position.y - 8.5f, mainCamera.transform.position.y + 8.5f));
        }
        return randPos;
    }
}
