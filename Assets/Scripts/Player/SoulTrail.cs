using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulTrail : MonoBehaviour
{
    [SerializeField] GameObject particles;

    private void OnEnable()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}
