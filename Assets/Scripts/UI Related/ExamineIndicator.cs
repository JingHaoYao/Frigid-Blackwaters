using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineIndicator : MonoBehaviour
{
    public GameObject parentObject;
    Vector3 startPos, thisStartPos;

    void Start()
    {
        startPos = parentObject.transform.position;
        thisStartPos = transform.position;
    }

    void Update()
    {
        transform.position = thisStartPos + (parentObject.transform.position - startPos);
    }
}
