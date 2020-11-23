using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrandedSword : ArtifactEffect
{
    [SerializeField] GameObject playerSwordProjectile;
    List<Transform> allSwordTransforms = new List<Transform>();
    private float offset;

    public override void artifactEquipped()
    {
        StartCoroutine(summonSwordsAndBeginLoop());
    }

    public override void artifactUnequipped()
    {
        StopAllCoroutines();
        foreach(Transform swordTransform in allSwordTransforms)
        {
            Destroy(swordTransform.gameObject);
        }
        allSwordTransforms.Clear();
    }

    IEnumerator summonSwordsAndBeginLoop()
    {
        offset = 0;
        for(int i = 0; i < 6; i++)
        {
            GameObject swordInstant = Instantiate(playerSwordProjectile, PlayerProperties.playerShipPosition, Quaternion.identity);
            allSwordTransforms.Add(swordInstant.transform);
        }

        while(true)
        {
            offset += Time.deltaTime * 4;
            if(offset > 2 * Mathf.PI)
            {
                offset = 0;
            }
            for(int i = 0; i < allSwordTransforms.Count; i++)
            {
                allSwordTransforms[i].position = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(offset + i * Mathf.PI / 3), Mathf.Sin(offset + i * Mathf.PI / 3));
                allSwordTransforms[i].rotation = Quaternion.Euler(0, 0, (i * 60) + (offset * Mathf.Rad2Deg) - 90);
            }
            yield return null;
        }
    }
}
