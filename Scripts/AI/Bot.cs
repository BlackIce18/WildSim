using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class Bot : MonoBehaviour
{
    //public float speed;//Speed Multiplier
    //public float rotation;//Rotation multiplier
    //public LayerMask raycastMask;//Mask for the sensors

    private float[] input = new float[8];//input to the neural network
    public NeuralNetwork network;
    public Bear animal;

    public int position;//Checkpoint number on the course
    public bool collided;//To tell if the car has crashed


    private NeuralNetwork _brain;
    [SerializeField] private float _fitness;
    public float Fitness { get { return _fitness; } private set { _fitness = value; } }
    private bool _alive = true;
    private float _spawnTime;

    private Vector3 previousPosition;

    public void SetBrain(NeuralNetwork brain)
    {
        _brain = brain;
        Fitness = 0;
    }
    private void Start()
    {
        OnCreate();
    }

    private void Update()
    {
        if (_alive)
        {
            //Fitness = Time.time - _spawnTime;
            UpdateFitness();
            if(_brain == null)
            {
                NeuralNetwork net = new NeuralNetwork(Manager.layers);
                net.Load("Assets/ModelSave.txt");
                _brain = net;
                //Debug.Log(net.biases);
            }
            UseNeuroNetwork();
        }
    }
    private IEnumerator CheckCheat()
    {
        yield return new WaitForSeconds(10f);
        previousPosition = transform.position;
        yield return new WaitForSeconds(10f);
        if((transform.position.x + 5 >= previousPosition.x) || (transform.position.x - 5 >= previousPosition.x) &&
            (transform.position.y + 5 >= previousPosition.y) || (transform.position.y - 5 >= previousPosition.y) &&
            (transform.position.z + 5 >= previousPosition.z) || (transform.position.z - 5 >= previousPosition.z))
        {
            Fitness -= 50f;
        }
    }
    public void OnCreate()
    {
        _spawnTime = Time.time;
    }

    private void UseNeuroNetwork()
    {
        input = new float[9];
        input[0] = animal.parameters.satiety;
        input[1] = animal.parameters.hydration;
        input[2] = animal.parameters.fatigue;
        input[3] = animal.parameters.mood;
        input[4] = GameController.instance.DetectorsController.CurrentTemperature;
        input[5] = GameController.instance.DetectorsController.CurrentHumidity;
        input[6] = GameController.instance.DetectorsController.CurrentPressure;
        input[7] = animal.parameters.speed;
        input[8] = animal.parameters.health;
        //inputs[6] = transform.position; // Расстояние до ...

        var output = _brain.FeedForward(input);

        int max = 0;

        for(int i = 0; i < output.Length; i++)
        {
            if (output[max] < output[i])
            {
                max = i;
            }
        }

        switch (max)
        {
            case 0:
                animal.Eat();
                if (animal.parameters.satiety == animal.parameters.maxSatiety)
                {
                    Fitness -= 0.5f;
                }
                animal.Eat();
                StartCoroutine(CheckCheat());
                //Debug.Log("Eat");
                break;
            case 1:
                //Debug.Log("Drink");
                if(animal.parameters.hydration == animal.parameters.maxHydration && (animal.state == AnimalState.drinking))
                {
                    Fitness -= 0.5f;
                }
                
                animal.Drink();
                StartCoroutine(CheckCheat());
                
                break;
            case 2:
                //Debug.Log("Sleep");
                if(GameController.instance.DaynightController.CurrentHour == animal.parameters.sleepFrom)
                {
                    Fitness += 0.3f;
                }
                if(animal.parameters.fatigue >= animal.parameters.maxFatigue)
                {
                    StartCoroutine(CheckCheat());
                }
                animal.Sleep();
                break;
            case 3:
                //Debug.Log("DoSomething");
                animal.MoveRandomly();
                break;
            case 4:
                if((animal._currentReproduct == animal._canReproduct) && (animal.state == AnimalState.reproduction))
                {
                    Fitness -= 0.5f;
                }
                //Debug.Log("Reproduction");
                animal.Reproduction();
                StartCoroutine(CheckCheat());
                break;
        }
    }

    public void UpdateFitness()
    {
        //network.fitness = position;
        Fitness = Time.time - _spawnTime; //обновляет пригодность сети для сортировки
        if(animal.parameters.health > 0)
        {
            Fitness += 0.5f;
        }
        if(animal.parameters.health < 0)
        {
            Fitness -= 0.7f;
        }
        if (animal.parameters.hydration > 0)
        {
            Fitness += 0.2f;
        }
        if(animal.parameters.hydration <= 0)
        {
            Fitness -= 0.2f;
        }
        if (animal.parameters.satiety > 0)
        {
            Fitness += 0.2f;
        }
        if (animal.parameters.satiety <= 0)
        {
            Fitness -= 0.2f;
        }
        if (animal.parameters.mood > 0)
        {
            Fitness += 0.1f;
        }
        if (animal.parameters.mood <= 0)
        {
            Fitness -= 0.1f;
        }
        if (animal.parameters.fatigue > 0)
        {
            Fitness += 0.2f;
        }
        if (animal.parameters.fatigue <= 0)
        {
            Fitness -= 0.2f;
        }
        if (animal.parameters.family != null)
        {
            Fitness += 0.65f;
        }

        if ((GameController.instance.DaynightController.CurrentHour == animal.parameters.sleepFrom) && (animal.state == AnimalState.sleeping))
        {
            Fitness += 1f;

            if ((GameController.instance.DaynightController.CurrentHour + 1 == animal.parameters.sleepTo) && (animal.state == AnimalState.sleeping))
            {
                Fitness += 1f;

                if ((GameController.instance.DaynightController.CurrentHour >= animal.parameters.sleepTo) && (animal.state != AnimalState.sleeping))
                {
                    Fitness += 0.3f;
                }
            }
        }
    }
}
