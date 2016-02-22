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

    public float mCinematicTime = 3.0f;

    void OnPlayerEnter(GameObject zone, Collider2D col)
    {
        if (col.gameObject != PlayerManager.Inst().GetPlayer())
            return;
        
        mIsPlayerInRange = true;

        if (zone == mVineDown.gameObject)
        {
            if(mIsPlayerClimbing && mIsPlayerTargetDown)
                InteractionManager.Inst().StopInteraction();
                
            mIsPlayerAtDown = true;
        } else

            mIsPlayerAtDown = false;
    }

    void OnPlayerExit(GameObject zone, Collider2D col)
    {
        if (col.gameObject != PlayerManager.Inst().GetPlayer())
            return;

        mIsPlayerInRange = false;


        if (zone == mVineUp.gameObject)
        {
            if (mIsPlayerClimbing && !mIsPlayerTargetDown)
                InteractionManager.Inst().StopInteraction();
        }

    }


	public override void Start()
	{
		base.Start();
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
        GameSceneController.Inst().StartCinematic(mCinematicTime);
        if (mIsPlayerAtDown)
        {
            PlayerManager.Inst().StartPlayerClimbing(mVineDown.transform.position, mVineUp.transform.position);
            mIsPlayerTargetDown = false;
        }
        else
        {
            PlayerManager.Inst().StartPlayerClimbing(mVineUp.transform.position, mVineDown.transform.position);
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
        PlayerManager.Inst().EndPlayerClimbing();
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
