using UnityEngine;

[RequireComponent(typeof(RadioButtonImage))]
public class RadioButtonItem : MonoBehaviour
{
    public RadioButtonImage Image { get; private set; }
    public bool isActive = false;
  
    private void Start()
    {
        Image = GetComponent<RadioButtonImage>();
    }
}
