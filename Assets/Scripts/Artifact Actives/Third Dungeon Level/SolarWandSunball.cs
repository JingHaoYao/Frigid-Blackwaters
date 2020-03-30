using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarWandSunball : MonoBehaviour
{
    public GameObject solarWandPellet;

    private void Start()
    {
        StartCoroutine(solarWandProcess());
    }

    IEnumerator solarWandProcess()
    {
        yield return new WaitForSeconds(4 / 12f);
        for(int i = 0; i < 36; i++)
        {
            float angleInDeg = i * 10;
            float angleInRad = angleInDeg * Mathf.Deg2Rad;
            GameObject pellet = Instantiate(solarWandPellet, transform.position + new Vector3(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad)), Quaternion.identity);
            pellet.GetComponent<BasicProjectile>().angleTravel = angleInDeg;
        }
        yield return new WaitForSeconds(5 / 12f);
        Destroy(this.gameObject);
    }
}
