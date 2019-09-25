using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinalFistSmash : MonoBehaviour
{
    CircleCollider2D circCol;

    IEnumerator turnOffCollider()
    {
        yield return new WaitForSeconds(3f / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(4f / 12f);
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(5f / 12f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        circCol = GetComponent<CircleCollider2D>();
        StartCoroutine(turnOffCollider());
    }
}
