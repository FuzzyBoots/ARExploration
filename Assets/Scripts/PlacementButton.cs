using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class PlacementButton : MonoBehaviour
{
    [SerializeField] GameObject _prefab;

    [SerializeField] PlacementManager _placementManager;
    // Start is called before the first frame update
    void Start()
    {
        if (_placementManager == null)
        {
            _placementManager = FindFirstObjectByType<PlacementManager>();

            if (_placementManager == null)
            {
                Debug.LogError("#### No Placement Manager found");
            }
        }
    } 

    public void SetPlacementInteractorPrefab()
    {
        _placementManager.HandleObject(_prefab);
    }
}
