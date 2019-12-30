using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormantOysterBoss : MonoBehaviour
{
    public GameObject oysterBoss;
    bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated == false && collision.gameObject.GetComponent<DamageAmount>())
        {
            activated = true;
            GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer.spawnDoorSeals();
            FindObjectOfType<PlayerScript>().enemiesDefeated = false;
            GameObject oyster = Instantiate(oysterBoss, transform.position, Quaternion.identity);
            oyster.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
            Destroy(this.gameObject);
        }
    }
}
