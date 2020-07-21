using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedBlueCrystals : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;

    private void Start()
    {
        consumableBonus.SetAction(() => StartCoroutine(addArtifactKills()));
    }

    IEnumerator addArtifactKills()
    {
        PlayerProperties.durationUI.addTile(this.GetComponent<DisplayItem>().displayIcon, 10);

        for (int i = 0; i < 10; i++)
        {
            if (PlayerProperties.playerArtifacts.numKills < 20)
            {
                PlayerProperties.playerArtifacts.numKills++;
            }
            yield return new WaitForSeconds(2);
        }
        Destroy(this.gameObject);
    }
}
