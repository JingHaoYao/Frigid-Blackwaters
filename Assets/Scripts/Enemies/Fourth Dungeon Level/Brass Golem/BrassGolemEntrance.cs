using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemEntrance : MonoBehaviour
{
    [SerializeField] BrassGolemBossManager brassGolemBossManager;
    bool alreadyLoaded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alreadyLoaded == false)
        {
            alreadyLoaded = true;
            brassGolemBossManager.startMovingPlayer();
        }
    }
}
