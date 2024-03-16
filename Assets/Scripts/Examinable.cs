using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

[RequireComponent(typeof(ARSelectionInteractable))]
public class Examinable : MonoBehaviour
{
    [SerializeField] ExaminableManager _manager;
    [SerializeField] float examineScaleOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<ExaminableManager>();

        if (_manager == null)
        {
            Debug.LogError($"Could not find Examinable Manager for {gameObject.name}");
        }
    }

    public float ExamineScaleOffset() { return examineScaleOffset; }
}
