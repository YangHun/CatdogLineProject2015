using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BlueSam : ObjectData, IInteractor {
	
	private HealType Type = HealType.BLUE;
	public GameObject OperateButton;
    public bool m_IsPlayerInRange = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            m_IsPlayerInRange = true;
            Debug.Log("Sam : Player In Range");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            m_IsPlayerInRange = false;
            Debug.Log("Sam : Player Out of Range");
        }
    }

    public void OnInteractStart()
    {
        GameManager.Inst().SetPlayerHealType(Type);
        GameManager.Inst().StopInteraction();
    }

    public void OnInteractStay()
    {
        // do nothing
    }

    public void OnInteractEnd()
    {
        // do nothing
    }

    public bool IsInteractable()
    {
        return m_IsPlayerInRange;
    }
}
