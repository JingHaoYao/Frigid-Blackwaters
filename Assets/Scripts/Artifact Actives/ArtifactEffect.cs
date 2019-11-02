using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArtifactEffect : MonoBehaviour
{
    public abstract void addedKill(string tag, Vector3 deathPos);
    // Whenever the player takes damage
    public abstract void tookDamage(int amountDamage, Enemy enemy);
    // Whenever the player fires the left weapon, and so on
    public abstract void firedLeftWeapon(GameObject[] bullet);
    public abstract void firedFrontWeapon(GameObject[] bullet);
    public abstract void firedRightWeapon(GameObject[] bullet);
    // Whenever the player enters a previously unentered room
    public abstract void exploredNewRoom(int whatRoomType);
    // Whenever the player picks up an item (updates the inventory)
    public abstract void updatedInventory();
    // whenever the player dashes
    public abstract void playerDashed();

    public abstract void dealtDamage(int damageDealt, Enemy enemy);
}
