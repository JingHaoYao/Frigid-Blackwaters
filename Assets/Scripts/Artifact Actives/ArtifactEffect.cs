using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactEffect : MonoBehaviour
{
    public virtual void addedKill(string tag, Vector3 deathPos, Enemy enemy) { }
    // Whenever the player takes damage
    public virtual void tookDamage(int amountDamage, Enemy enemy) { }
    // Whenever the player fires the left weapon, and so on
    public virtual void firedLeftWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel) { }
    public virtual void firedFrontWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel) { }
    public virtual void firedRightWeapon(GameObject[] bullet, Vector3 spawnPosition, float angleTravel) { }
    // Whenever the player enters a previously unentered room
    public virtual void exploredNewRoom(int whatRoomType) { }
    // Whenever the player picks up an item (updates the inventory)
    public virtual void updatedInventory() { }
    // whenever the player dashes
    public virtual void playerDashed() { }

    public virtual void dealtDamage(int damageDealt, Enemy enemy) { }

    public virtual void healed(int healingAmount) { }
}
