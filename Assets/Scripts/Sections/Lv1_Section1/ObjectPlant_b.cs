using UnityEngine;
using System.Collections;

public class ObjectPlant_b : UnitData, IHealable {

    public HealType m_Type = HealType.GREEN;

    public GameObject Leaf1;
    public GameObject Leaf2;

    public float Timer;
    private float BackTimer;

    void Update()
    {
        BackTimer -= Time.deltaTime;
        if (BackTimer <= 0)
        {
            Leaf1.SetActive(false);
            Leaf2.SetActive(false);
            GiveDamage(2f);
        }
    }

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
            BackTimer = Timer;
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