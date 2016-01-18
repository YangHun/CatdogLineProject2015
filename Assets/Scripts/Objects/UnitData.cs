using UnityEngine;
using System.Collections;

public class UnitData : ObjectData
{
    [SerializeField]private float MaxHP = 100;
    [SerializeField]private float CurrentHP = 100;

    private bool Healthy = false;

    public void GiveDamage(float amount)
    {
        CurrentHP -= amount;

        if (CurrentHP <= 0)
            SectionManager.Inst().GetCurrentSection().OnDie(this);
        else if (CurrentHP >= MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = true;
        }
        else if (CurrentHP < MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = false;
        }
    }

    public void GiveHeal(float amount)
    {
        if (!Healthy)
            CurrentHP += amount;

        if (CurrentHP <= 0)
            SectionManager.Inst().GetCurrentSection().OnDie(this);
        else if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = true;
        }
        else if (CurrentHP < MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = false;
        }
    }

    public void GiveGuilty(float amount) //Only Healer
    {
        MaxHP -= amount;
        if (MaxHP <= 0)
            SectionManager.Inst().GetCurrentSection().OnDie(this);
        else if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }

    public float GetHP()
    {
        return CurrentHP;
    }
    public float GetMaxHP()
    {
        return MaxHP;
    }

    public bool IsFullHealth()
    {
        return CurrentHP == MaxHP;
    }

    public bool IsHealthy()
    {
        return Healthy;
    }
}
