using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityAcceleratorEngines : MonoBehaviour
{
    [SerializeField] Animator engineAnimator1, engineAnimator2;
    [SerializeField] BoxCollider2D damageBox1, damageBox2;
    [SerializeField] DamageAmount damageAmount1, damageAmount2;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem particlesystem1, particleSystem2;
    bool isBursting = false;

    public void Initialize()
    {
        damageBox1.enabled = false;
        damageBox2.enabled = false;
        isBursting = false;
        particlesystem1.Stop();
        particleSystem2.Stop();

        StartCoroutine(followAndRotate());
    }

    IEnumerator followAndRotate()
    {
        while(true)
        {
            transform.position = PlayerProperties.playerShipPosition;
            if (!isBursting)
            {
                transform.rotation = Quaternion.Euler(0, 0, PlayerProperties.playerScript.whatAngleTraveled - 90);
            }
            yield return null;
        }
    }

    public void StartBurst(float direction)
    {
        if (isBursting == false)
        {
            engineAnimator1.SetTrigger("Burst");
            engineAnimator2.SetTrigger("Burst");
            particlesystem1.Play();
            particleSystem2.Play();
            damageBox1.enabled = true;
            damageBox2.enabled = true;
            transform.rotation = Quaternion.Euler(0, 0, direction - 90);
            audioSource.Play();
            isBursting = true;
        }
    }

    public void StopBurst()
    {
        if (isBursting)
        {
            engineAnimator1.SetTrigger("Idle");
            engineAnimator2.SetTrigger("Idle");
            particlesystem1.Stop();
            particleSystem2.Stop();
            damageBox1.enabled = false;
            damageBox2.enabled = false;
            audioSource.Stop();
            isBursting = false;
        }
    }
    
    public void UpdateDamageOnColliders(int damage)
    {
        damageAmount1.originDamage = damage;
        damageAmount2.originDamage = damage;
        damageAmount1.updateDamage();
        damageAmount2.updateDamage();
    }
}
