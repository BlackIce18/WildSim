using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavouriteFoodUI : MonoBehaviour
{
    [SerializeField] private Image _meat;
    [SerializeField] private Image _grass;

    public void ShowMeatImage()
    {
        _meat.gameObject.SetActive(true);
        _grass.gameObject.SetActive(false);
    }

    public void ShowGrassImage()
    {
        _grass.gameObject.SetActive(true);
        _meat.gameObject.SetActive(false);

    }

    public void ShowMeatAndGrassImages()
    {
        _meat.gameObject.SetActive(true);
        _grass.gameObject.SetActive(true);
    }
}
