using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadioButtonImage : MonoBehaviour
{
    public Color32 ActiveColor { get { return _activeColor; } }
    public Color32 DefaultColor { get { return _defaultColor; } }

    [SerializeField] private Color32 _defaultColor;
    [SerializeField] private Color32 _activeColor;

    private Image _icon;

    private void Start()
    {
        _icon = GetComponent<Image>();

        ChangeColor(_defaultColor);
    }
    public void ChangeColor(Color32 color32)
    {
        _icon.color = color32;
    }

    public void SetToActiveColor()
    {
        _icon.color = ActiveColor;
    }

    public void SetToDefaultColor()
    {
        _icon.color = DefaultColor;
    }
}
