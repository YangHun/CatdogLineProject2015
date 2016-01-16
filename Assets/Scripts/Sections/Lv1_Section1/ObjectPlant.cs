using UnityEngine;
using System.Collections;

public class ObjectPlant : UnitData, IHealable {

    private HealType m_Type = HealType.GREEN;

    [SerializeField] private GameObject Leaf1;
    [SerializeField] private GameObject Leaf2;

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
}
