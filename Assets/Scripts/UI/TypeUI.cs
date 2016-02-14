using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypeUI : MonoBehaviour {

    public Image A;
	void Update () {
        if (PlayerManager.Inst().GetPlayerHealType() == HealType.BLUE)
        {
            A.color = new Color(0, 0, 255);
        }
        if (PlayerManager.Inst().GetPlayerHealType() == HealType.GREEN)
        {
            A.color = new Color(0, 255, 0);
        }
	}
}
