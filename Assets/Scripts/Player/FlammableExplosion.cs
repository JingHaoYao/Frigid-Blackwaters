using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableExplosion : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        Destroy(this.gameObject, 1f);    
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder + 5;
        transform.position = PlayerProperties.playerShipPosition;
    }
}
