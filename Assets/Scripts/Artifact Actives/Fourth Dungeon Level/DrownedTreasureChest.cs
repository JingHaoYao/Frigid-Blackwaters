using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WhichTreasureChest
{
    Golden,
    Violent,
    Healing
}

public class DrownedTreasureChest : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] WhichTreasureChest whichTreasureChest;
    [SerializeField] Animator animator;
    [SerializeField] PolygonCollider2D collider;
    KeyOfTheDrownedTreasure artifact;


    public void Initialize(KeyOfTheDrownedTreasure artifact)
    {
        this.artifact = artifact;
    }

    private void Start()
    {
        StartCoroutine(waitToRise());
    }

    IEnumerator waitToRise()
    {
        collider.enabled = false;
        yield return new WaitForSeconds(7 / 12f);
        collider.enabled = true;
    }

    public void Sink()
    {
        animator.SetTrigger("Sink");
        Destroy(this.gameObject, 9 / 12f);
        collider.enabled = false;
    }

    IEnumerator activateEffect()
    {
        audio.Play();
        artifact.activateTreasures(this.whichTreasureChest);
        animator.SetTrigger("Open");
        yield return new WaitForSeconds(11 / 12f);
        Sink();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerProperties.playerShip)
        {
            StartCoroutine(activateEffect());
            collider.enabled = false;
        }
    }
}
