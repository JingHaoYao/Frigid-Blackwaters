using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRoomSwitch : MonoBehaviour
{
    public Sprite off;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && GetComponent<SpriteRenderer>().sprite != off)
        {
            GetComponent<SpriteRenderer>().sprite = off;
            FindObjectOfType<FirstRoomDoorBlock>().addSwitch();
            GetComponent<AudioSource>().Play();
        }
    }
}
