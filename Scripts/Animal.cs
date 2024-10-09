using System;
using UnityEngine;
public interface IAnimale
{
    public void Eat();
    public void Sleep();
    public void Move();
    public void Reproduction();
    public void Die();
    public void Drink();
    public void ApplyDamage(float damage);
}
public enum AnimalState
{
    idle,
    moving,
    hunting,
    playing,
    sleeping,
    drinking,
    eating,
    reproduction
}

public enum FavouriteFood
{
    grass,
    meat,
    everething
}
public enum Gender
{
    male,
    female
}

[Serializable]
public struct AnimalParams
{
    public string name;
    public float satiety; // сытость
    public float maxSatiety; // сытость
    public float hydration;
    public float maxHydration;
    public float fatigue; // усталость
    public float maxFatigue; // усталость
    public float mood;
    public float maxMood;
    public float health;
    public float maxHealth;
    public float speed;
    public float weight;
    public float temperature;
    [Range(0,23.59f)]
    public int sleepFrom;
    [Range(0, 23.59f)]
    public int sleepTo;
    public FavouriteFood favouriteFood;
    public Gender gender;
    public Animal family;
}
public class Animal : MonoBehaviour
{
    //public AnimalData data;
    public AnimalParams parameters;
    public float currentLifeSeconds = 0;
    public GameObject movePoint;
    public AnimalHomeSpot home;
    public AnimalState state;
    public FoodSearch foodSearch;
    public bool _canReproduction = true;
    public int _canReproduct = 1;
    public int _currentReproduct = 0;
    public virtual void Eat() { }
    public virtual void Sleep() { }
    public virtual void Move() { }
    public virtual void Reproduction() { }
    public virtual void Die() { }
    public virtual void Drink() { }
    public virtual void ApplyDamage(float damage) { }

    public WaterSpot GetMinDistanceWaterSpot()
    {
        WaterSpot waterSpotMinDistance = GameController.instance.WaterSpots[0];
        Vector3 currentPosition = this.transform.position;
        float min = Vector3.Distance(waterSpotMinDistance.transform.position, currentPosition);

        for (int i = 0; i < GameController.instance.WaterSpots.Count; i++)
        {
            WaterSpot waterSpot = GameController.instance.WaterSpots[i];
            float d = Vector3.Distance(waterSpot.transform.position, currentPosition);

            if (d < min)
            {
                waterSpotMinDistance = waterSpot;
                min = d;
            }
        }
        return waterSpotMinDistance;
    }
}
