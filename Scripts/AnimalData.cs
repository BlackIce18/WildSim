using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "Animals", order = 51)]
public class AnimalData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _prefab;
    [Range(0.0f, 1.0f)][SerializeField] private float _satiety = 0.5f; // сытость
    [Range(0.0f, 1.0f)][SerializeField] private float _fatigue = 0.5f; // усталость
    [Range(0.0f, 1.0f)][SerializeField] private float _hydration = 0.5f;
    [Range(0.0f, 1.0f)][SerializeField] private float _mood = 0.5f;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _weight = 1f;
    [SerializeField] private float _temperature= 1f;

    public GameObject Prefab { get { return _prefab; } }
    public float Satiety { get { return _satiety; } }
    public float Fatigue { get { return _fatigue; } }
    public float Hydration { get { return _hydration; } }
    public float Mood { get { return _mood; } }
    public float Health { get { return _health; } }
    public float Speed { get { return _speed; } }
}
