using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderGodsBowZap : MonoBehaviour
{
    CircleCollider2D circCol;
    public GameObject splash;
    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        circCol.enabled = false;
        StartCoroutine(zap());
    }

    IEnumerator zap()
    {
        yield return new WaitForSeconds(1f / 12f);
        circCol.enabled = true;
        GameObject splashInstant = Instantiate(splash, transform.position, Quaternion.identity);
        splashInstant.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 3;
        yield return new WaitForSeconds(2f / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(5 / 12f);
        Destroy(this.gameObject);
    }
}
