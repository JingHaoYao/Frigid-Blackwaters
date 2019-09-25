using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLayoutTemplates : MonoBehaviour {
    public List<RoomLayout> roomLayouts = new List<RoomLayout>();

    private void Start()
    {
        //
        // Base layouts for all dungeon stages
        //
        roomLayouts.Add(new RoomLayout(new Vector3[4] { new Vector3(-4, 4, 0), new Vector3(-4, -4, 0), new Vector3(4, -4, 0), new Vector3(4, 4, 0) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[4] { new Vector3(-3, 0, 0), new Vector3(3, 0, 0), new Vector3(0, -3, 0), new Vector3(0, 3, 0) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[2] { new Vector3(5, 3, 0), new Vector3(5, -3, 0) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[2] { new Vector3(-5, 3, 0), new Vector3(-5, -3, 0) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[2] { new Vector3(3, -5, 0), new Vector3(-3, -5, 0) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[2] { new Vector3(3, 5, 0), new Vector3(-3, 5, 0) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[2] { new Vector3(4, 4), new Vector3(-4, -4) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[2] { new Vector3(-4, 4), new Vector3(4, -4) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[4] { new Vector3(-4, 4, 0), new Vector3(-4, -4, 0), new Vector3(4, -4, 0), new Vector3(4, 4, 0) }, new Vector3[12] { new Vector3(0.5f, -0.5f), new Vector3(0.5f, 0.5f), new Vector3(-0.5f, 0.5f), new Vector3(-0.5f, -0.5f), new Vector3(0.5f, 1.5f), new Vector3(-0.5f, 1.5f), new Vector3(0.5f, -1.5f), new Vector3(-0.5f, -1.5f), new Vector3(1.5f, 0.5f), new Vector3(1.5f, -0.5f), new Vector3(-1.5f, 0.5f), new Vector3(-1.5f, -0.5f)}));
        roomLayouts.Add(new RoomLayout(new Vector3[5] { new Vector3(-4, 4, 0), new Vector3(-4, -4, 0), new Vector3(4, -4, 0), new Vector3(4, 4, 0), new Vector3(0, 0) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[4] { new Vector3(6, 6, 0), new Vector3(-6, 6, 0), new Vector3(-6, -6, 0), new Vector3(6, -6, 0) }, new Vector3[16] { new Vector3(-3.5f, -0.5f), new Vector3(-3.5f, 0.5f), new Vector3(-3.5f, 1.5f), new Vector3(-3.5f, -1.5f), new Vector3(3.5f, -0.5f), new Vector3(3.5f, 0.5f), new Vector3(3.5f, 1.5f), new Vector3(3.5f, -1.5f), new Vector3(-0.5f, 3.5f), new Vector3(0.5f, 3.5f), new Vector3(1.5f, 3.5f), new Vector3(-1.5f, 3.5f), new Vector3(-0.5f, -3.5f), new Vector3(0.5f, -3.5f), new Vector3(1.5f, -3.5f), new Vector3(-1.5f, -3.5f) }));
        roomLayouts.Add(new RoomLayout(new Vector3[0], new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[1] { new Vector3(0, 0) }, new Vector3[12] { new Vector3(-1.5f, -0.5f), new Vector3(-1.5f, 0.5f), new Vector3(1.5f, -0.5f), new Vector3(1.5f, 0.5f), new Vector3(0.5f, 1.5f), new Vector3(-0.5f, 1.5f), new Vector3(-0.5f, -1.5f), new Vector3(0.5f, -1.5f), new Vector3(1.5f, 1.5f), new Vector3(-1.5f, 1.5f), new Vector3(1.5f, -1.5f), new Vector3(-1.5f, -1.5f) }));
        roomLayouts.Add(new RoomLayout(new Vector3[6] { new Vector3(0, 4), new Vector3(2, 4), new Vector3(-2, 4), new Vector3(0, -4), new Vector3(2, -4), new Vector3(-2, -4)}, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[6] { new Vector3(4, 0), new Vector3(4, 2), new Vector3(4, -2), new Vector3(-4, 0), new Vector3(-4, 2), new Vector3(-4, -2) }, new Vector3[0]));
        roomLayouts.Add(new RoomLayout(new Vector3[0], new Vector3[12] { new Vector3(-4.5f, -4.5f), new Vector3(-4.5f, 4.5f), new Vector3(4.5f, -4.5f), new Vector3(4.5f, 4.5f), new Vector3(-3.5f, -4.5f), new Vector3(3.5f, -4.5f), new Vector3(-3.5f, 4.5f), new Vector3(3.5f, 4.5f), new Vector3(-4.5f, 3.5f), new Vector3(4.5f, 3.5f), new Vector3(-4.5f, -3.5f), new Vector3(4.5f, -3.5f) }));
    }
}
