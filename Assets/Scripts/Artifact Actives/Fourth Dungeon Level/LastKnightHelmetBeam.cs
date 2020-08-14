using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKnightHelmetBeam : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCol;

    void Start()
    {
        StartCoroutine(beamProcedure());
    }

    IEnumerator beamProcedure()
    {
        yield return new WaitForSeconds(3 / 12f);
        boxCol.enabled = true;
        yield return new WaitForSeconds(4 / 12f);
        boxCol.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        Destroy(this.gameObject);
    }
}
