using UnityEngine;
using System.Collections;

public class UnitManager : SingleTonBehaviour<UnitManager>
{
    /// <summary>
    /// Call this to kill unit
    /// </summary>
    /// <param name="unit"></param>
    public void OnDie(UnitData unit)
    {
        Debug.Log("Unit died : " + unit.name);
        if (unit.gameObject == PlayerManager.Inst().GetPlayer())
        {
            GameSceneController.Inst().OnEndGame(false);
        }
        else if (SectionManager.Inst().GetCurrentSection() != null)
        {
            // send destory message to section
            SectionManager.Inst().GetCurrentSection().OnDie(unit);
		}
        else
        {
            // unit died on no section enabled
            unit.OnDestroyObject();
			PlayerManager.Inst().GiveGuilty(10);
        }
    }


    /// <summary>
    /// Calling this will heal area
    /// </summary>
    /// <param name="position"> center </param>
    /// <param name="radius"> radius </param>
    public void HealArea(Vector2 position, float radius)
    {
        Debug.Log("Healing position : " + position + " radius : " + radius);

        // create healinfo
        HealInfo info = new HealInfo();
        info.type = PlayerManager.Inst().GetPlayerHealType();

        // find objects
        var objects = Physics2D.OverlapCircleAll(position, radius);
        foreach (var obj in objects)
        {
            var healables = obj.GetComponents<IHealable>();
            foreach (var healable in healables)
            {
                if (healable.IsHealable())
                {
                    if (SectionManager.Inst().GetCurrentSection() != null)
                    {
                        // send heal message to section
                        SectionManager.Inst().GetCurrentSection().OnHealed(healable, info);
                    }
                    else
                    {
                        // heal on no section enabled
                        healable.OnHealed(info);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Call this to attack unit
    /// </summary>
    /// <param name="info"></param>
    public void Attack(AttackInfo info)
    {
        Debug.Log("Dealing Unit : " + info.target.name + " amount : " + info.amount);

        // do nothing
        if (info.target == null)
            return;

        if (SectionManager.Inst().GetCurrentSection() != null)
        {
            // send attack message to section
            SectionManager.Inst().GetCurrentSection().OnAttacked(info.target, info);
        }
        else
        {
            // attack on no section enabled
            info.target.GiveDamage(info.amount);
        }
    }
}
