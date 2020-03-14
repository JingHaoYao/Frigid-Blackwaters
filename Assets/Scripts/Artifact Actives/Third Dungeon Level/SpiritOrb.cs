using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOrb : MonoBehaviour
{
    [SerializeField] private Collider2D collider;

    public EtherealSconce sconce;

    [SerializeField] private AudioSource audioSource;

    float timePeriodUntilOut = 0;

    void addBonuses()
    {
        audioSource.Play();
        collider.enabled = false;
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
        sconce.addBonus();
    }

    private void Update()
    {
        if (timePeriodUntilOut >= 4 && this.collider.enabled)
        {
            collider.enabled = false;
            LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
        }
        else
        {
            timePeriodUntilOut += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            addBonuses();
        }
    }
}
