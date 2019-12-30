using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormantLancerGolem : MonoBehaviour
{
    bool activated = false;
    public GameObject lancerGolem;
    public SwordFist[] swordFists;

    private void Start()
    {
        GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = transform.parent.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated == false && collision.gameObject.GetComponent<DamageAmount>())
        {
            activated = true;
            GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer.spawnDoorSeals();
            FindObjectOfType<PlayerScript>().enemiesDefeated = false;
            GameObject golem = Instantiate(lancerGolem, transform.position, Quaternion.identity);
            golem.GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer = GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
            golem.GetComponent<LancerGolem>().swordFists = swordFists;
            Destroy(this.gameObject);
        }
    }
}
