
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
    public static float currentPlayerTravelDirection;
    public static Vector3 shipTravellingVector;

    public static PlayerArmorEffect armorIndicator;
    public static DurationUI durationUI;
    public static ShipStats shipStats;
    public static ToolTip toolTip;
    public static ArtifactToolTip artifactToolTip;
    public static ConsumableToolTip consumableToolTip;

    public static ShipWeaponScript leftWeapon;
    public static ShipWeaponScript rightWeapon;
    public static ShipWeaponScript frontWeapon;

    public static Vector3 mainCameraPosition;

    public static SoulTrailSpawner soulTrailSpawner;
    public static CameraShake cameraShake;

    public static FlammableController flammableController;
    public static ArticraftingDisenchantingMenu articraftingDisenchantingMenu;
    public static ArticraftingCraftingMenu articraftingCraftingMenu;

    public static AudioManager audioManager;

    public static ItemTemplates itemTemplates;

    public static TutorialWidgetMenu tutorialWidgetMenu;

    public static PauseMenu pauseMenu;

}
