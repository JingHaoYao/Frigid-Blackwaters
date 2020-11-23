using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderFrostMageSummoningCircle : MonoBehaviour
{
    IEnumerator waitToSpawn()
    {
        yield return new WaitForSeconds(15f / 12);
        LeanTween.alpha(this.gameObject, 0, 1f);
        Destroy(this.gameObject, 1f);
    }

    private void Start()
    {
        StartCoroutine(waitToSpawn());
    }
}
