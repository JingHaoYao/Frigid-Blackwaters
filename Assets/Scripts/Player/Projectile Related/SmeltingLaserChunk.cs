using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmeltingLaserChunk : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] AudioSource pickUpAudio;

    private void Start()
    {
        LeanTween.move(this.gameObject, transform.position + Vector3.up * 0.5f, 0.3f).setEaseOutQuad().setOnComplete(() => LeanTween.move(this.gameObject, transform.position + Vector3.down * 0.5f, 0.3f).setEaseInQuad()); 
    }

    void upgradeLaser()
    {
        collider.enabled = false;
        LeanTween.alpha(this.gameObject, 0, 1f).setOnComplete(() => Destroy(this.gameObject));
        foreach(ShipWeaponScript script in PlayerProperties.playerScript.GetShipWeaponScripts())
        {
            SmeltingLaserUpgradeManager smeltingLaserUpgradeManager = script.shipWeaponTemplate.GetComponent<SmeltingLaserUpgradeManager>();
            if(smeltingLaserUpgradeManager != null)
            {
                smeltingLaserUpgradeManager.addFragment();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9 && collision.gameObject.tag == "playerHitBox")
        {
            upgradeLaser();
            pickUpAudio.Play();
        }
    }
}
