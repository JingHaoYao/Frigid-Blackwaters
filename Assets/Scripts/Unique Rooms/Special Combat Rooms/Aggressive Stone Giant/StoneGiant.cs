using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGiant : Enemy {
    SpriteRenderer spriteRenderer;
    public AntiSpawnSpaceDetailer anti;
    public GameObject fistAttack, slamAttack, warningCircle;
    GameObject playerShip;
    bool leftFistActive = false, rightFistActive = false;
    GameObject leftFist, rightFist;
    int numAttacksSlam = 0;
    public GameObject deadGiant;
    public GameObject giantChest;
    
    IEnumerator summonSlamAttack(Vector3 pos)
    {
        if(Mathf.Abs(pos.x - transform.position.x) > 2f)
        {
            if(Random.Range(0,2) == 1)
            {
                Instantiate(warningCircle, pos + new Vector3(-1.6f, 0.6f, 0), Quaternion.identity);
                Instantiate(warningCircle, pos + new Vector3(1.6f, -0.6f, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
                GameObject leftSlam = Instantiate(slamAttack, pos + new Vector3(-1.6f, 0.6f, 0), Quaternion.identity);
                GameObject rightSlam = Instantiate(slamAttack, pos + new Vector3(1.6f, -0.6f, 0), Quaternion.identity);
                leftSlam.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                rightSlam.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                rightSlam.transform.localScale = new Vector3(-0.5f, 0.5f, 0);
            }
            else
            {
                Instantiate(warningCircle, pos + new Vector3(-1.6f, -0.6f, 0), Quaternion.identity);
                Instantiate(warningCircle, pos + new Vector3(1.6f, 0.6f, 0), Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
                GameObject leftSlam = Instantiate(slamAttack, pos + new Vector3(-1.6f, -0.6f, 0), Quaternion.identity);
                GameObject rightSlam = Instantiate(slamAttack, pos + new Vector3(1.6f, 0.6f, 0), Quaternion.identity);
                leftSlam.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                rightSlam.GetComponent<ProjectileParent>().instantiater = this.gameObject;
                rightSlam.transform.localScale = new Vector3(-0.5f, 0.5f, 0);
            }
        }
        else
        {
            Instantiate(warningCircle, pos + new Vector3(-5.46f, 0, 0), Quaternion.identity);
            Instantiate(warningCircle, pos + new Vector3(5.46f, 0, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            GameObject leftSlam = Instantiate(slamAttack, pos + new Vector3(-5.46f, 0, 0), Quaternion.identity);
            GameObject rightSlam = Instantiate(slamAttack, pos + new Vector3(5.46f, 0, 0), Quaternion.identity);
            leftSlam.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            rightSlam.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            rightSlam.transform.localScale = new Vector3(-0.5f, 0.5f, 0);
        }
        yield return new WaitForSeconds(1.35f);
        leftFistActive = false;
        rightFistActive = false;
    }

    IEnumerator summonLeftFist(Vector3 pos)
    {
        yield return new WaitForSeconds(0.5f);
        leftFist = Instantiate(fistAttack, pos, Quaternion.identity);
        leftFist.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        yield return new WaitForSeconds(1.1f);
        leftFistActive = false;
    }

    IEnumerator summonRightFist(Vector3 pos)
    {
        yield return new WaitForSeconds(0.5f);
        rightFist = Instantiate(fistAttack, pos, Quaternion.identity);
        rightFist.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        rightFist.transform.localScale = new Vector3(-0.5f, 0.5f, 0);
        yield return new WaitForSeconds(1.1f);
        rightFistActive = false;
    }

    void Start () {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        playerShip = GameObject.Find("PlayerShip");
        FindObjectOfType<BossHealthBar>().bossStartUp("Awakened Stone Giant");
        FindObjectOfType<BossHealthBar>().targetEnemy = this;
        EnemyPool.addEnemy(this);
    }

    void pickAttack()
    {
        if (numAttacksSlam < 4)
        {
            if (leftFistActive == false && playerShip.transform.position.x < transform.position.x)
            {
                Vector3 pos = playerShip.transform.position;
                Instantiate(warningCircle, pos, Quaternion.identity);
                StartCoroutine(summonLeftFist(pos));
                leftFistActive = true;
                numAttacksSlam++;
            }
            
            if(rightFistActive == false && playerShip.transform.position.x > transform.position.x)
            {
                Vector3 pos = playerShip.transform.position;
                Instantiate(warningCircle, pos, Quaternion.identity);
                StartCoroutine(summonRightFist(pos));
                rightFistActive = true;
                numAttacksSlam++;
            }
        }
        else
        {
            if(leftFistActive == false && rightFistActive == false)
            {
                leftFistActive = true;
                rightFistActive = true;
                numAttacksSlam = 0;
                StartCoroutine(summonSlamAttack(playerShip.transform.position));
            } 
        }
    }

	void Update () {
        pickAttack();
	}

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        GameObject spawnedDeadGiant = Instantiate(deadGiant, transform.position, Quaternion.identity);
        SpriteRenderer[] deadRends = spawnedDeadGiant.GetComponentsInChildren<SpriteRenderer>();
        FindObjectOfType<BossHealthBar>().bossEnd();
        foreach (SpriteRenderer element in deadRends)
        {
            element.sortingOrder = spriteRenderer.sortingOrder;
        }
        anti.trialDefeated = true;
        Destroy(this.gameObject);
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
        Instantiate(giantChest, transform.position + new Vector3(0, -3, 0), Quaternion.identity);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        SpawnArtifactKillsAndGoOnCooldown(1);
    }
}
