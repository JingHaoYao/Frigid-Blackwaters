using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthLevelFogController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleSystems;
    [SerializeField] ParticleSystemRenderer[] renderers;
    public RoomTemplates roomTemplates;

    private void Start()
    {
        StartCoroutine(waitForSpawned());
        foreach (ParticleSystemRenderer renderer in renderers)
        {
            renderer.sharedMaterial.color = new Color(0.6226415f, 0.8121411f, 0.8301887f, 0.05882353f);
        }
    }

    IEnumerator waitForSpawned()
    {
        while(roomTemplates.areRoomsSpawned() != true)
        {
            yield return null;
        }

        foreach(AntiSpawnSpaceDetailer room in roomTemplates.antiList)
        {
            room.GetComponent<FogCycleRoom>()?.SetFogController(this);
        }
    }

    public void ActivateFog()
    {
        LeanTween.cancelAll(this.gameObject);
        foreach(ParticleSystemRenderer renderer in renderers)
        {
            LeanTween.value(0.05882353f, 0.05882353f * 2, 1f).setOnUpdate((float val) => renderer.sharedMaterial.color = new Color(0.6226415f, 0.8121411f, 0.8301887f, val));
        }
    }

    public void DeActivateFog()
    {
        LeanTween.cancelAll(this.gameObject);
        foreach (ParticleSystemRenderer renderer in renderers)
        {
            LeanTween.value(0.05882353f * 2, 0.05882353f, 1f).setOnUpdate((float val) => renderer.sharedMaterial.color = new Color(0.6226415f, 0.8121411f, 0.8301887f, val));
        }
    }
}
