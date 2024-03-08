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
    private ARSelectionInteractable _selectionInteractable;
    [SerializeField] float examineScaleOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<ExaminableManager>();

        if (_manager == null)
        {
            Debug.LogError($"Could not find Examinable Manager for {gameObject.name}");
        }

        _selectionInteractable = GetComponent<ARSelectionInteractable>();
        SelectEnterEvent selectEvent = _selectionInteractable.selectEntered;
        selectEvent.AddListener(OnSelect);

        SelectExitEvent unselectEvent = _selectionInteractable.selectExited;
        unselectEvent.AddListener(OnUnselect);
    }

    private void OnUnselect(SelectExitEventArgs arg0)
    {
        Debug.Log($"Deselected {this.gameObject.name}");
        _manager.ClearExaminedObject();
    }

    private void OnSelect(SelectEnterEventArgs eventArgs)
    {
        Debug.Log($"Selected {this.gameObject.name}");
        _manager.SetExaminedObject(this);
    }

    public void ClearSelection()
    {
        _manager.ClearExaminedObject();
    }

    public void RequestUnexamine()
    {
        _manager.Unexamine();
    }

    public float ExamineScaleOffset() { return examineScaleOffset; }
}
