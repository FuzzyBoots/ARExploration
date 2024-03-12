using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] GameObject[] _prefabs;
    [SerializeField] private GameObject _selectionVisualization;
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _maxScale = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // This is where we could fill the prefabs list automatically
    }

    public bool isPrefabPresent(string name)
    {
        for (int i=0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child?.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public GameObject GetExtantPrefab(string name)
    {
        Transform child = transform.Find(name);
        return child?.gameObject;
    }

    public GameObject GetExaminableObject(string name)
    {
        if (_prefabs.Any(x => x.name == name))
        {
            GameObject examinable = Instantiate(_prefabs.First(x => x.name == name));
            Examinable instance = examinable.AddComponent<Examinable>();

            return examinable;
        }
        return null;
    }

    public GameObject GetPlaceableObject(string name)
    {
        if (_prefabs.Any(x => x.name == name))
        {
            GameObject placeable = Instantiate(_prefabs.First(x => x.name == name));
            ARSelectionInteractable selectionInteractable = placeable.AddComponent<ARSelectionInteractable>();
            ARTranslationInteractable translationInteractable = placeable.AddComponent<ARTranslationInteractable>();
            ARScaleInteractable scaleInteractable = placeable.AddComponent<ARScaleInteractable>();

            selectionInteractable.selectionVisualization = _selectionVisualization;

            scaleInteractable.minScale = _minScale;
            scaleInteractable.maxScale = _maxScale;
            scaleInteractable.elasticity = 0.15f;
            scaleInteractable.elasticRatioLimit = 0.5f;
            scaleInteractable.sensitivity = 0.75f;

            return placeable;
        }
        return null;
    }

    internal void HouseObject(GameObject placed)
    {
        // Put it in the hierarchy
        placed.transform.parent = transform;
    }
}
