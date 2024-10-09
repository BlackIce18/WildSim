using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "AnimalScriptableObject", order = 0)]
public class AnimalScriptableObject : ScriptableObject
{

    [SerializeField] private string _name;
    [SerializeField] private FavouriteFood _favouriteFoods = FavouriteFood.everething;
    
    [TextArea]
    [SerializeField] private string _description;

    public string Name { get { return _name; } }
    public FavouriteFood FavouriteFoods { get { return _favouriteFoods; } }
    public string Description { get { return _description; } }
}
