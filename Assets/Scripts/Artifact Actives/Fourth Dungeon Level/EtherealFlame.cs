using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherealFlame : MonoBehaviour
{
    [SerializeField] CircleCollider2D collider;
    public GatheringOfEtherealFlame artifact;

    private void Start()
    {
        StartCoroutine(flameProcess());
    }

    IEnumerator flameProcess()
    {
        transform.localScale = new Vector3(0, 0);
        LeanTween.scale(this.gameObject, new Vector3(2, 2), 0.5f);
        yield return new WaitForSeconds(3f);
        LeanTween.scale(this.gameObject, new Vector3(0, 0), 0.5f).setOnComplete(() => Destroy(this.gameObject));
        collider.enabled = false;
        artifact.removeFlame(this.gameObject);
    }
}
