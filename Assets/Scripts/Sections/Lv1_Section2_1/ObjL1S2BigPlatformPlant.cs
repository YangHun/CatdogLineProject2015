using UnityEngine;
using System.Collections;
using System;

public class ObjL1S2BigPlatformPlant : UnitData, IHealable
{

    public HealType m_Type = HealType.GREEN;


    public GameObject Leaf_1_On = null;
    public GameObject Leaf_1_Off = null;
    public GameObject Leaf_2_On = null;
    public GameObject Leaf_2_Off = null;

    public float ActivateTime = 1.0f;

    private int m_HealLeft = 2;

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20);
        else
            GiveHeal(3);

        if (m_HealLeft == 2)
            StartCoroutine(ActivateLeaf(Leaf_1_On, Leaf_1_Off));
        else if (m_HealLeft == 1)
            StartCoroutine(ActivateLeaf(Leaf_2_On, Leaf_2_Off));


        m_HealLeft--;
        if (m_HealLeft < 0)
            m_HealLeft = 0;
    }

    public bool IsHealable() { return true; }


    IEnumerator ActivateLeaf(GameObject on, GameObject off)
    {
        float alpha = 0;
        on.SetActive(true);
        off.SetActive(true);

        var onrenderer = on.GetComponent<SpriteRenderer>();
        var offrenderer = off.GetComponent<SpriteRenderer>();

        while (alpha < 1)
        {
            alpha += GameTime.deltaTime / ActivateTime;

            var color = onrenderer.color;
            color.a = alpha;
            onrenderer.color = color;


            color = offrenderer.color;
            color.a = 1 - alpha;
            offrenderer.color = color;


            yield return null;
        }


        on.SetActive(true);
        off.SetActive(false);
    }

}
