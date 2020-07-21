using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GildedCompass : ArtifactBonus
{
    public static bool mapRevealedAlready = false;

    MapSpawn mapSpawn;
    RoomMemory roomMemory;
    List<int> filter = new List<int> { 2, 3, 7, 12 };
    [SerializeField] DisplayItem displayItem;
    [SerializeField] AudioSource activateAudio;

    private void Start()
    {
        mapSpawn = FindObjectOfType<MapSpawn>();
        roomMemory = FindObjectOfType<RoomMemory>();
    }

    public void revealAllUniqueTiles()
    {
        PlayerProperties.playerArtifacts.numKills -= killRequirement;
        activateAudio.Play();
        foreach(MapExploration tile in mapSpawn.tileList)
        {
            if (!filter.Contains(roomMemory.roomID[tile.xID, tile.yID]))
            {
                tile.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    revealAllUniqueTiles();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    revealAllUniqueTiles();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    revealAllUniqueTiles();
                }
            }
        }
    }
}
