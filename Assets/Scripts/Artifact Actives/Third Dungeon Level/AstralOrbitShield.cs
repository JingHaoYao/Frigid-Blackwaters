using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralOrbitShield : MonoBehaviour
{
    [SerializeField] DisplayItem displayItem;
    private List<GameObject> blackHoleShields = new List<GameObject>();
    public GameObject blackHoleShield;
    [SerializeField] float loopSpeed;
    private float period = 0;

    void updateBlackHoleShield()
    {
        period += Time.deltaTime * loopSpeed;

        if(period >= Mathf.PI * 2)
        {
            period = 0;
        }

        for(int i = 0; i < 3; i++)
        {
            float angle = period + (Mathf.PI * 2 / 3f) * i;
            blackHoleShields[i].transform.position = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 2.5f;
        }
    }

    void addBlackHoleShields()
    {
        period = 0;
        for(int i = 0; i < 3; i++)
        {
            GameObject shieldInstant = Instantiate(blackHoleShield, PlayerProperties.playerShipPosition, Quaternion.identity);
            blackHoleShields.Add(shieldInstant);
        }
        updateBlackHoleShield();
    }

    void Update()
    {
        if(displayItem.isEquipped == true)
        {
            if(blackHoleShields.Count <= 0)
            {
                addBlackHoleShields();
            }
            updateBlackHoleShield();
        }
        else
        {
            if(blackHoleShields.Count > 0)
            {
                foreach(GameObject blackHoleShield in blackHoleShields)
                {
                    Destroy(blackHoleShield);
                }
                blackHoleShields.Clear();
            }
        }
    }
}
