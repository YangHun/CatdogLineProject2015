using UnityEngine;
using System.Collections;

public class ObjectSeed : UnitData, IHealable {

    public HealType m_Type = HealType.GREEN;

    public GameObject Leaf1;
	
    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
        if (IsHealthy())
        {
            Leaf1.SetActive(true);
        }
    }

    public bool IsHealable()
    {
        return true;
    }
}
