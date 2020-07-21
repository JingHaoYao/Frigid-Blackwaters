using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystalSample : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource audioSource;
    bool activated = false;

    private void Start()
    {
        consumableBonus.SetAction(addKills);
    }

    void addKills()
    {
        if (activated == false)
        {
            activated = true;
            PlayerProperties.playerArtifacts.numKills = Mathf.Clamp(PlayerProperties.playerArtifacts.numKills + 6, 0, 20);
            audioSource.Play();
            Destroy(this.gameObject, 1f);
        }
    }
}
