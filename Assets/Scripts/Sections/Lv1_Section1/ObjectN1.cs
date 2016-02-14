using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ObjectN1 : ObjectData, IInteractor
{
    private bool mIsPlayerInRange = false;
    [SerializeField]
    private TriggerZone Trig = null;
    public Transform Talk1 = null;

    void Start()
    {
        if (Trig != null)
        {
            Trig.AddListener(TriggerType.ENTER, OnPlayerEnter);
            Trig.AddListener(TriggerType.EXIT, OnPlayerExit);
        }
        Talk1.transform.position = new Vector2(1000f, 1000f);
    }

    void OnPlayerEnter(GameObject zone, Collider2D col)
    {
        if (col.gameObject != PlayerManager.Inst().GetPlayer())
            return;

        mIsPlayerInRange = true;
    }

    void OnPlayerExit(GameObject zone, Collider2D col)
    {
        if (col.gameObject != PlayerManager.Inst().GetPlayer())
            return;

        mIsPlayerInRange = false;
        Talk1.transform.position = new Vector2(1000f, 1000f);
    }

    public bool IsInteractable()
    {
        return mIsPlayerInRange;
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
        InteractionManager.Inst().StopInteraction();
    }
}
