using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorousTangleFiend : Enemy
{
    public List<CarnivorousTangleFiendFlowerTurret> turrets;
    public List<CarnivorousTangleFiendSpikeFlower> spikeFlowers;
    BossHealthBar healthBar;
    private float attackPeriod = 2;
    public BossManager bossManager;
    public Animator roomAnimator;

    int numberTurretAttacks = 0;

    void Start()
    {
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("Carnivorous Tangle Fiend");
        healthBar.targetEnemy = this;
        StartCoroutine(attackCycle());
    }

    IEnumerator attackCycle()
    {
        while (true)
        {
            if(attackPeriod > 0)
            {
                attackPeriod -= Time.deltaTime;
            }
            else
            {
                closeAllSpikeFlowers();
                if(numberTurretAttacks < 2 && turrets.Count > 0)
                {
                    turretAttack();
                    numberTurretAttacks++;
                }
                else if(spikeFlowers.Count > 0)
                {
                    spikeFlowerAttack();
                    numberTurretAttacks = 0;
                }
            }
            yield return null;
        }
    }

    void spikeFlowerAttack()
    {
        CarnivorousTangleFiendSpikeFlower flower = spikeFlowers[Random.Range(0, spikeFlowers.Count)];
        flower.openFlower();
        attackPeriod = 3;
    }

    void turretAttack()
    {
        if(turrets.Count > 2)
        {
            List<CarnivorousTangleFiendFlowerTurret> pickedTurrets = new List<CarnivorousTangleFiendFlowerTurret>();
            CarnivorousTangleFiendFlowerTurret turret = turrets[Random.Range(0, turrets.Count)];
            pickedTurrets.Add(turret);
            for(int i = 0; i < 100; i++)
            {
                if (pickedTurrets.Contains(turret))
                {
                    turret = turrets[Random.Range(0, turrets.Count)];
                }
                else
                {
                    break;
                }
            }
            pickedTurrets.Add(turret);

            foreach(CarnivorousTangleFiendFlowerTurret tur in pickedTurrets)
            {
                tur.spitSeed(2);
            }
            attackPeriod = 2.5f;
        }
        else
        {
            if (turrets.Count == 1)
            {
                turrets[0].spitSeed(4);
                attackPeriod = 3.8f;
            }
            else
            {
                foreach (CarnivorousTangleFiendFlowerTurret tur in turrets)
                {
                    tur.spitSeed(3);
                }
                attackPeriod = 3.2f;
            }
        }
    }
    
    void closeAllSpikeFlowers()
    {
        foreach(CarnivorousTangleFiendSpikeFlower flower in spikeFlowers)
        {
            flower.closeFlower();
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        bossManager.bossBeaten(nameID, 1.083f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        roomAnimator.SetTrigger("Death");
        SaveSystem.SaveGame();
    }

    public override void damageProcedure(int damage)
    {
        
    }

    public void updateDamageToBoss(int damage, int currentHealth)
    {
        if(damage > currentHealth)
        {
            dealDamage(currentHealth);
        }
        else
        {
            dealDamage(damage);
        }
    }
}
