using UnityEngine;
using System.Collections;

public class UnitData : ObjectData
{
    [SerializeField]
    protected float MaxHP = 100;
    [SerializeField]
    protected float CurrentHP = 100;
    [SerializeField]
    private bool Healthy = false;

    public void GiveDamage(float amount)
    {
        CurrentHP -= amount;

        if (CurrentHP <= 0)
            GameManager.Inst().OnDie(this);
        else if (CurrentHP >= MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = true;
        }
        else if (CurrentHP < MaxHP)
        {
            Healthy = false;
        }
    }

    public void GiveHeal(float amount)
    {
        if (!Healthy)
            CurrentHP += amount;

        if (CurrentHP <= 0)
            SectionManager.Inst().GetCurrentSection().OnDie(this);
        else if (CurrentHP >= MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = true;
            Debug.Log("Now Healthy");
        }
        else if (CurrentHP < MaxHP)
        {
            Healthy = false;
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
