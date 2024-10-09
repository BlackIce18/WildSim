using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Bear : Animal, IAnimale
{
    //[SerializeField] private AnimalParams parameters;
    [SerializeField] private float _decreaseParametersTime = 15f;
    [SerializeField] private GameObject _bones;
    [SerializeField] private AIMoveTo _aIMoveTo;
    [SerializeField] private Animator _animator;
    [SerializeField] private FoodSearch _foodSearch;
    [SerializeField] private Bot _bot;
    public static event Action OnDie;


    private delegate IEnumerator TimerDelegate(float time);
    private TimerDelegate _drinkTimer;
    private TimerDelegate _eatTimer;
    private TimerDelegate _sleepTimer;
    private bool _drinking = false;
    private bool _findFood = false;
    private void Start()
    {
        //MoveRandomly();
        //_aIMoveTo.ChangeGoal(home.transform);
        _decreaseParametersTime = _decreaseParametersTime / Time.timeScale;

        StartCoroutine(LoopTimer(_decreaseParametersTime));
        //DrinkWhile();
        //Drink();
        _canReproduct = UnityEngine.Random.Range(1, 5);
    }
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        parameters.gender = GetRandomGender();

        _drinkTimer = DrinkTimer;
        _eatTimer = EatTimer;
        _sleepTimer = SleepTimer;

        home.transform.parent = transform.parent;
        movePoint.transform.parent = transform.parent;
        _aIMoveTo.goal = movePoint.transform;
    }

    private Gender GetRandomGender()
    {
        int gender = UnityEngine.Random.Range(0, 2);

        if (gender == 0)
        {
            return Gender.male;
        }
        else
        {
            return Gender.female;
        }
    }

    private IEnumerator LoopTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Loop();
        //Die();
        StartCoroutine(LoopTimer(time));
    }

    private void Loop()
    {
        currentLifeSeconds += _decreaseParametersTime;
        if (state != AnimalState.sleeping)
            parameters.fatigue -= 5;
        if (state != AnimalState.eating)
            parameters.satiety -= 5;
        if(state != AnimalState.drinking)
            parameters.hydration -= 5;
        if ((state != AnimalState.sleeping) || (state != AnimalState.hunting) || (state != AnimalState.playing))
            parameters.mood -= 5;

        if(parameters.mood <= 0)
        {
            parameters.health -= 1;
        }
        if(parameters.fatigue <= 0)
        {
            parameters.mood -= 10;
            parameters.health -= 5;
        }
        if(parameters.satiety <= 0)
        {
            parameters.mood -= 10;
            parameters.health -= 5;
        }
        if(parameters.hydration <= 0)
        {
            parameters.mood -= 10;
            parameters.health -= 5;
        }

        if(GameController.instance.DetectorsController.CurrentTemperature > 18)
        {
            parameters.mood += 1;
            parameters.hydration -= 1;
        }

        if (GameController.instance.DetectorsController.CurrentHumidity > 70)
        {
            parameters.hydration += 2;
            parameters.mood -= 3;
        }

        if (parameters.mood > 0 && parameters.satiety > 0 && parameters.hydration > 0)
        {
            if(parameters.health < parameters.maxHealth)
            {
                parameters.health += 3;
            }
        }

        if (GameController.instance.DetectorsController.CurrentTemperature < 10)
        {
            parameters.fatigue -= 5;
            parameters.mood -= 3;
            parameters.satiety -= 5;
        }
    }

    public override void Drink()
    {
        state = AnimalState.drinking;
        WaterSpot waterSpot = base.GetMinDistanceWaterSpot();
        MoveTo(waterSpot.transform);
        if(_aIMoveTo.CheckDestinationReached())
        {
            _drinking = true;
        }
        if(_drinking)
        {
            StartCoroutine(WaitForDestinationAndDoSomething(DrinkTimer));
        }
    }

    private bool _start = false;
    delegate void deleg();
    private void DrinkWhile()
    {
        deleg sd;
        sd = DoSomething;
        StartCoroutine(Wait(sd));
    }
    
    private IEnumerator Wait(deleg function)
    {
        _start = true;
        //movePoint.transform.position = GameController.instance.WaterSpots[0].transform.position;
        MoveTo(GameController.instance.WaterSpots[0].transform);

        do
        {
            yield return new WaitForSeconds(1f);
        }
        while (!_aIMoveTo.CheckDestinationReached());

        _start = false;
        function();
    }

    private void DoSomething()
    {
        MoveRandomly();
    }

    private IEnumerator WaitForDestinationAndDoSomething(TimerDelegate timerDelegate)
    {
        yield return new WaitForSeconds(3f);
        if(_aIMoveTo.CheckDestinationReached())
        {
            _animator.Play("Base Layer.Bear_IDLE", 0, 0.25f);
            StartCoroutine(timerDelegate(5));
            StopCoroutine("WaitForDestinationAndDoSomething");
        }
        else
        {
            StartCoroutine(WaitForDestinationAndDoSomething(timerDelegate));
        }
    }

    private IEnumerator DrinkTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (parameters.mood < parameters.maxMood)
        {
            parameters.mood += UnityEngine.Random.Range(1, 5);
        }
        if(parameters.hydration < parameters.maxHydration)
        {
            parameters.hydration += UnityEngine.Random.Range(1, 10);
        }
        _drinking = false;
    }
    private IEnumerator EatTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (parameters.mood < parameters.maxMood)
        {
            parameters.mood += UnityEngine.Random.Range(1, 2);
        }

        if (parameters.satiety < parameters.maxSatiety)
        {
            parameters.satiety += UnityEngine.Random.Range(1, 10);
        }

        if (parameters.hydration < parameters.maxHydration)
        {
            parameters.hydration += UnityEngine.Random.Range(1, 2);
        }
        _findFood = false;
    }
    private IEnumerator SleepTimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (parameters.mood < parameters.maxMood)
        {
            parameters.mood += UnityEngine.Random.Range(1, 5);
        }

        if (parameters.fatigue < parameters.maxFatigue)
        {
            parameters.fatigue += UnityEngine.Random.Range(1, 10);
        }
    }
    public override void Eat()
    {
        state = AnimalState.eating;
	
        switch(parameters.favouriteFood)
        {
            case FavouriteFood.everething:
            {
                if(_findFood == false)
                {
                    GameObject grass = GameController.instance.GetNearGrass(foodSearch.collider);
                    MoveTo(grass.transform);
                }
                if (_aIMoveTo.CheckDestinationReached())
                {
                    _findFood = true;
                }
                if (_findFood)
                {
                    StartCoroutine(WaitForDestinationAndDoSomething(EatTimer));
                }
                _findFood = true;
                break;
            }
            case FavouriteFood.meat:
            {
		        //GameController.instance.getMeatFood();
                break;
            }
            case FavouriteFood.grass:
            {
		        //GameController.instance.getGrassFood();
                break;
            }
        }
		WaitForDestinationAndDoSomething(EatTimer);

        //StartCoroutine(Eats());
    }
    public void EatObject(GameObject gameObject)
    {
        state = AnimalState.eating;

        switch (parameters.favouriteFood)
        {
            case FavouriteFood.everething:
            {
                if (parameters.satiety < (parameters.maxSatiety / 2))
                {
                    GameController.instance.GetNearMeat(_foodSearch.collider);
                }
                else
                {
                    GameController.instance.GetNearGrass(_foodSearch.collider);
                }
                break;
            }
            case FavouriteFood.meat:
            {
                GameController.instance.GetNearMeat(_foodSearch.collider);
                break;
            }
            case FavouriteFood.grass:
            {
                GameController.instance.GetNearGrass(_foodSearch.collider);
                break;
            }
        }
    }
    private IEnumerator Eats() 
    {
        yield return new WaitForSeconds(3);
    }

    public override void Move()
    {
        //state = AnimalState.moving;
        _animator.Play("Base Layer.Bear_Move", 0, 0.25f);
        _aIMoveTo.ChangeGoal(movePoint.transform);
    }

    public void MoveTo(Transform position)
    {
        //state = AnimalState.moving;
        _animator.Play("Base Layer.Bear_Move", 0, 0.25f);
        movePoint.transform.position = position.position;
        if(position == null)
        {
            Debug.Log(position + " null");
        }
        _aIMoveTo.ChangeGoal(position);
    }

    public void MoveRandomly()
    {
        if(state != AnimalState.moving)
        {
            if (_aIMoveTo.CheckDestinationReached())
            {
                float x = UnityEngine.Random.Range(transform.position.x, transform.position.x + UnityEngine.Random.Range(-150, 150));
                float y = UnityEngine.Random.Range(transform.position.y, transform.position.y + UnityEngine.Random.Range(-25, 25));
                float z = UnityEngine.Random.Range(transform.position.z, transform.position.y + UnityEngine.Random.Range(-150, 150));
                Vector3 newPoint = new Vector3(x, y, z);
                movePoint.transform.position = newPoint;
                Move();
                state = AnimalState.playing;
            }
            else
            {
                state = AnimalState.moving;
            }
        }
    }

    public override void Reproduction()
    {
        state = AnimalState.reproduction;
        if(_canReproduction && (_currentReproduct <= _canReproduct))
        {
            List<Animal> animals = GameController.instance.GetListOfAnimalsByType(this);

            foreach(Animal animal in animals)
            {
                if(((animal.parameters.gender == Gender.female) && (this.parameters.gender == Gender.male)) || ((animal.parameters.gender == Gender.male)&& (this.parameters.gender == Gender.female)))
                {
                    if(animal.parameters.family == null)
                    {
                        animal.parameters.family = this;
                        this.parameters.family = animal;
                        this.MoveTo(animal.transform);
                        GameObject bear = GameController.instance.prefabSpawner.SpawnBear(this.transform).gameObject;
                        GameController.instance.AddAnimalToList(bear.GetComponent<Animal>());
                        bear.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        _currentReproduct++;
                        break;
                    }
                }
            }
        }
    }

    public override void Sleep()
    {
        _animator.Play("Base Layer.Bear_Sleep",0,0.25f);
        MoveTo(home.transform);
        StartCoroutine(WaitForDestinationAndDoSomething(_sleepTimer));
        state = AnimalState.sleeping;
    }

    public override void Die()
    {
        if (parameters.health <= 0)
        {
            StopCoroutine(LoopTimer(_decreaseParametersTime));
            _animator.Play("Base Layer.Bear_Sleep", 0, 0.25f);
            GameController.instance.RemoveAnimalFromList(this);
            Debug.Log(_bot.Fitness + " " + currentLifeSeconds);
            OnDie?.Invoke();
            StartCoroutine(DieTimer());
        }
    }


    private IEnumerator DieTimer()
    {
        yield return new WaitForSeconds(8f * Time.deltaTime);
        DestroyAnimal();
    }

    private void DestroyAnimal()
    {
        Destroy(home.gameObject);
        Destroy(movePoint.gameObject);
        Destroy(this.gameObject);
    }

    public override void ApplyDamage(float damage)
    {
        parameters.health -= damage;

        if (parameters.health <= 0)
        {
            Die();
        }
    }
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }
}
