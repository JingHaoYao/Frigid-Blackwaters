using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishWater : ArtifactEffect
{
    public override void exploredNewRoom(int whatRoomType)
    {
        if(PlayerProperties.flammableController != null)
        {
            PlayerProperties.flammableController.RemoveFlammableStack();
        }
    }
}
