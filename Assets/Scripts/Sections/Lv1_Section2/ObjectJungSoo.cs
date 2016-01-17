using UnityEngine;
using System.Collections;

public class ObjectJungSoo : UnitData, IHealable {

    [SerializeField]
    private HealType m_Type;

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
    }

    public bool IsHealable()
    {
        if (GameManager.Inst().GetHealType() == m_Type)
            return true;
        else
            return false;
    }
}
