using UnityEngine;
using System.Collections;

public class ObjectPlant : UnitData, IHealable {

    public HealType m_Type = HealType.GREEN;

    public GameObject Leaf1;
    public GameObject Leaf2;

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
        if (IsHealthy())
        {
            Leaf1.SetActive(true);
            Leaf2.SetActive(true);
        }

    }

    public bool IsHealable()
    {
        return true;
    }

    public bool IsOn()
    {
        return IsHealthy();
    }
}
