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

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject == GameManager.Inst().GetPlayer())
    //    {
    //        m_IsPlayerInRange = true;
    //        Debug.Log("Hurted : Player In Range");
    //    }
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject == GameManager.Inst().GetPlayer())
    //    {
    //        m_IsPlayerInRange = false;
    //        Debug.Log("Hurted : Player Out of Range");
    //    }
    //}

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
    }

    public bool IsHealable()
    {
//        if (m_IsPlayerInRange)
            return true;
//        else
//            return false;
    }

    public Vector2 GetPOS()
    {
        return transform.position;
    }
}
