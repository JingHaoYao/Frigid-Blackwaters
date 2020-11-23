using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableController : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    private int flammableCount = 0;
    public SpriteMask shipSpriteMask;
    [SerializeField] GameObject explosion;
    [SerializeField] FlammableStackUI flammableStackUI;
    private float damageMultiplier = 0.1f;

    public void UpdateDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public float GetDamageMultiplier
    {
        get
        {
            return damageMultiplier;
        }
    }

    private void Awake()
    {
        PlayerProperties.flammableController = this;
    }

    private void Start()
    {
        flammableStackUI.UpdateFlammableIconStacks(flammableCount);
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startSizeMultiplier = 0;
        emissionModule.rate = 0;
        ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
        shapeModule.radius = 0.05f;
    }

    public void UpdateFireyBar()
    {
        flammableStackUI.UpdateFireyBar(Mathf.RoundToInt(flammableCount * damageMultiplier * PlayerProperties.playerScript.shipHealthMAX));
    }

    void PickRadiusFromAngleTraveled()
    {
        if(PlayerProperties.playerScript.whatAngleTraveled % 90 < 10)
        {
            ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
            shapeModule.radius = 0.025f;
        }
        else
        {
            ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
            shapeModule.radius = 0.05f;
        }
    }

    private void Update()
    {
        PickRadiusFromAngleTraveled();
        shipSpriteMask.sprite = PlayerProperties.spriteRenderer.sprite;
    }

    public void IgniteFlammableStacks(GameObject damagingObject)
    {
        if (flammableCount > 0)
        {
            PlayerProperties.playerScript.dealTrueDamageToShip(Mathf.RoundToInt(flammableCount * 0.1f * PlayerProperties.playerScript.shipHealthMAX));
            GameObject explosionInstant = Instantiate(explosion, PlayerProperties.playerShipPosition, Quaternion.identity);
            explosionInstant.transform.localScale = Vector3.one * flammableCount * 4f / 5f;
            // need visuals here
            flammableCount = 0;

            foreach (GameObject artifact in PlayerProperties.playerArtifacts.activeArtifacts)
            {
                ArtifactEffect artifactEffect = artifact.GetComponent<ArtifactEffect>();
                if (artifactEffect != null)
                {
                    artifactEffect.ignitedPlayer();
                }
            }

            UpdateFireyBar();
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            emissionModule.rate = 0;
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startSizeMultiplier = 0;
            flammableStackUI.IgniteStacksIconsAnimation();
        }
    }

    public void AddFlammableStack(GameObject damagingObject)
    {
        flammableCount++;
        flammableStackUI.UpdateFlammableIconStacks(flammableCount);
        UpdateFireyBar();
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.rate = 10 * flammableCount;
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startSizeMultiplier = flammableCount * 0.05f;

        foreach(GameObject artifact in PlayerProperties.playerArtifacts.activeArtifacts)
        {
            ArtifactEffect artifactEffect = artifact.GetComponent<ArtifactEffect>();
            if(artifactEffect != null)
            {
                artifactEffect.addedFlammableStack(flammableCount);
            }
        }

        if (flammableCount >= 5)
        {
            IgniteFlammableStacks(damagingObject);
        }
    }

    public void RemoveAllFlammableStacks()
    {
        if (flammableCount > 0)
        {
            flammableCount = 0;
            flammableStackUI.UpdateFlammableIconStacks(flammableCount);
            UpdateFireyBar();
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            emissionModule.rate = 10 * flammableCount;
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startSizeMultiplier = flammableCount * 0.05f;
        }
    }

    public void RemoveFlammableStack()
    {
        if(flammableCount > 0) {
            flammableCount--;
            flammableStackUI.UpdateFlammableIconStacks(flammableCount);
            UpdateFireyBar();
            ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
            emissionModule.rate = 10 * flammableCount;
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startSizeMultiplier = flammableCount * 0.05f;
        }
    }
}
