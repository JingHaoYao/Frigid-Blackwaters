using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilaPlantPickup : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] int healAmount = 0;

    private void Start()
    {
        StartCoroutine(waitUntilDissapear());
    }

    IEnumerator waitUntilDissapear()
    {
        yield return new WaitForSeconds(3f);
        collider.enabled = false;
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
    }

    void heal()
    {
        collider.enabled = false;
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
        PlayerProperties.playerScript.healPlayer(healAmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 && collision.gameObject.tag == "playerHitBox")
        {
            heal();
        }
    }
}
