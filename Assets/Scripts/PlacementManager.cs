using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    private ARGestureInteractor _gestureInteractor;

    [SerializeField] private ARPlacementInteractable _placementInteractable;

    [SerializeField] private PrefabManager _prefabManager;
    
    // Start is called before the first frame update
    void Start()
    {
        ARObjectPlacementEvent aRObjectPlacedEvent = _placementInteractable.objectPlaced;
        aRObjectPlacedEvent.AddListener(ObjectPlaced);
    }

    private void ObjectPlaced(ARObjectPlacementEventArgs eventArgs)
    {
        Debug.Log("Placed object?");
        GameObject placed = eventArgs.placementObject;

        _prefabManager.HouseObject(placed);
    }

    public void ResetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void HandleObject(string name)
    {
        GameObject prefab = _prefabManager.GetExtantPrefab(name);
        if (prefab != null)
        {
            Debug.Log($"Select {prefab}");
            if (prefab.TryGetComponent<IXRSelectInteractable>(out IXRSelectInteractable select))
            {
                // Select the item!
                _gestureInteractor.StartManualInteraction(select);
                _gestureInteractor.EndManualInteraction();
            }
        } else
        {
            prefab = _prefabManager.GetPlaceableObject(name);
            if (prefab != null)
            {
                _placementInteractable.placementPrefab = prefab;
            }
        }

        //if (validElements.Any(x => x.name == name))
        //{
        //    Debug.Log("Keys: " + _placedObjects.Keys.ToCommaSeparatedString());
        //    if (_placedObjects.Contains(name))
        //    {
        //        GameObject placed = (GameObject)_placedObjects[name];
        //        Debug.Log($"Select {placed}");
        //        if (placed.TryGetComponent<IXRSelectInteractable>(out IXRSelectInteractable select))
        //        {
        //            // Select the item!
        //            _gestureInteractor.StartManualInteraction(select);
        //            _gestureInteractor.EndManualInteraction();
        //        }
        //    } else
        //    {
        //        GameObject toBePlaced = validElements.First(x => x.name == name).prefab;
        //        _placementInteractable.placementPrefab = toBePlaced;
        //    }
        //}
        //else
        //{
        //    Debug.LogError($"Invalid object name {name} provided to PlacementManager");
        //}
    }
}
