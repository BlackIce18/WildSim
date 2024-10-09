using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public float timeframe; // время, в течение которого обучается каждое поколение ботов
    public int populationSize;// Численность популяции
    public PrefabSpawner prefabSpawner;
    public GameObject prefab;// Префаб бота

    [SerializeField] private TextMeshProUGUI epochCount;

    public static int[] layers = new int[5] { 9, 5, 3, 2, 5 };// Инициализация сети до нужного размера

    [Range(0.0001f, 1f)] public float MutationChance = 0.01f; // изменение мутации, происходящее с каждым весом или смещением

    [Range(0f, 1f)] public float MutationStrength = 0.5f; // Изменение весов и смещений на рандом значение в диапазоне -mutationStrength, mutationStrength

    [Range(0.1f, 10f)] public float Gamespeed = 1f;

    [Range(1, 1000)] public int maxEpoch = 1;
    private int currentEpoch = 0;
    //public List<Bot> Bots;
    public List<NeuralNetwork> networks;
    private List<Bot> _animals;

    void Start()
    {
        if (populationSize % 2 != 0)
            populationSize = 50; // если размер популяции неравномерен, установите его равным пятидесяти

        InitNetworks();
        InvokeRepeating("CreateBots", 5.0f, timeframe);//повторяемая функция
        
    }

    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>();
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Load("Assets/ModelSave.txt"); //при запуске загружается сохранение
            networks.Add(net);
        }
    }

    public void CreateBots()
    {
        if (currentEpoch == maxEpoch)
        {
            CancelInvoke("CreateBots");
            return;
        }

        Time.timeScale = Gamespeed;// устанавливается скорость игры, которая будет увеличиваться для ускореиня тренировки
        if (_animals != null)
        {
            for (int i = 0; i < _animals.Count; i++)
            {
                //GameObject.Destroy(_animals[i].gameObject); // если в сцене есть Prefab, убираем
                if (_animals[i] != null)
                {
                    Save(_animals[i]);
                    _animals[i].animal.parameters.health = 0;
                    _animals[i].animal.Die();
                }
            }

            SortNetworks();//сортирует сети и мутации в нем
        }

        _animals = new List<Bot>();
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 position = new Vector3(UnityEngine.Random.Range(-20, 20) + 2.55f, -0.25f, UnityEngine.Random.Range(-20, 20) + 53f);
            GameObject g = prefabSpawner.SpawnPrefab(prefab, position);
            Bot animal = g.GetComponent<Bot>();
            //Bot animal = (Instantiate(prefab, new Vector3(UnityEngine.Random.Range(-20, 20) + 2.55f, -0.25f, UnityEngine.Random.Range(-20, 20)+53f), new Quaternion(0, 0, 1, 0))).GetComponent<Bot>();//создание ботов
            //animal.network = networks[i];//развертывает сеть для каждого учащегося
            animal.SetBrain(networks[i]); //развертывает сеть для каждого учащегося
            _animals.Add(animal);
        }
        currentEpoch++;
        epochCount.text = currentEpoch.ToString();
    }

    private void Save(Bot animals)
    {
        string path = "Assets/Bear.txt";
        if(!File.Exists(path))
            File.Create(path).Close();
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(currentEpoch+") " + GameController.instance.DaynightController.CurrentHour + ":"+ GameController.instance.DaynightController.CurrentMinute + " " + animals.Fitness + " " + animals.GetComponent<Bear>().currentLifeSeconds);
        writer.Close();
    }
    public void SortNetworks()
    {
        for (int i = 0; i < populationSize; i++)
        {
            _animals[i].UpdateFitness();// позволяет ботам настраивать пригодность своих сетей
        }
        networks.Sort();
        networks[populationSize - 1].Save("Assets/ModelSave.txt");//сохраняет веса и смещения в файлах, для сохранения производительности сети
        for (int i = 0; i < populationSize / 2; i++)
        {
            networks[i] = networks[i + populationSize / 2].copy(new NeuralNetwork(layers));
            networks[i].Mutate((int)(1/MutationChance), MutationStrength);
        }
    }

    private int SortByFitness(Bot a, Bot b)
    {
        return -(a.Fitness.CompareTo(b.Fitness));
    }
}