using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NettleLayerSpikes : MonoBehaviour
{
    public float waitTimeUntilDecay = 2;
    [SerializeField] private Collider2D damageCollider;

    private void Start()
    {
        StartCoroutine(delay(waitTimeUntilDecay));
    }

    IEnumerator delay(float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnStart(() => damageCollider.enabled = false).setOnComplete(() => Destroy(this.gameObject));
    }
}
