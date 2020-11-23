using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanseFlammabilityConsumable : MonoBehaviour
{
    [SerializeField] ConsumableBonus consumableBonus;
    [SerializeField] private int numberStacksToCleanse;

    private void Start()
    {
        consumableBonus.SetAction(CleanseStacks);
    }

    void CleanseStacks()
    {
        if (PlayerProperties.flammableController != null)
        {
            for (int i = 0; i < numberStacksToCleanse; i++)
            {
                PlayerProperties.flammableController.RemoveFlammableStack();
            }
        }
    }
}
