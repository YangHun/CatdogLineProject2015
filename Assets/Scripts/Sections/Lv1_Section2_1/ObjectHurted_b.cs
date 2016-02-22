using UnityEngine;
using System.Collections;

public class ObjectHurted_b : UnitData, IHealable {

    public float DamageSecond;
    public float DamageAmountJS;
    public float DamageAmount;
    public HealType m_Type;
    private float t = 0;
    [SerializeField]
    private Sprite Healed;
    [SerializeField]
    private ObjectJungSoo JS;

    void Update()
    {
        if (!JS.IsHealthy())
        {
            t += GameTime.deltaTime;
            if (t >= DamageSecond)
            {
                GiveDamage(DamageAmountJS);
                t = 0;
            }
        }
        else if (!IsHealthy())
        {
            t += GameTime.deltaTime;
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
        if (IsHealthy() && JS.IsHealthy())
            this.GetComponent<SpriteRenderer>().sprite = Healed;
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
