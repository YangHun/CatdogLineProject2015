using UnityEngine;
using System.Collections;

public class ObjectSickVine : UnitData, IHealable{
   
    public HealType m_Type = HealType.GREEN;

    Vector2 GetPosition()
    {
        return this.transform.position;
    }

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
    }

    public bool IsHealable()
    {
        return true;
    }

}