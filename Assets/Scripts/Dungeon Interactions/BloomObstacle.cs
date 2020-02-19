using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomObstacle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Sprite[] evolutionSprites;
    public GameObject bloomParticles;

    public void MoveToNextBloomState(int bloomProgress)
    {
        spriteRenderer.sprite = evolutionSprites[bloomProgress];
        Instantiate(bloomParticles, transform.position + Vector3.up * 0.75f, Quaternion.identity);
    }
}
