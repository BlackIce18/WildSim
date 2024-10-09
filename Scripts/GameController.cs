using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private List<WaterSpot> _waterSpots = new List<WaterSpot>();
    [SerializeField] private List<Transform> _grass = new List<Transform>();

    [SerializeField] private List<Animal> _animals = new List<Animal>();
    public List<Animal> Animals { get { return _animals; } }

    [SerializeField] private List<Animal> _carnivores = new List<Animal>();
    public List<Animal> Carnivores { get { return _carnivores; } }

    [SerializeField] private List<Animal> _herbivores = new List<Animal>();
    public List<Animal> Herbivores { get { return _herbivores; } }

    [SerializeField] private List<Animal> _omnivores = new List<Animal>();
    public List<Animal> Omnivores { get { return _omnivores; } }

    public List<WaterSpot> WaterSpots { get { return _waterSpots; } private set { _waterSpots = value; } }
    [SerializeField] private DetectorsController _detectorsController;
    public DetectorsController DetectorsController { get { return _detectorsController; } }
    [SerializeField] private TimeProgressor _dayNightController;
    public TimeProgressor DaynightController { get { return _dayNightController; } }

    public PrefabSpawner prefabSpawner;

    public Collider meat;

    public AddAnimalInfo addAnimalInfo;


    private void Awake()
    {
        instance = this;
    }

    public void RemoveAnimalFromList(Animal animal)
    {
        addAnimalInfo.RemoveFromList(animal);
        if (animal.parameters.favouriteFood == FavouriteFood.grass)
        {
            for (int i = 0; i < _herbivores.Count; i++)
            {
                if (_herbivores[i] == animal)
                {
                    _herbivores.RemoveAt(i);
                    break;
                }
            }
        }
        else if (animal.parameters.favouriteFood == FavouriteFood.meat)
        {
            for (int i = 0; i < _carnivores.Count; i++)
            {
                if (_carnivores[i] == animal)
                {
                    _carnivores.RemoveAt(i);
                    break;
                }
            }
        }
        else if(animal.parameters.favouriteFood == FavouriteFood.everething)
        {
            for (int i = 0; i < _omnivores.Count; i++)
            {
                if (_omnivores[i] == animal)
                {
                    _omnivores.RemoveAt(i);
                    break;
                }
            }
        }

        for (int i = 0; i < _animals.Count; i++)
        {
            if (_animals[i] == animal)
            {
                GameController.instance._animals.RemoveAt(i);
                break;
            }
        }

        _animals = _animals.Where(x => x != null).ToList();
        _herbivores = _animals.Where(x => x != null).ToList();
        _carnivores = _animals.Where(x => x != null).ToList();
        _omnivores = _animals.Where(x => x != null).ToList();
       /* GameController.instance._animals.RemoveAll(item => item == null);
        GameController.instance._herbivores.RemoveAll(item => item == null);
        GameController.instance._carnivores.RemoveAll(item => item == null);
        GameController.instance._omnivores.RemoveAll(item => item == null);*/
    }

    public void AddAnimalToList(Animal animal)
    {
        if(animal != null)
        {
            if(animal.parameters.favouriteFood == FavouriteFood.grass)
            {
                _herbivores.Add(animal);
            }
            if(animal.parameters.favouriteFood == FavouriteFood.meat)
            {
                _carnivores.Add(animal);
            }
            if(animal.parameters.favouriteFood == FavouriteFood.everething)
            {
                _omnivores.Add(animal);
            }
            addAnimalInfo.AddToList(animal);
            _animals.Add(animal);
        }
        else
        {
            Debug.Log("Animal null");
        }
    }

    public List<Animal> GetListOfAnimalsByType(Animal animale)
    {
        List<Animal> list = new List<Animal>();

        for(int i = 0; i < GameController.instance._animals.Count; i++)
        {
            if(animale.parameters.name == GameController.instance._animals[i].parameters.name)
            {
                list.Add(GameController.instance._animals[i]);
            }
        }

        return list;
    }

    public List<Animal> GetMeat()
    {
        return _herbivores;
    }

    public List<Transform> GetGrassFood()
    {
        return _grass;
    }

    public GameObject GetNearMeat(Collider collider)
    {
        return meat.gameObject;
        /*GameObject nearestGrass = null;
        float minDistance = float.MaxValue;

        // Поиск ближайшего коллайдера, соприкасающегося с медведем
        foreach (Transform grassTransform in _animals)
        {
            Collider grassCollider = grassTransform.GetComponent<Collider>();
            if (grassCollider != null && collider.bounds.Intersects(grassCollider.bounds))
            {
                float distance = Vector3.Distance(collider.transform.position, grassTransform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestGrass = grassTransform.gameObject;
                }
            }
        }

        if (nearestGrass != null)
        {
            Debug.Log("Nearest Grass: " + nearestGrass.name + " at distance " + minDistance);
        }
        else
        {
            Debug.Log("No Grass collider is touching the Bear.");
        }
        return nearestGrass;*/
    }

    public GameObject GetNearGrass(Collider collider)
    {
        GameObject nearestGrass = null;
        float minDistance = float.MaxValue;

        // Поиск ближайшего коллайдера, соприкасающегося с медведем
        foreach (Transform grassTransform in _grass)
        {
            Collider grassCollider = grassTransform.GetComponent<Collider>();
            if (grassCollider != null && collider.bounds.Intersects(grassCollider.bounds))
            {
                float distance = Vector3.Distance(collider.transform.position, grassTransform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestGrass = grassTransform.gameObject;
                }
            }
        }

        if (nearestGrass != null)
        {
            //Debug.Log("Nearest Grass: " + nearestGrass.name + " at distance " + minDistance);
        }
        else
        {
            Debug.Log("No Grass collider is touching the Bear.");
        }
        return nearestGrass;
    }

    public List<Transform> GetEverythingFood(Animal animal)
    {
        List<Transform> list = new List<Transform>();
        
        //animal.foodSearch.

        return list;
    }
}
