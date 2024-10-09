using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject currentPrefab;
    public Bear bearPrefab;
    [SerializeField] private Transform _parent;

    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        DetectObjectWithRaycast();
    }
    public void DetectObjectWithRaycast()
    {
        if (Input.GetMouseButtonDown(0) && (currentPrefab != null))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject g = Instantiate(currentPrefab, hit.point, Quaternion.identity, _parent);

                Animal animal = g.GetComponent<Animal>();
             
                if (animal)
                {
                    GameController.instance.AddAnimalToList(animal);
                    animal.home.homePoint.position = hit.point;
                }

                currentPrefab = null;

                Debug.Log($"{hit.collider.name} Detected", hit.collider.gameObject);
            }
        }
    }

    public void ChangePrefab(GameObject newPrefab)
    {
        currentPrefab = newPrefab;
    }

    public void ChooseRandomPrefab(TabsPrefabList tabsPrefabList)
    {
        if((tabsPrefabList.prefabsList != null) && (tabsPrefabList.prefabsList.Count > 0))
        {
            int rand = UnityEngine.Random.Range(0, tabsPrefabList.prefabsList.Count - 1);
            currentPrefab = tabsPrefabList.prefabsList[rand];
        }
    }

    public void ClearPrefab()
    {
        currentPrefab = null;
    }

    public void Spawn()
    {
        if(currentPrefab != null)
        {

            //if(activeRadioButton.isActiveAndEnabled == true)
            //{

            //}
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(touchPos);
            GameObject g = Instantiate(currentPrefab, touchPos, Quaternion.identity);
            //Bear bear = new Bear(animal);
            //Instantiate(animal.data.Prefab);
        }
    }

    public Bear SpawnBear(Transform position)
    {
        if(bearPrefab != null)
        {
            GameObject g = Instantiate(currentPrefab, position.position, Quaternion.identity);
            Bear animal = g.GetComponent<Bear>();
            GameController.instance.AddAnimalToList(animal);
            animal.home.homePoint.position = position.position;
            return animal;
        }
        return null;
    }


    public GameObject SpawnPrefab(GameObject prefab, Vector3 spawnPoint)
    {
        GameObject g = Instantiate(prefab, spawnPoint, Quaternion.identity, _parent);

        Animal animal = g.GetComponent<Animal>();

        if (animal)
        {
            GameController.instance.AddAnimalToList(animal);
            animal.home.homePoint.position = spawnPoint;
        }

        return g;
    }
}
