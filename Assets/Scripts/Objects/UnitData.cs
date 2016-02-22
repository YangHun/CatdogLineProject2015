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
	[SerializeField]
	private bool PlayWarningOnDanger = true;

	private WarningManager m_WarningManager = null;

	public virtual void Start()
	{
		if(PlayWarningOnDanger == true)
			m_WarningManager = GameUIManager.Inst().CreateWarningManager(this, (unit) => { return unit.CurrentHP < 30; });
	}

	public override void OnDestroyObject()
	{
		base.OnDestroyObject();
		if (m_WarningManager != null)
			GameUIManager.Inst().DestroyWarningManager(m_WarningManager);
    }

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

            if (PlayEffectOnHealthy && !Healthy)
            {
                EffectInfo info = new EffectInfo();
                info.name = "Healthy";
                info.position = transform.position;
                info.rotation = transform.rotation;
                info.existtime = 3.0f;

                EffectManager.Inst().CreateEffect(info);
            }

            Healthy = true;
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