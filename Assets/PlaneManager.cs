using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    ARPlaneManager _planeManager;

    private void Start()
    {
        if (!_planeManager)
        {
            _planeManager = GetComponent<ARPlaneManager>();
            if (!_planeManager )
            {
                Debug.LogError("Could not find AR Place Manager");
            }
        }
    }

    public void TogglePlaneManager()
    {
        _planeManager.enabled = !_planeManager.enabled;
    }
}
