using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalamanderAlchemistFog : MonoBehaviour
{
    [SerializeField] CircleCollider2D circCol;

    IEnumerator fogProcedure()
    {
        for(int i = 0; i < 10; i++)
        {
            circCol.enabled = true;
            yield return new WaitForSeconds(0.1f);
            circCol.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(fogProcedure());
    }
}
