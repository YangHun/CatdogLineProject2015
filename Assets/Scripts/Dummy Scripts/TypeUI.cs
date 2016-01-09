using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeUI : MonoBehaviour {

    public Image A;
	void Update () {
        if (GameManager.Inst().GetHealType() == HealType.BLUE)
        {
            A.color = new Color(0, 0, 255);
        }
        if (GameManager.Inst().GetHealType() == HealType.GREEN)
        {
            A.color = new Color(0, 255, 0);
        }
	}
}
