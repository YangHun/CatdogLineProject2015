using UnityEngine;
using System.Collections;

public abstract class Section : MonoBehaviour
{

    public TriggerZone mEntranceZone = null;
    public TriggerZone mExitZone = null;

    public virtual void Start()
    {
        if (mEntranceZone != null)
            mEntranceZone.AddListener(TriggerType.ENTER, OnPlayerAtEntrance);
        if (mExitZone != null)
            mExitZone.AddListener(TriggerType.ENTER, OnPlayerAtExit);
    }

    public void OnPlayerAtEntrance(GameObject zone, Collider2D col)
    {
        if (col.gameObject == PlayerManager.Inst().GetPlayer())
            SectionManager.Inst().OnSectionEnter(this);
    }

    public void OnPlayerAtExit(GameObject zone, Collider2D col)
    {
        if (col.gameObject == PlayerManager.Inst().GetPlayer())
            SectionManager.Inst().OnSectionExit(this);
    }

    public virtual void OnHealed(IHealable target, HealInfo info)
    {
        target.OnHealed(info);
    }

    public virtual void OnAttacked(UnitData target, AttackInfo info)
    {
        target.GiveDamage(info.amount);
    }

    public virtual void OnInteractStart(IInteractor target)
    {
        target.OnInteractStart();
    }

    public virtual void OnInteractStay(IInteractor target)
    {
        target.OnInteractStay();
    }

    public virtual void OnInteractEnd(IInteractor target)
    {
        target.OnInteractEnd();
    }

    public virtual void OnDie(UnitData unit)
    {
        unit.OnDestroyObject();
    }

    /// <summary>
    /// Called when section start
    /// </summary>
    public abstract void OnSectionStart();

    /// <summary>
    /// Called when section pause
    /// </summary>
    public abstract void OnSectionPause();

    /// <summary>
    /// Called when section resume from pause
    /// </summary>
    public abstract void OnSectionResume();

    /// <summary>
    /// Called when section end
    /// </summary>
    public abstract void OnSectionEnd();

    /// <summary>
    /// Called when scoring
    /// </summary>
    /// <returns> value [0..1] </returns>
    public abstract float GetSectionScore();
    
}
