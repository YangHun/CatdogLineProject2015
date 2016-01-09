using UnityEngine;
using System.Collections;

public class GreenSam : MonoBehaviour {

    private HealType Type = HealType.GREEN;
    public GameObject OperateButton;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            Debug.Log("I green player!!!");
            OperateButton.SetActive(true);
            GameManager.Inst().SetPlayerHealType(HealType.GREEN);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            Debug.Log("I degreen player!!!");
            OperateButton.SetActive(false);
        }
    }
}