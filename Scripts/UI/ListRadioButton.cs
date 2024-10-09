using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ListRadioButton : MonoBehaviour
{
    [SerializeField] private Transform _radioButtonParent;
    private List<RadioButtonItem> _radioButtonItems;
    public bool isSelected = false;
    private RadioButtonItem _activeRadioButton;
    [SerializeField] private PrefabSpawner _prefabSpawner;
    private void Start()
    {
        _radioButtonItems = _radioButtonParent.GetComponentsInChildren<RadioButtonItem>().ToList();
        DisableAllRadioButtons();
    }
    public void Activate(RadioButtonItem radioButton)
    {
        DisableAllRadioButtons();

        if ((_activeRadioButton == radioButton) && (radioButton.isActive))
        {
            _prefabSpawner.currentPrefab = null;
            radioButton.isActive = false;
        }
        else
        {
            radioButton.isActive = true ;
            radioButton.Image.SetToActiveColor();
            isSelected = true;
            _activeRadioButton = radioButton;
        }

    } 

    private void DisableAllRadioButtons()
    {
        foreach(RadioButtonItem item in _radioButtonItems)
        {
            item.isActive = false;
            item.Image.SetToDefaultColor();
        }
        isSelected = false;
    }
}
