using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantLliliaceaeEntrance : MonoBehaviour
{
    public MutantLiliaceaeBossManager bossManager;
    private bool engaged = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 && engaged == false)
        {
            engaged = true;
            bossManager.InitiateBossFromCheckpoint();
        }
    }
}
