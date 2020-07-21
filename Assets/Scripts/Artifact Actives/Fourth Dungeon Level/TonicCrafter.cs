using UnityEngine;

public class TonicCrafter : ArtifactBonus
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject healingTonicItem;
    [SerializeField] AudioSource bubbleAudio;
    
    void grantTonicItem()
    {
        if (PlayerProperties.playerInventory.itemList.Count < PlayerItems.maxInventorySize)
        {
            bubbleAudio.Play();
            PlayerProperties.playerArtifacts.numKills -= killRequirement;
            GameObject healingTonicInstant = Instantiate(healingTonicItem);
            PlayerProperties.playerInventory.itemList.Add(healingTonicInstant);
            PlayerProperties.playerInventory.UpdateUI();
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
                    grantTonicItem();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    grantTonicItem();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    grantTonicItem();
                }
            }
        }
    }
}
