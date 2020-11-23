using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyClimberFireWave : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Collider2D collider2D;
    
    public void Initialize(float rotation, GameObject instantiatingObject, Vector3 moveToPosition, float time)
    {
        ParticleSystem.MainModule mainModule = particleSystem.main;
        transform.rotation = Quaternion.Euler(0, 0, rotation - 90);
        projectileParent.instantiater = instantiatingObject;
        LeanTween.move(this.gameObject, moveToPosition, time).setOnComplete(() => { particleSystem.Stop(); collider2D.enabled = false; });
    }
}
