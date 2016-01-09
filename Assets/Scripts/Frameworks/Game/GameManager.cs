using UnityEngine;
using System.Collections;

public class GameManager : SingleTonBehaviour<GameManager>
{
    // Player

    [SerializeField]
    private GameObject m_Player = null;
    

    // Heal Info

    [SerializeField]
    private float m_HealRange = 1.0f;
    [SerializeField]
    private HealType m_HealType = HealType.BLUE;
    [SerializeField]
    private float m_HealCoolDown = 1.0f;
    [SerializeField]
    private float m_HealCoolDownLeft = 0.0f;


    // Interactor Info

    [SerializeField]
    private IInteractor m_Interactor = null;
    enum InteractState
    {
        NONE,
        START,
        STAY,
        END
    }
    private StateMachine m_InteractStateMachine = null;

    void Awake()
    {
        m_InteractStateMachine = new StateMachine();
        m_InteractStateMachine.AddState(InteractState.NONE, () => { });
        m_InteractStateMachine.AddState(InteractState.START, () =>
        {
            SectionManager.Inst().GetCurrentSection().OnInteractStart(m_Interactor);
            m_InteractStateMachine.ChangeState(InteractState.STAY);
        });
        m_InteractStateMachine.AddState(InteractState.STAY, () =>
        {
            SectionManager.Inst().GetCurrentSection().OnInteractStay(m_Interactor);
        });
        m_InteractStateMachine.AddState(InteractState.END, () =>
        {
            SectionManager.Inst().GetCurrentSection().OnInteractEnd(m_Interactor);
            m_InteractStateMachine.ChangeState(InteractState.NONE);
        });
        m_InteractStateMachine.SetInitialState(InteractState.NONE);
    }

    void Update()
    {
        if (!GameSceneController.Inst().IsInGame())
            return;

        m_InteractStateMachine.Update();
        if (m_InteractStateMachine.GetCurrentState().Equals(InteractState.NONE))
            CheckInteractor();
        UpdateHealCoolDown();
    }

    void UpdateHealCoolDown()
    {
        m_HealCoolDownLeft -= Time.deltaTime;
        if (m_HealCoolDownLeft < 0.0f)
            m_HealCoolDownLeft = 0.0f;
    }

    void LateUpdate()
    {
        if (!GameSceneController.Inst().IsInGame())
            return;

        m_InteractStateMachine.LateUpdate();
    }

    void CheckInteractor()
    {
        if (m_Interactor == null)
        {
            // ray cast user input

            // if user touch available IInteractor, interact
        }
    }

    // Game info

    public GameObject GetPlayer()
    {
        return m_Player;
    }

    // Events

    public void HealArea()
    {
        HealArea(m_Player.transform.position, m_HealRange);
    }

    public void HealArea(Vector2 position, float radius)
    {
        if (m_HealCoolDownLeft > 0.0f)
            return;

        Debug.Log("Healing position : " + position + " radius : " + radius);
        m_HealCoolDownLeft = m_HealCoolDown;

        // create healinfo
        HealInfo info = new HealInfo();
        info.type = m_HealType;

        // find objects
        var objects = Physics2D.OverlapCircleAll(position, radius);
        foreach (var obj in objects)
        {
            var healables = obj.GetComponents<IHealable>();
            foreach (var healable in healables)
            {
                if (healable.IsHealable())
                    SectionManager.Inst().GetCurrentSection().OnHealed(healable, info);
            }
        }

    }

    public void SetPlayerHealType(HealType healtype)
    {
        m_HealType = healtype;
    }

    public void StopInteraction()
    {
        if ((InteractState)m_InteractStateMachine.GetCurrentState() != InteractState.STAY
            && (InteractState)m_InteractStateMachine.GetCurrentState() != InteractState.START)
            return;

        m_InteractStateMachine.ChangeState(InteractState.END);
    }

    public void OnPlayerHPChange()
    {
        // change UI
    }

    public void OnEndGame(bool cleared)
    {
        // Save data
        SaveData();

        // End GameScene
        GameSceneController.Inst().EndScene();

    }

    public void OnEndSection()
    {
        // Auto save data
        SaveData();
    }


    // Data Saving

    void SaveData()
    {
        Debug.Log("Saving player data... (not implemented)");

        // CreateUserData

        // Save UserData

    }

    public HealType GetHealType()
    {
        return m_HealType;
    }
}
