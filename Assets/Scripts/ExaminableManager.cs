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

    GameObject _selectionObject;

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
            Debug.Log("Handling touch");
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
        if (_examinedObject != null)
        {
            Destroy(_examinedObject.gameObject);
            Debug.Log($"Still there? {_examinedObject}");
        }
    }

    public void SetExaminedObject(Examinable target)
    {
        Examinable[] objects = FindObjectsOfType<Examinable>();
        Debug.Log($"Found {objects.Length} examinable objects");
        foreach (Examinable obj in objects)
        {
            if (obj.TryGetComponent<ARSelectionInteractable>(out ARSelectionInteractable selection))
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

    private void PrintComponents()
    {
        Component[] allComponents;
        allComponents = _examinedObject.GetComponents(typeof(Component));
        foreach (Component component in allComponents)
        {
            MonoBehaviour mb = component as MonoBehaviour;
            if (mb != null)
            {
                Debug.Log($"Component: {component} enabled={mb.enabled}");
            }
        }
    }

    public void ToggleExamination()
    {
        PrintComponents();
        if (!isExamining)
        {
            PerformExamination();
        } else
        {
            Unexamine();
        }
    }

    public void PerformExamination()
    {
        if (_examinedObject == null || _examineTarget == null)
        {
            Debug.Log($"Examine failed: Target: {_examinedObject} examineTarget: {_examineTarget}");
            return; 
        }

        Debug.Log($"Examining {_examinedObject.gameObject.name}");

        // Try to get rid of the selection object
        if (_examinedObject.TryGetComponent<ARSelectionInteractable>(out ARSelectionInteractable selection))
        {
            Debug.Log("Found a selection interactable");
            _selectionObject = selection.selectionVisualization;
            _selectionObject.SetActive(false);
            // selection.enabled = false;
        }

        //if (_examinedObject.TryGetComponent<ARScaleInteractable>(out ARScaleInteractable scale))
        //{
        //    Debug.Log("Found a scale interactable");
        //    scale.enabled = false;
        //}

        //if (_examinedObject.TryGetComponent<ARTranslationInteractable>(out ARTranslationInteractable translate))
        //{
        //    Debug.Log("Found a translate interactable");
        //    translate.enabled = false;
        //}

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
        if (isExamining)
        {
            _examinedObject.transform.position = _priorPos;
            _examinedObject.transform.rotation = _priorRot;
            _examinedObject.transform.localScale = _priorScale;

            if (_examinedObject.TryGetComponent<ARSelectionInteractable>(out ARSelectionInteractable selection))
            {
                selection.selectionVisualization.SetActive(true);
                //selection.enabled = true;
            }

            _examinedObject.transform.parent = null;

            //if (_examinedObject.TryGetComponent<ARScaleInteractable>(out ARScaleInteractable scale))
            //{
            //    Debug.Log("Found a scale interactable");
            //    scale.enabled = true;
            //}

            //if (_examinedObject.TryGetComponent<ARTranslationInteractable>(out ARTranslationInteractable translate))
            //{
            //    Debug.Log("Found a translate interactable");
            //    translate.enabled = true;
            //}

            isExamining = false;
        }
    }
}
