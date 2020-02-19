
using UnityEngine;

public class PlayerProperties
{
    // This class serves as a public reference point to various things about the player to streamline
    // performance.

    public static Vector3 cursorPosition;
    public static PlayerScript playerScript;
    public static GameObject playerShip;
    public static Vector3 playerShipPosition;
    public static Artifacts playerArtifacts;
    public static Inventory playerInventory;
    public static SpriteRenderer spriteRenderer;

    public static PlayerArmorEffect armorIndicator;
    public static DurationUI durationUI;

}
