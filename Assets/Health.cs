using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    public int MaxHP => maxHP;

    public int CurrentHP { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<int, int> OnHPChanged; // (current, max)
    public event Action OnDeath;
    public event Action OnFullHealth;

    void Awake()
    {
        CurrentHP = maxHP;
        IsDead = false;
        OnHPChanged?.Invoke(CurrentHP, maxHP);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        if (amount <= 0) return;

        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            IsDead = true;
            OnHPChanged?.Invoke(CurrentHP, maxHP);
            OnDeath?.Invoke();
            return;
        }

        OnHPChanged?.Invoke(CurrentHP, maxHP);
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        if (amount <= 0) return;

        int before = CurrentHP;
        CurrentHP = Mathf.Min(maxHP, CurrentHP + amount);

        if (CurrentHP != before)
            OnHPChanged?.Invoke(CurrentHP, maxHP);

        if (CurrentHP == maxHP)
            OnFullHealth?.Invoke();
    }

    public void ResetToFull()
    {
        IsDead = false;
        CurrentHP = maxHP;
        OnHPChanged?.Invoke(CurrentHP, maxHP);
        OnFullHealth?.Invoke();
    }
}
