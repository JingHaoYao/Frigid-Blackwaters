using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoungGoliathShroom : ArtifactEffect
{
    public GameObject mushroomSpray;

    void fireMushroomSpray(float angleTravel, Vector3 spawnPosition)
    {
        GameObject mushroomInstant = Instantiate(mushroomSpray, spawnPosition, Quaternion.Euler(0, 0, angleTravel));
        mushroomInstant.GetComponent<BasicProjectile>().angleTravel = angleTravel;
    }

    public override void firedFrontWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        fireMushroomSpray(angleTravel, spawnPosition);
    }

    public override void firedLeftWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        fireMushroomSpray(angleTravel, spawnPosition);
    }

    public override void firedRightWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel)
    {
        fireMushroomSpray(angleTravel, spawnPosition);
    }
}
