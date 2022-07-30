using System;
using UnityEngine;

public class Wings : MonoBehaviour
{
    [Header("General")]
    [Range(0.0f, 1.0f)]
    public float WeightOffset = 0.0f;
    public Wing[] AllWings { get; private set; }

    public void Awake()
    {
        if (Instance == null) Instance = this;

        AllWings = GetComponentsInChildren<Wing>();
    }

    public void UpdateWeight(float weight) => Array.ForEach(AllWings, wing => wing.UpdateWeight(weight));

    public static Wings Instance { get; private set; }
}