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
        Debug.Log("##### Placed object?");
        string objectName = eventArgs?.placementObject?.name;
        if (objectName != "")
        {
            objectName = objectName.Replace("(Clone)", "");
            Debug.Log($"##### Placed {objectName}");
            eventArgs.placementObject.name = objectName;
            eventArgs.placementObject.transform.parent = this.transform;
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

    public void HandleObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("#### Attempted to place a null prefab");
            return;
        }

        Debug.Log("##### HandleObject " + name);

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
