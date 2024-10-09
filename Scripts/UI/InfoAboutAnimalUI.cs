using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct AnimalInfo
{
    public Sprite imageButton;
    public AnimalScriptableObject animalScriptable;
    public Sprite imageFullSize;
}
public class InfoAboutAnimalUI : MonoBehaviour
{
    [SerializeField] private FavouriteFoodUI _favouriteFoodUI;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _image;
    [SerializeField] private List<AnimalInfo> _animalsInfo;
    [SerializeField] private AnimalInfoButton _animalInfoButtonPrefab;
    [SerializeField] private GameObject _buttonParent;
    private List<AnimalInfoButton> _animalInfoButtons = new List<AnimalInfoButton>();
    private void Start()
    {
        for(int i = 0; i < _animalsInfo.Count; i++)
        {
            var button = Instantiate(_animalInfoButtonPrefab);
            button.transform.parent = _buttonParent.transform;
            button.ChangeSprite(_animalsInfo[i].imageButton);
            button.transform.localScale = Vector3.one;

            var index = i;
            button.Button.onClick.AddListener(() => {
                Click(_animalsInfo[index]);
            });

            _animalInfoButtons.Add(button);
        }
    }
    private void Click(AnimalInfo animalInfo)
    {
        _name.text = animalInfo.animalScriptable.Name;
        _description.text = animalInfo.animalScriptable.Description;
        _image.sprite = animalInfo.imageFullSize;


        switch (animalInfo.animalScriptable.FavouriteFoods)
        {
            case FavouriteFood.everething:
            _favouriteFoodUI.ShowMeatAndGrassImages();
            break;
            case FavouriteFood.grass:
            _favouriteFoodUI.ShowGrassImage();
            break;
            case FavouriteFood.meat:
            _favouriteFoodUI.ShowMeatImage();
            break;
        }
    }
    public void ChangeTitleField(string text)
    {
        _name.text = text;
    }

    public void ChangeDescriptionField(string text)
    {
        _description.text = text;
    }

    public void ShowMeatIcon()
    {
        _favouriteFoodUI.ShowMeatImage();
    }

    public void ShowGrassIcon()
    {
        _favouriteFoodUI.ShowGrassImage();
    }

    public void ShowMeatAndGrassIcons()
    {
        _favouriteFoodUI.ShowMeatAndGrassImages();
    }
}
