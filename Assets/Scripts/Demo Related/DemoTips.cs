using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoTips : MonoBehaviour
{
    public int whatStage = 0;
    float timer = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if (whatStage == 0)
        {
            GetComponentInChildren<Text>().text = "You can fire your weapons with your mouse button.\nWhich weapon you fire is dependent on where your mouse is relative to your ship.";
            if (FindObjectOfType<FirstRoomDoorBlock>().numberSwitchesActivated > 1)
            {
                whatStage = 1;
            }
        }
        else if (whatStage == 1)
        {
            GetComponentInChildren<Text>().text = "Open your inventory with 'I'.\nEquip artifacts by clicking on them.\nUse your artifact by pressing 1, 2, 3 based on which slot they're equipped in.";
            if (FindObjectOfType<FirstRoomDoorBlock>() == null)
            {
                whatStage = 2;
            }
        }
        else if (whatStage == 2)
        {
            GetComponentInChildren<Text>().text = "Now we'll send some skeletons after you in the next room.";
            if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 20, 0)) < 0.5f)
            {
                whatStage = 3;
            }
        }
        else if(whatStage == 3)
        {
            GetComponentInChildren<Text>().enabled = false;
            GetComponent<Image>().enabled = false;
            if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 40, 0)) < 0.5f)
            {
                whatStage = 4;
            }
        }
        else if(whatStage == 4)
        {
            GetComponentInChildren<Text>().enabled = true;
            GetComponent<Image>().enabled = true;
            GetComponentInChildren<Text>().text = "In the middle of the room, there is a weapon stash.\nApproach it and press 'E' to change the weapons on either of the 3 sides of your ship.";
            if (FindObjectOfType<TrainingHouseWeaponIcon>() || (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 40, 0)) > 12f && Camera.main.transform.position.y >= 40))
            {
                whatStage = 5;
            }
        }
        else if(whatStage == 5)
        {
            GetComponentInChildren<Text>().text = "The rest of the dungeon is yours to explore now.";
            timer += Time.deltaTime;
            if(timer > 4)
            {
                timer = 0;
                whatStage = 6;
            }
        }
        else if(whatStage == 6)
        {
            GetComponentInChildren<Text>().enabled = false;
            GetComponent<Image>().enabled = false;

            if (FindObjectOfType<PlayerScript>().enemiesDefeated == true) {
                if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 80, 0)) < 0.5f)
                {
                    whatStage = 7;
                }
                else if(Vector2.Distance(Camera.main.transform.position, new Vector3(40, 80, 0)) < 0.5f)
                {
                    whatStage = 8;
                }
            }
        }
        else if(whatStage == 7)
        {
            GetComponentInChildren<Text>().enabled = true;
            GetComponent<Image>().enabled = true;
            GetComponentInChildren<Text>().text = "The room to your right is the boss room.\nWe created a special boss for this expo demo, we hope you like it!";
            if (Vector2.Distance(Camera.main.transform.position, new Vector3(0, 80, 0)) > 0.5f)
            {
                whatStage = 9;
            }
        }
        else if(whatStage == 8)
        {
            GetComponentInChildren<Text>().enabled = true;
            GetComponent<Image>().enabled = true;
            GetComponentInChildren<Text>().text = "The room to your left is the boss room.\nWe created a special boss for this expo demo, we hope you like it!";
            if(Vector2.Distance(Camera.main.transform.position, new Vector3(40, 80, 0)) > 0.5f)
            {
                whatStage = 9;
            }
        }
        else
        {
            GetComponentInChildren<Text>().enabled = false;
            GetComponent<Image>().enabled = false;
        }
    }
}
