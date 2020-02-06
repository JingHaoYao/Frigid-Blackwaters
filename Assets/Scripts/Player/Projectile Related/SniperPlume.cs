using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperPlume : WeaponFireScript {
    GameObject playerShip;
    float baseAngle = 0;
    public LayerMask bulletImpactLayerMask;
    public bool extremeFocus = false;
    public int numberExtremeFocusShots = 2;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void summonBullet()
    {

        if (extremeFocus == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos(baseAngle * Mathf.Deg2Rad), Mathf.Sin(baseAngle * Mathf.Deg2Rad)).normalized, 60, bulletImpactLayerMask);
            GameObject bulletInstant = null;

            if (hit.collider != null)
            {
                bulletInstant = Instantiate(bullet, hit.point, Quaternion.Euler(0, 0, baseAngle + 180));
                if (PlayerUpgrades.sniperUpgrades.Contains("target_weaknesses_unlocked"))
                {
                    if (hit.collider.GetComponent<Enemy>())
                    {
                        if (hit.collider.GetComponent<Enemy>().stopAttacking == true)
                        {
                            bulletInstant.GetComponent<DamageAmount>().damage += 5;
                        }
                    }
                }
            }

            triggerWeaponFireFlag(new GameObject[1] { bulletInstant });
        }
        else
        {
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            if (allEnemies.Length == 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos(baseAngle * Mathf.Deg2Rad), Mathf.Sin(baseAngle * Mathf.Deg2Rad)).normalized, 20, bulletImpactLayerMask);
                GameObject bulletInstant = null;

                if (hit.collider != null)
                {
                    bulletInstant = Instantiate(bullet, hit.point, Quaternion.Euler(0, 0, baseAngle + 180));
                }
            }
            else
            {
                List<Enemy> hitEnemies = new List<Enemy>();
                List<GameObject> firedBullets = new List<GameObject>();
                float closestDistance = int.MaxValue;
                Enemy targetEnemy = null;
                for(int i = 0; i < numberExtremeFocusShots; i++)
                {
                    foreach(Enemy enemy in allEnemies)
                    {
                        if(Vector2.Distance(enemy.transform.position, FindObjectOfType<CursorTarget>().transform.position) < closestDistance && !hitEnemies.Contains(enemy))
                        {
                            closestDistance = Vector2.Distance(enemy.transform.position, FindObjectOfType<CursorTarget>().transform.position);
                            targetEnemy = enemy;
                        }
                    }
                    hitEnemies.Add(targetEnemy);
                    GameObject bulletInstant = Instantiate(bullet, targetEnemy.transform.position + new Vector3(0, 0.4f, 0), Quaternion.Euler(0, 0, baseAngle + 180));
                    firedBullets.Add(bulletInstant);
                    closestDistance = int.MaxValue;
                    triggerWeaponFireFlag(firedBullets.ToArray());
                }
            }
        }
    }

    IEnumerator waitForAudio()
    {
        yield return new WaitForSeconds(animLength / 3f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animLength = weaponFire.length;
        StartCoroutine(waitForAudio());
        initShipPos = GameObject.Find("PlayerShip").transform.position;
        initFirePos = transform.position;
        playerShip = GameObject.Find("PlayerShip");
        baseAngle = pickDirectionTravel();
        summonBullet();
    }

    void Update()
    {
        transform.position = initFirePos + (GameObject.Find("PlayerShip").transform.position - initShipPos);
        pickRendererLayer();
    }
}
