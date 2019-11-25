using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBossCrystal : MonoBehaviour
{
    public GameObject projectile;
    Animator animator;
    public float currAngle;
    public Sprite unactive;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void attack(int whichAttack)
    {
        StartCoroutine(launchAttack(whichAttack));
    }

    IEnumerator launchAttack(int whichAttack)
    {
        animator.SetTrigger("Shine");
        yield return new WaitForSeconds(9f / 12f);
        if (whichAttack == 0)
        {
            GetComponent<AudioSource>().Play();
            for (int i = 0; i < 4; i++)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<BasicProjectile>().angleTravel = i * 90;
                shot.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
            }
        }
        else if (whichAttack == 1)
        {
            GetComponent<AudioSource>().Play();
            for (int i = 0; i < 4; i++)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<BasicProjectile>().angleTravel = i * 90 + 45;
                shot.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
            }
        }
        else if (whichAttack == 2)
        {
            GetComponent<AudioSource>().Play();
            for (int i = 0; i < 8; i++)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<BasicProjectile>().angleTravel = i * 45;
                shot.GetComponent<DamageHitBox>().damageAmount = 50;
                shot.transform.localScale = new Vector3(2.5f, 2.5f, 0);
                shot.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
            }
        }
        else if (whichAttack == 3)
        {
            float targetAngle = currAngle;
            for(int i = 0; i < 3; i++)
            {
                for(int k = 0; k < 2; k++)
                {
                    GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                    shot.GetComponent<BasicProjectile>().angleTravel = targetAngle - 15 + 15 * i;
                    shot.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
                    GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(0.2f);
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (whichAttack == 4)
        {
            float targetAngle = currAngle;
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                    shot.GetComponent<BasicProjectile>().angleTravel = targetAngle + 15 - 15 * i;
                    shot.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
                    GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(0.2f);
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (whichAttack == 5)
        {
            for (int k = 0; k < 3; k++)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                GameObject shot2 = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<BasicProjectile>().angleTravel = 270;
                shot.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
                shot2.GetComponent<BasicProjectile>().angleTravel = 90;
                shot2.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if(whichAttack == 6)
        {
            for (int k = 0; k < 3; k++)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                GameObject shot2 = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<BasicProjectile>().angleTravel = 180;
                shot.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
                shot2.GetComponent<BasicProjectile>().angleTravel = 0;
                shot2.GetComponent<ProjectileParent>().instantiater = this.transform.parent.gameObject;
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(0.2f);
            }
        }
        animator.SetTrigger("Idle");
        GetComponent<SpriteRenderer>().sprite = unactive;
    }

}
