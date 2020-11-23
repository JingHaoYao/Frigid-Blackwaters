using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityAccelerator : ArtifactEffect
{
    float acceleratorSpeed = 0;
    Vector3 directionVector;
    RealityAcceleratorEngines engines;
    [SerializeField] GameObject realityAcceleratorEngines;
    [SerializeField] DisplayItem displayItem; 

    public override void artifactEquipped()
    {
        engines = Instantiate(realityAcceleratorEngines, PlayerProperties.playerShipPosition, Quaternion.identity).GetComponent<RealityAcceleratorEngines>();
        engines.Initialize();
    }

    public override void artifactUnequipped()
    {
        Destroy(engines.gameObject);
    }

    private void Update()
    {
        if (displayItem.isEquipped)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.dash)))
            {
                directionVector = new Vector3(Mathf.Cos(PlayerProperties.playerScript.whatAngleTraveled * Mathf.Deg2Rad), Mathf.Sin(PlayerProperties.playerScript.whatAngleTraveled * Mathf.Deg2Rad));
                engines.StartBurst(PlayerProperties.playerScript.whatAngleTraveled);
            }
            else if (Input.GetKey((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.dash)))
            {
                acceleratorSpeed = Mathf.Clamp(acceleratorSpeed + Time.deltaTime * 16, 0, 16);
                PlayerProperties.playerScript.setPlayerMomentum(directionVector * acceleratorSpeed, 1f);
                engines.UpdateDamageOnColliders(Mathf.FloorToInt(PlayerProperties.playerScript.boatSpeed * 2));
            }

            if (Input.GetKeyUp((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.dash)))
            {
                engines.UpdateDamageOnColliders(0);
                engines.StopBurst();
                acceleratorSpeed = 0;
            }
        }
    }
}
