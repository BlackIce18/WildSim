using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Detectors : MonoBehaviour
{
    [SerializeField] private TimeProgressor _timeProgressor;
    [SerializeField] private List<float> _timeValue = new List<float>();
    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private string _postfixTextUI;
    protected float _currentValue;

    public float CurrentValue { get { return _currentValue; } }
    public virtual void Regulate()
    {
        int currentHour = _timeProgressor.CurrentHour;
        if(currentHour == 24) { currentHour = 0; }
        _currentValue = _timeValue[currentHour];
        _textUI.text = _currentValue.ToString()+" "+_postfixTextUI;
    }
}
