using UnityEngine;
using System.Collections;

public class ObjectVine : UnitData, IInteractor, IHealable {

    [SerializeField]
    private HealType m_Type = HealType.GREEN;

    [SerializeField]
    private TriggerZone mVineDown = null;
    [SerializeField]
    private TriggerZone mVineUp = null;
    
    private bool mIsPlayerAtDown = true;
    private bool mIsPlayerTargetDown = false;
    private bool mIsPlayerInRange = false;
    private bool mIsPlayerClimbing = false;

    void OnPlayerEnter(GameObject zone, Collider2D col)
    {
        if (col.gameObject != GameManager.Inst().GetPlayer())
            return;
        
        mIsPlayerInRange = true;

        if (zone == mVineDown.gameObject)
        {
            if(mIsPlayerClimbing && mIsPlayerTargetDown)
                GameManager.Inst().StopInteraction();

            mIsPlayerAtDown = true;
        }
        else
        {
            if (mIsPlayerClimbing && !mIsPlayerTargetDown)
                GameManager.Inst().StopInteraction();
            mIsPlayerAtDown = false;
        }
    }

    void OnPlayerExit(GameObject zone, Collider2D col)
    {
        mIsPlayerInRange = false;
    }


    void Start()
    {
        if (mVineDown != null)
        {
            mVineDown.AddListener(TriggerType.ENTER, OnPlayerEnter);
            mVineDown.AddListener(TriggerType.EXIT, OnPlayerExit);
        }

        if (mVineUp != null)
        {
            mVineUp.AddListener(TriggerType.ENTER, OnPlayerEnter);
            mVineUp.AddListener(TriggerType.EXIT, OnPlayerExit);
        }
    }
    

    public void OnInteractStart()
    {
        mIsPlayerClimbing = true;
        if (mIsPlayerAtDown)
        {
            GameManager.Inst().StartPlayerClimbing(mVineDown.transform.position, new Vector2(0, 1));
            mIsPlayerTargetDown = false;
        }
        else
        {
            GameManager.Inst().StartPlayerClimbing(mVineUp.transform.position, new Vector2(0, -1));
            mIsPlayerTargetDown = true;
        }
    }

    public void OnInteractStay()
    {
        // do nothing
    }

    public void OnInteractEnd()
    {
        mIsPlayerClimbing = false;
        GameManager.Inst().EndPlayerClimbing();
    }

    public bool IsInteractable()
    {
        if (IsFullHealth() && mIsPlayerInRange)
            return true;
        return false;
    }

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
    }

    public bool IsHealable()
    {
        return true;
    }
}
