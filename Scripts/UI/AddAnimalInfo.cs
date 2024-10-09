using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AddAnimalInfo : MonoBehaviour
{
    [SerializeField] private List<Animal> animals = new List<Animal>();
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private List<AnimalInfoButton> buttons = new List<AnimalInfoButton>();
    public void AddToList(Animal animal)
    {
        animals.Add(animal);
        buttons.Add(Instantiate(buttonPrefab, buttonsParent.transform).GetComponent<AnimalInfoButton>());
    }

    public void RemoveFromList(Animal animal)
    {
        for(int i = 0; i < animals.Count; i++)
        {
            if (animals[i] == animal)
            {
                animals.RemoveAt(i);
            }
        }
    }

    public void RemoveAt(int index)
    {
        animals.RemoveAt(index);
        buttons.RemoveAt(index);
        Destroy(buttonsParent.transform.GetChild(index).gameObject);
    }


}
