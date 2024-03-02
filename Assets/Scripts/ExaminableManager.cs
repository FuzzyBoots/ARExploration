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

    void Start()
    {
        
    }

    Examinable _examinedObject;
    private bool isExamining;
    private float _rotSpeed = 25f;

    private void Update()
    {
        if (isExamining && Input.touchCount > 0)
        {
            // Get first touch
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved) {
                Vector3 rotation = new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0) * _rotSpeed * Time.deltaTime;
                _examinedObject.transform.Rotate(rotation);
            }
        }
    }

    public void DeleteSelected()
    {
        if (_examinedObject != null)
        {
            Destroy(_examinedObject.gameObject);
            Debug.Log($"Still there? {_examinedObject}");
        }
    }

    public void SetExaminedObject(Examinable target)
    {
        Examinable[] objects = FindObjectsOfType<Examinable>();
        foreach (Examinable obj in objects)
        {
            if (TryGetComponent<ARSelectionInteractable>(out ARSelectionInteractable selection))
            {
                Debug.Log($"{obj.gameObject.name} Selected: {selection.isSelected}");
            }
        }
        _examinedObject = target;
    }

    public void ClearExaminedObject()
    {
        _examinedObject = null;
    }

    public void PerformExamination()
    {
        if (_examinedObject == null || _examineTarget == null)
        {
            Debug.Log($"Examine failed: Target: {_examinedObject} examineTarget: {_examineTarget}");
            return; 
        }

        Debug.Log($"Examining {_examinedObject.gameObject.name}");

        _priorPos = _examinedObject.transform.position;
        _priorRot = _examinedObject.transform.rotation;
        _priorScale = _examinedObject.transform.localScale;
        
        _examinedObject.transform.position = _examineTarget.position;
        _examinedObject.transform.parent = _examineTarget;
        _examinedObject.transform.localScale = _priorScale * _examinedObject.ExamineScaleOffset();

        isExamining = true;
    }

    public void Unexamine()
    {
        _examinedObject.transform.position = _priorPos;
        _examinedObject.transform.rotation = _priorRot;
        _examinedObject.transform.localScale = _priorScale;

        _examinedObject.transform.parent = null;

        isExamining = false;
    }
}
