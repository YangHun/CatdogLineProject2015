using UnityEngine;
using System.Collections;

public class ObjectHurted : UnitData, IHealable {

    public float DamageSecond;
    public float DamageAmount;
    public HealType m_Type;
    private float t = 0;
    private bool m_IsPlayerInRange = false;

    void Update ()
    {
        if (!IsHealthy())
        {
            t += Time.deltaTime;
            if (t >= DamageSecond)
            {
                GiveDamage(DamageAmount);
                t = 0;
            }
        }
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

    public Vector2 GetPOS()
    {
        return transform.position;
    }
}
