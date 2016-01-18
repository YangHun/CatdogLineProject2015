using UnityEngine;
using System.Collections;

public class ObjectJungSoo : UnitData, IHealable {

    [SerializeField]
    private HealType m_Type;

    public void OnHealed(HealInfo heal)
    {
        GiveHeal(100f);
    }

    public bool IsHealable()
    {
        if (GameManager.Inst().GetHealType() == m_Type)
            return true;
        else
            return false;
    }
}
