using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectiousMushroom : ArtifactEffect
{
    public GameObject infectiousMushroom;

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if (!enemy.containsStatus("Spore Infection Effect"))
        {
            GameObject infectiousMushroomInstant = Instantiate(infectiousMushroom, enemy.transform.position, Quaternion.identity);
            enemy.addStatus(infectiousMushroomInstant.GetComponent<SporeInfectionEffect>());
        }
    }
}
