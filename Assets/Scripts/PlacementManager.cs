using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    private ARGestureInteractor _gestureInteractor;

    [SerializeField]
    private ExaminableManager _examinableManager;

    [SerializeField] private ARPlacementInteractable _placementInteractable;

    [SerializeField] UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        ARObjectPlacementEvent aRObjectPlacedEvent = _placementInteractable.objectPlaced;
        aRObjectPlacedEvent.AddListener(ObjectPlaced);

        if (_uiManager == null)
        {
            _uiManager = FindAnyObjectByType<UIManager>();
        }
    }

    private void ObjectPlaced(ARObjectPlacementEventArgs eventArgs)
    {
        Debug.Log("##### Placed object?");
        string objectName = eventArgs?.placementObject?.name;
        if (objectName != "")
        {
            objectName = objectName.Replace("(Clone)", "");
            Debug.Log($"##### Placed {objectName}");
            _uiManager.DisablePlacementButton(objectName);
            eventArgs.placementObject.name = objectName;
            eventArgs.placementObject.transform.parent = this.transform;
        }
    }

    public GameObject GetSelectedObject()
    {
        Debug.Log($"#### Found {transform.childCount} objects");
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            Debug.Log($"#### {obj.name} is found");
            if (obj.TryGetComponent<ARSelectionInteractable>(out ARSelectionInteractable selection))
            {
                Debug.Log("#### Found selection interactable");
                if (selection.isSelected)
                {
                    Debug.Log("#### is selected!");
                    return obj;
                }
            }
        }
        return null;
    }

    public void DeleteSelected()
    {
        Debug.Log("#### Entering Delete Selected");
        GameObject _examinedObject = GetSelectedObject();
        if (_examinedObject != null)
        {
            _uiManager.EnablePlacementButton( _examinedObject.name );
            Destroy(_examinedObject);
            _examinableManager.Unexamine();
            Debug.Log($"Still there? {_examinedObject}");
        }
    }

    public void ResetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void HandleObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("#### Attempted to place a null prefab");
            return;
        }

        Debug.Log("##### HandleObject " + prefab.name);

        Transform found = transform.Find(prefab.name);
        for (int i = 0; i < transform.childCount; i++) { Debug.Log($"#### Found {transform.GetChild(i).name}"); }
        if (found != null)
        {
            GameObject placed = found.gameObject;
            Debug.Log($"##### Select {placed.name}");
            if (placed.TryGetComponent<IXRSelectInteractable>(out IXRSelectInteractable select))
            {
                // Select the item!
                _gestureInteractor.StartManualInteraction(select);
            }
        }
        else
        {
            Debug.Log($"#### Placing {prefab.name}");
            _placementInteractable.placementPrefab = prefab;
        }
    }
}
