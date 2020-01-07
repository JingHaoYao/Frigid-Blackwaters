using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrustaceaCrystalShardCrystalObstacle : MonoBehaviour
{
    PolygonCollider2D polyCol;
    public CrustaceaCrystalShard shard;

    void Start()
    {
        polyCol = GetComponent<PolygonCollider2D>();
        StartCoroutine(crystalShardRoutine());
    }

    IEnumerator crystalShardRoutine()
    {
        yield return new WaitForSeconds(0.667f);
        polyCol.enabled = true;
        yield return new WaitForSeconds(8f);
        GetComponent<Animator>().SetTrigger("Shatter");
        GetComponent<AudioSource>().Play();
        polyCol.enabled = false;
        yield return new WaitForSeconds(0.417f);
        shard.crystals.Remove(this);
        Destroy(this.gameObject);
    }
}
