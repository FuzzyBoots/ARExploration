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

    public void DeleteSelected()
    {
        _examinedObject = GetSelectedObject();
        if (_examinedObject != null)
        {
            Destroy(_examinedObject.gameObject);
            Debug.Log($"Still there? {_examinedObject}");
        }
    }

    private GameObject GetSelectedObject()
    {
        Debug.Log($"#### Found {_placementManager.gameObject.transform.childCount} objects");
        for (int i=0; i < _placementManager.gameObject.transform.childCount; ++i)
        {
            GameObject obj = _placementManager.gameObject.transform.GetChild(i).gameObject;
            Debug.Log($"#### {obj.name} is found");
            if (TryGetComponent<ARSelectionInteractable>(out ARSelectionInteractable selection))
            {
                if (selection.isSelected)
                {
                    Debug.Log("#### is selected!");
                    return obj;
                }
            }
        }
        return null;
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
        _examinedObject = GetSelectedObject();
        if (_examinedObject == null || _examineTarget == null)
        {
            Debug.Log($"Examine failed: Target: {_examinedObject} examineTarget: {_examineTarget}");
            return; 
        }

        Debug.Log($"Examining {_examinedObject.gameObject.name}");

        // We need to make a copy of the object, make it Examinable,
        // and make sure all of the Scale/Translate/etc is off

        GameObject _toBeExamined = Instantiate(_examinedObject, _examineTarget.position, Quaternion.identity);
        _examinedObject.SetActive(false);
        _toBeExamined.transform.parent = _examineTarget;
        _toBeExamined.AddComponent<Examinable>();
        // _examinedObject.transform.localScale = _toBeExamined.transform.localScale * _examinedObject.ExamineScaleOffset();

        // Do we need to disable other things?

        isExamining = true;
    }

    public void Unexamine()
    {
        _examinedObject.SetActive(true);
        _examinedObject.transform.position = _priorPos;
        _examinedObject.transform.rotation = _priorRot;
        _examinedObject.transform.localScale = _priorScale;

        _examinedObject.transform.parent = null;

        isExamining = false;
    }
}
