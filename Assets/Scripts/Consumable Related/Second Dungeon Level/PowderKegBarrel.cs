using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderKegBarrel : MonoBehaviour
{
    public GameObject explosion;

    IEnumerator explodeMultiple()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < 5; i++)
        {
            Instantiate(explosion, transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f)), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }
    void Start()
    {
        StartCoroutine(explodeMultiple());
    }
}
