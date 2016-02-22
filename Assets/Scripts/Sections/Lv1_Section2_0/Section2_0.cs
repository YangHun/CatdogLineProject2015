using UnityEngine;
using System.Collections;

public class Section2_0 : Section
{
    [SerializeField]
    private Collider2D Block;

    public GameObject[] StartActivateList = null;

    public UnitData m_Flower = null;

    public override void OnSectionStart()
    {
        foreach (var obj in StartActivateList)
            obj.SetActive(true);


        IInteractor[] tmp = GetComponentsInChildren<IInteractor>();
        InteractionManager.Inst().RefreshInteractorList(tmp);
        Block.gameObject.SetActive(true);
    }

    public override void OnSectionPause()
    {

    }

    public override void OnSectionResume()
    {

    }

    public override void OnSectionEnd()
    {
        Block.gameObject.SetActive(true);
    }

    public override void OnDie(UnitData unit)
    {
        base.OnDie(unit);
        if (unit == m_Flower)
            Block.gameObject.SetActive(false);
    }

    public override void OnHealed(IHealable target, HealInfo info)
    {
        base.OnHealed(target, info);
        if (target.Equals(m_Flower))
            Block.gameObject.SetActive(false);

    }

    public override float GetSectionScore()
    {
        return 0;
    }
}
