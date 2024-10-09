using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalInfoButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    public Button Button { get { return _button; } }
    public Image Image { get { return _image; } }
    public void ChangeSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
