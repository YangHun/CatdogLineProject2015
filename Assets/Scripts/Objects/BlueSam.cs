using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlueSam : MonoBehaviour {
	
	private HealType Type = HealType.BLUE;
	public GameObject OperateButton;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            Debug.Log("I blue player!!!");
            OperateButton.SetActive(true);
            GameUIManager.Inst().TriggeredType = Type;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            Debug.Log("I deblue player!!!");
            OperateButton.SetActive(false);
        }
    }
}
