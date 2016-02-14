using UnityEngine;
using System.Collections;

public class ObjectJungSoo : UnitData, IHealable {

    [SerializeField]
    private HealType m_Type;

    public void OnHealed(HealInfo heal)
    {
        GiveHeal(100f);
        StartCoroutine(FadeOut());
    }

    public bool IsHealable()
    {
        if (PlayerManager.Inst().GetPlayerHealType() == m_Type)
            return true;
        else
            return false;
    }

    IEnumerator FadeOut()
    {
        Color c = new Color();
        c = this.GetComponent<SpriteRenderer>().color;
        for (float f = 1; f >= 0f; f -= 0.01f)
        {
            c.a = f;
            this.GetComponent<SpriteRenderer>().color = c;
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }
}