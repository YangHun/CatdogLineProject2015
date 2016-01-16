using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour {

    public PlayerController Player;
    public Image HPbar;
	void Update ()
    {
        HPbar.rectTransform.localScale = new Vector3(Player.GetHP() / Player.GetMaxHP(), 1, 1);
	}
}