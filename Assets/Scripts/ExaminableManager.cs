using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ExaminableManager : MonoBehaviour
{
    [SerializeField]
    Transform _examineTarget;

    Vector3 _priorPos;
    Quaternion _priorRot;
    Vector3 _priorScale;
    Transform _priorParent;
    
    [SerializeField]
    PlacementManager _placementManager;

    void Start()
    {
        if (_placementManager == null) { 
            _placementManager = GameObject.FindFirstObjectByType<PlacementManager>();

            if (_placementManager == null ) {
                Debug.LogError("#### Could not locate Placement Manager.");
            }
        }
    }

    GameObject _examinedObject;
    private bool isExamining = false;
    private float _rotSpeed = 25f;

    private void Update()
    {
        if (isExamining && Input.touchCount > 0)
        {
            // Get first touch
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 rotation = new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0) * _rotSpeed * Time.deltaTime;
                _examinedObject.transform.Rotate(rotation);
            }
        }
    }

    public void ToggleExamination()
    {
        if (isExamining)
        {
            Unexamine();
        } else
        {
            PerformExamination();
        }
    }

    public void PerformExamination()
    {
        GameObject selectedObject = _placementManager.GetSelectedObject();
        if (selectedObject == null || _examineTarget == null)
        {
            Debug.Log($"Examine failed: Target: {selectedObject} examineTarget: {_examineTarget}");
            return; 
        }

        Debug.Log($"Examining {selectedObject.gameObject.name}");

        if (selectedObject.TryGetComponent<Examinable>(out Examinable examinable))
        {
            _examinedObject = examinable.ExaminedObject;
            _priorPos = _examinedObject.transform.localPosition;
            _priorRot = _examinedObject.transform.localRotation;
            _priorScale = _examinedObject.transform.localScale;
            _priorParent = _examinedObject.transform.parent;

            _examinedObject.transform.position = _examineTarget.position;
            _examinedObject.transform.parent = _examineTarget;
            _examinedObject.transform.localScale = _priorScale * examinable.ExamineScaleOffset();
        }

        isExamining = true;
    }

    public void Unexamine()
    {
        if (_examinedObject != null)
        {
            _examinedObject.transform.parent = _priorParent;

            _examinedObject.transform.localPosition = _priorPos;
            _examinedObject.transform.localRotation = _priorRot;
            _examinedObject.transform.localScale = _priorScale;
        }

        isExamining = false;
    }
}
