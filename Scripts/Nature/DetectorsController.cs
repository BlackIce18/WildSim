using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorsController : MonoBehaviour
{
    [SerializeField] private TemperatureController _currentTemperature;
    [SerializeField] private HumidityController _currentHumidity;
    [SerializeField] private PressureController _currentPressure;

    public float CurrentTemperature { get { return _currentTemperature.CurrentValue; } }
    public float CurrentHumidity { get { return _currentHumidity.CurrentValue;} }
    public float CurrentPressure { get { return _currentPressure.CurrentValue; } }


}
