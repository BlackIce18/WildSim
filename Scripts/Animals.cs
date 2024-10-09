

public abstract class Animals
{
    protected float _satiety; // сытость
    protected float _fatigue; // усталость
    protected float _hydration;
    protected float _mood;
    protected float _health;
    protected float _speed;
    protected AnimalState _state;
    protected FavouriteFood _favouriteFood;

    public abstract void Eat();
    public abstract void Play(float time);
    public abstract void Sleep(float time);
    public abstract void Drink();
    public abstract void Die();
    public abstract void ApplyDamage(float damage);
}
