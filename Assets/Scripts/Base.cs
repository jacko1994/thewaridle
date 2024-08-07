﻿using UnityEngine;

public class Base : GameEntity
{
    public int MaxHealth { get; private set; }

    protected override void OnEnable()
    {
        IsMobile = false;
        MaxHealth = 100;
        Health = MaxHealth;
        AttackPower = 0;
        MovementSpeed = 0f;
        AttackSpeed = 0f;
        AttackRange = 0f;

        base.OnEnable();

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
        TheWarIdleManager.Instance.GameOver();

        //base.Die();
    }
}
