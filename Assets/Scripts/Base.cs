using UnityEngine;

public class Base : GameEntity
{
    public int MaxHealth { get; private set; }

    protected override void Start()
    {
        base.Start();
        IsMobile = false;
        MaxHealth = 1000;
        Health = MaxHealth;
        AttackPower = 0;
        MovementSpeed = 0f;
        AttackSpeed = 0f;
        AttackRange = 0f;

        DamageBehavior = new StandardDamage(this, animatorController);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Repair(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    protected override void Die()
    {
        Debug.Log("Base Is Destroyed");

        base.Die();
    }
}
