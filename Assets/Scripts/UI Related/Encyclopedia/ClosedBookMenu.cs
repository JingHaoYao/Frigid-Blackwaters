using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosedBookMenu : MonoBehaviour
{
    [SerializeField]
    List<Button> buttons;

    public void Initialize(EnemyEncyclopedia encyclopedia)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int dungeonLevel = i + 1;
            buttons[i].onClick.AddListener(()=> { encyclopedia.OpenDungeonPage(dungeonLevel); });
        }
    }
}