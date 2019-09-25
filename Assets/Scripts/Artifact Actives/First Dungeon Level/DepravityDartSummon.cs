using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepravityDartSummon : MonoBehaviour
{
    public GameObject depravityDart;

    IEnumerator summonDart()
    {
        yield return new WaitForSeconds(3f / 12f);
        for(int i = 0; i < 8; i++)
        {
            GameObject instant = Instantiate(depravityDart, transform.position, Quaternion.identity);
            instant.GetComponent<DepravityDart>().angleTravel = i * 45;
        }
        yield return new WaitForSeconds(3 / 12f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(summonDart());
    }

    void Update()
    {
        
    }
}
