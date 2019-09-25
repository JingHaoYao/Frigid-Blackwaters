using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpire : MonoBehaviour {
    GameObject obstacleToolTip;
    public Sprite broken1, broken2, broken3, drained;
    SpriteRenderer spriteRenderer;
    Animator animator;
    bool brokenCrystal = false;
    public GameObject aStarGrid, summonEffectParticles;
    AntiSpawnSpaceDetailer anti;
    GameObject spawnedGrid;
    bool summonedLittleGuys = false;
    public GameObject chestParticles;

	void Start () {
        obstacleToolTip = GameObject.Find("PlayerShip").GetComponent<PlayerScript>().obstacleToolTip;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        anti = GetComponent<WhichRoomManager>().antiSpawnSpaceDetailer;
        animator.enabled = false;
    }

	void Update () {
        if (brokenCrystal == true && anti.trialDefeated == false && summonedLittleGuys == true)
        {
            GameObject[] ActiveRangedEnemies = GameObject.FindGameObjectsWithTag("RangedEnemy");
            GameObject[] ActiveMeleeEnemies = GameObject.FindGameObjectsWithTag("MeleeEnemy");
            GameObject[] ActiveShieldEnemies = GameObject.FindGameObjectsWithTag("EnemyShield");
            if(ActiveRangedEnemies.Length == 0 && ActiveMeleeEnemies.Length == 0 && ActiveShieldEnemies.Length == 0)
            {
                GameObject instant = Instantiate(chestParticles, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                instant.GetComponent<CrystalParticles>().target = transform.position + new Vector3(0, -5f, 0);
                anti.trialDefeated = true;
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = true;
                if (spawnedGrid)
                {
                    Destroy(spawnedGrid);
                }
            }
        }
	}

    IEnumerator generateSkeletons()
    {
        GameObject.Find("PlayerShip").GetComponent<PlayerScript>().enemiesDefeated = false;
        for(int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 2; k++)
            {
                Vector3 randPos = new Vector3(transform.position.x + Random.Range(-8, 8), transform.position.y + Random.Range(-8, 8), 0);
                while(Physics2D.OverlapCircle(randPos, 0.5f))
                {
                    randPos = new Vector3(transform.position.x + Random.Range(-8, 8), transform.position.y + Random.Range(-8, 8), 0);
                }
                GameObject instant = Instantiate(summonEffectParticles, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                instant.GetComponent<CrystalParticles>().target = randPos;
            }
            yield return new WaitForSeconds(0.3f);
        }
        summonedLittleGuys = true;
    }

    IEnumerator changeToDrained()
    {
        animator.SetTrigger("Explode");
        this.GetComponents<AudioSource>()[1].Play();
        this.GetComponents<AudioSource>()[2].Play();
        yield return new WaitForSeconds(0.833f);
        animator.enabled = false;
        spriteRenderer.sprite = drained;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 16 && obstacleToolTip.activeSelf == false && brokenCrystal == false)
        {
            if (spriteRenderer.sprite != broken3)
            {
                if (spriteRenderer.sprite == broken1)
                {
                    spriteRenderer.sprite = broken2;
                }
                else if (spriteRenderer.sprite == broken2)
                {
                    spriteRenderer.sprite = broken3;
                }
                else
                {
                    spriteRenderer.sprite = broken1;
                }
                this.GetComponents<AudioSource>()[0].Play();
            }
            else
            {
                if (brokenCrystal == false)
                {
                    StartCoroutine(generateSkeletons());
                    spawnedGrid = Instantiate(aStarGrid, transform.position, Quaternion.identity);
                    brokenCrystal = true;
                    animator.enabled = true;
                    anti.trialDefeated = false;
                    anti.spawnDoorSeals();
                    StartCoroutine(changeToDrained());
                }
            }
        }
    }
}
