using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalParticles : MonoBehaviour {
    public Vector3 target;
    public float speed = 2.5f;
    public GameObject particles;
    float period = 0;
    public GameObject summoningEffect;
    public bool isChest = false;
    ItemTemplates itemTemplates;

    void Start()
    {
        itemTemplates = FindObjectOfType<ItemTemplates>();
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            float angleToTarget = (360 + (Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg)) % 360;
            transform.position += new Vector3(Mathf.Cos(angleToTarget * Mathf.Deg2Rad), Mathf.Sin(angleToTarget * Mathf.Deg2Rad), 0) * speed * Time.deltaTime;
            period += Time.deltaTime;
            if (period > 0.05f)
            {
                period = 0;
                Instantiate(particles, transform.position, Quaternion.Euler(0, 0, angleToTarget + 90));
            }

            if(Vector2.Distance(target, transform.position) < 0.2f)
            {
                if (isChest)
                {
                    GameObject chest = Instantiate(summoningEffect, target, Quaternion.identity);

                    GameObject pickedArtifact = FindObjectOfType<ItemTemplates>().loadItem(this.GetComponent<ChallengeRoomItemPicker>().pickedItems[Random.Range(0, this.GetComponent<ChallengeRoomItemPicker>().pickedItems.Length)]);

                    chest.GetComponent<Chest>().uniqueItems = new GameObject[1] { pickedArtifact };
                    GameObject newItem = Instantiate(itemTemplates.gold);
                    newItem.transform.SetParent(GameObject.Find("PresentItems").transform);
                    newItem.GetComponent<DisplayItem>().goldValue = this.GetComponent<ChallengeRoomItemPicker>().goldAmount;
                    chest.GetComponent<Chest>().chestItems[1] = newItem;
                    Destroy(this.gameObject);
                }
                else
                {
                    Instantiate(summoningEffect, target, Quaternion.identity);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
