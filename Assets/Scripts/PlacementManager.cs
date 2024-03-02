using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class PlacementManager : MonoBehaviour
{
    [System.Serializable]
    public struct placementEntry
    {
        public string name;
        public GameObject prefab;
    }

    [SerializeField]
    private placementEntry[] validElements;

    [SerializeField]
    private ARGestureInteractor _gestureInteractor;

    Hashtable _placedObjects;

    [SerializeField] private ARPlacementInteractable _placementInteractable;
    
    // Start is called before the first frame update
    void Start()
    {
        _placedObjects = new Hashtable();
        ARObjectPlacementEvent aRObjectPlacedEvent = _placementInteractable.objectPlaced;
        aRObjectPlacedEvent.AddListener(ObjectPlaced);
    }

    private void ObjectPlaced(ARObjectPlacementEventArgs eventArgs)
    {
        Debug.Log("Placed object?");
        string objectName = eventArgs?.placementObject?.name;
        if (objectName != "") {
            Debug.Log($"Name: {objectName}");
            _placedObjects[objectName] = eventArgs.placementObject;
        }
    }

    public void ResetScene(string sceneName)
    {
        _placedObjects.Clear();
        SceneManager.LoadScene(sceneName);
    }

    public void RegisterObject()
    {

    }

    public void HandleObject(string name)
    {
        if (validElements.Any(x => x.name == name))
        {
            if (_placedObjects.Contains(name))
            {
                GameObject placed = (GameObject)_placedObjects[name];
                Debug.Log($"Select {placed}");
                if (placed.TryGetComponent<IXRSelectInteractable>(out IXRSelectInteractable select))
                {
                    // Select the item!
                    _gestureInteractor.StartManualInteraction(select);
                }
            } else
            {
                GameObject toBePlaced = validElements.First(x => x.name == name).prefab;
                _placementInteractable.placementPrefab = toBePlaced;
            }
        }
        else
        {
            Debug.LogError($"Invalid object name {name} provided to PlacementManager");
        }
    }
}
