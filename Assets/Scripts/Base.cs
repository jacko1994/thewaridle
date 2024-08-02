using UnityEngine;

public class Base : GameEntity
{
    public int MaxHealth { get; private set; }

    protected override void Start()
    {
        base.Start();
        MaxHealth = 1000;
        Health = MaxHealth;
        AttackPower = 0; // Căn cứ không có khả năng tấn công
        MovementSpeed = 0f; // Căn cứ không di chuyển
        AttackSpeed = 0f; // Không áp dụng
        AttackRange = 0f; // Không áp dụng
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (Health <= 0)
        {
            Die(); // Gọi phương thức Die khi máu về 0
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

    protected override bool IsAttackable()
    {
        // Căn cứ luôn có thể bị tấn công
        return true;
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Base destroyed!");
        // Thêm logic cần thiết khi căn cứ bị phá hủy
    }
}
