using UnityEngine;
using System.Collections;

public enum TriggerType
{
    ENTER,
    STAY,
    EXIT
};

public delegate void TriggerEvent(GameObject zone, Collider2D col);

public class TriggerZone : MonoBehaviour
{

    private event TriggerEvent m_EnterEvent;
    private event TriggerEvent m_StayEvent;
    private event TriggerEvent m_ExitEvent;

    public void AddListener(TriggerType type, TriggerEvent callback)
    {
        switch (type)
        {
            case TriggerType.ENTER:
                m_EnterEvent += callback;
                break;

            case TriggerType.STAY:
                m_StayEvent += callback;
                break;

            case TriggerType.EXIT:
                m_ExitEvent += callback;
                break;
        }
    }

    public void RemoveListener(TriggerType type, TriggerEvent callback)
    {
        switch (type)
        {
            case TriggerType.ENTER:
                m_EnterEvent -= callback;
                break;

            case TriggerType.STAY:
                m_StayEvent -= callback;
                break;

            case TriggerType.EXIT:
                m_ExitEvent -= callback;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (m_EnterEvent != null)
            m_EnterEvent(gameObject, col);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (m_StayEvent != null)
            m_StayEvent(gameObject, col);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (m_ExitEvent != null)
            m_ExitEvent(gameObject, col);
    }

}