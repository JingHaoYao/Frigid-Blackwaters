using UnityEngine;

public class FiendFlowerBud : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    [SerializeField] ArtifactBonus artifactBonus;
    Camera mainCamera;
    [SerializeField] LayerMask layerMask;
    public GameObject turret;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void spawnTurret()
    {
        if (PlayerProperties.playerScript.enemiesDefeated == false)
        {
            PlayerProperties.playerArtifacts.numKills -= artifactBonus.killRequirement;
            float randAngle = Random.Range(0, Mathf.PI * 2);
            Vector3 proposedPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * 1.5f;

            for (int i = 0; i < 100; i++)
            {
                if (!checkIfPositionIsValid(proposedPosition))
                {
                    randAngle = Random.Range(0, Mathf.PI * 2);
                    proposedPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * 1.5f;
                }
                else
                {
                    break;
                }
            }

            Instantiate(turret, proposedPosition, Quaternion.identity);
        }
    }

    bool checkIfPositionIsValid(Vector3 pos)
    {
        return Mathf.Abs(pos.x - mainCamera.transform.position.x) < 8.5f && Mathf.Abs(pos.y - mainCamera.transform.position.y) < 8.5f && !Physics2D.OverlapCircle(pos, 0.4f, layerMask);
    }

    void Update()
    {
        if (displayItem.isEquipped == true && PlayerProperties.playerArtifacts.numKills >= artifactBonus.killRequirement)
        {
            if (displayItem.whichSlot == 0)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact)))
                {
                    spawnTurret();
                }
            }
            else if (displayItem.whichSlot == 1)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact)))
                {
                    spawnTurret();
                }
            }
            else
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact)))
                {
                    spawnTurret();
                }
            }
        }
    }
}
