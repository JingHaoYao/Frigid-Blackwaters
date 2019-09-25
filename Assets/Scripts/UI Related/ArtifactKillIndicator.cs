using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactKillIndicator : MonoBehaviour
{
    Artifacts artifacts;
    Text text;

    void Start()
    {
        artifacts = FindObjectOfType<Artifacts>();
        text = GetComponentInChildren<Text>();
    }

    void Update()
    {
        text.text = artifacts.numKills.ToString();
    }
}
