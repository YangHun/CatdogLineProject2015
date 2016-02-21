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

    [SerializeField]
    private bool PlayEffectOnHealthy = true;

    public virtual void LateUpdate()
    {
        if (CurrentHP <= 0)
            UnitManager.Inst().OnDie(this);
    }

    public void GiveDamage(float amount)
    {
        CurrentHP -= amount;

        if (CurrentHP >= MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = true;
        }
        else if (CurrentHP < MaxHP)
        {
            Healthy = false;
        }
    }

    public virtual void GiveHeal(float amount)
    {
        if (!Healthy)
            CurrentHP += amount;
        
        if (CurrentHP >= MaxHP)
        {
            CurrentHP = MaxHP;
            Healthy = true;

            if (PlayEffectOnHealthy)
            {
                EffectInfo info = new EffectInfo();
                info.name = "Healthy";
                info.position = transform.position;
                info.rotation = transform.rotation;
                info.existtime = 3.0f;

                EffectManager.Inst().CreateEffect(info);
            }
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

    public void SetHealthy(bool a)
    {
        Healthy = a;
    }
}