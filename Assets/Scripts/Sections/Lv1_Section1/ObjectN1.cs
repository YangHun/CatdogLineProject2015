using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ObjectN1 : ObjectData, IInteractor
{
    private bool m_IsPlayerInRange = false;

    public Transform Talk1 = null;

    void Start()
    {
        Talk1.transform.position = new Vector2(1000f, 1000f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            m_IsPlayerInRange = true;
            Debug.Log("N1 : Player In Range");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            m_IsPlayerInRange = false;
            Talk1.transform.position = new Vector2(1000f, 1000f);
            Debug.Log("N1 : Player Out of Range");
        }
    }

    public bool IsInteractable()
    {
        return m_IsPlayerInRange;
    }

    public void OnInteractEnd()
    {
        //do nothing
    }

    public void OnInteractStart()
    {
        //GameManager.Inst().SetPlayerHealType(m_Type);
        Talk1.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
    }

    public void OnInteractStay()
    {
        GameManager.Inst().StopInteraction();
    }
}
