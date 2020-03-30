using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodAirEffect : MonoBehaviour
{
    public GameObject playerPodProjectile;
    public Enemy targetEnemy;

    private void Start()
    {
        Destroy(this.gameObject, 0.5f);
        GameObject podInstant = Instantiate(playerPodProjectile, transform.position, Quaternion.identity);
        podInstant.GetComponent<Thornball>().targetLocation = targetEnemy.transform.position;
    }
}
