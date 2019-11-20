using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRoomDoorBlock : MonoBehaviour
{
    public int numberSwitchesActivated;
    public GameObject doorBlock;
    public void addSwitch()
    {
        numberSwitchesActivated++;
    }

    private void Update()
    {
        if(numberSwitchesActivated >= 3)
        {
            doorBlock.GetComponent<DoorSeal>().open = true;
            Destroy(this);
        }

        if(FindObjectOfType<Artifacts>().numKills <= 0)
        {
            FindObjectOfType<Artifacts>().numKills = 15;
        }
    }
}
