using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Hashtable _placementButtons = new Hashtable();
    // Start is called before the first frame update

    GameObject _placementButtonsGroup;
    void Start()
    {
        PlacementButton[] buttons = GetComponentsInChildren<PlacementButton>();
        foreach (PlacementButton button in buttons)
        {
            TMP_Text buttonText = button.gameObject.GetComponentInChildren<TMP_Text>();
            _placementButtons[buttonText.text] = button.gameObject;
        }
        Debug.Log("#### Placement Buttons: " + _placementButtons.Keys.ToCommaSeparatedString());

        _placementButtonsGroup = transform.Find("Placement Buttons").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisablePlacementButtons()
    {
        _placementButtonsGroup.SetActive(false);
    }

    public void EnablePlacementButtons()
    {
        _placementButtonsGroup.SetActive(true);
    }

    public void DisablePlacementButton(string name)
    {
        GameObject button = (GameObject)_placementButtons[name];
        if (button != null)
        {
            button.SetActive(false);
        }
    }

    public void EnablePlacementButton(string name)
    {
        GameObject button = (GameObject)_placementButtons[name];
        if (button != null)
        {
            button.SetActive(true);
        }
    }
}
