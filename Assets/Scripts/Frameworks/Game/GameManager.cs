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

    private IInteractor[] m_Interactors = null;

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
    private bool m_IsInteractButtonEnabled = false;



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

    void Start()
    {
        SetPlayerHealType(m_HealType);
    }

    void Update()
    {
        if (!GameSceneController.Inst().IsInGame())
            return;

        m_InteractStateMachine.Update();
        if (m_InteractStateMachine.GetCurrentState().Equals(InteractState.NONE))
        {
            if (CheckInteractor() != null)
            {
                m_IsInteractButtonEnabled = true;
            }
            else
            {
                m_IsInteractButtonEnabled = false;
            }
        }
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

    IInteractor CheckInteractor()
    {
        if (m_Interactors == null)
            return null;

        // find interactable object
        foreach(var tmp_interactor in m_Interactors)
        {
            if (tmp_interactor.IsInteractable())
                return tmp_interactor;
        }

        // if no objects are interactable
        return null;
    }

    public bool IsInteractinButtonEnabled()
    {
        return m_IsInteractButtonEnabled;
    }

    /// <summary>
    /// Set new IInteractor list
    /// </summary>
    /// <param name="list"></param>
    public void RefreshInteractorList(IInteractor[] list)
    {
        m_Interactors = list;
    }

    // Game info

    /// <summary>
    /// Get player
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return m_Player;
    }

    // Events

    /// <summary>
    /// Calling this will heal area near player
    /// </summary>
    public void HealArea()
    {
        HealArea(m_Player.transform.position, m_HealRange);
    }

    /// <summary>
    /// Calling this will heal area
    /// </summary>
    /// <param name="position"> center </param>
    /// <param name="radius"> radius </param>
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

    /// <summary>
    /// Change HealType of player
    /// </summary>
    /// <param name="healtype"> new healtype </param>
    public void SetPlayerHealType(HealType healtype)
    {
        Debug.Log("Player heal type change to : " + healtype.ToString());
        GameUIManager.Inst().OnPlayerHealTypeChange(healtype);
        m_HealType = healtype;
    }

    /// <summary>
    /// Framework Based. Start interaction
    /// </summary>
    public void StartInteraction()
    {
        if (!m_InteractStateMachine.GetCurrentState().Equals(InteractState.NONE))
            return;

        var interactor = CheckInteractor();
        if(interactor == null)
            return;

        Debug.Log("Start Interaction");
        m_InteractStateMachine.ChangeState(InteractState.START);
        m_Interactor = interactor;
    }

    /// <summary>
    /// Stop Current Interaction
    /// </summary>
    public void StopInteraction()
    {
        if (!m_InteractStateMachine.GetCurrentState().Equals(InteractState.STAY)
            && !m_InteractStateMachine.GetCurrentState().Equals(InteractState.START))
            return;

        m_InteractStateMachine.ChangeState(InteractState.END);
    }

    /// <summary>
    /// Make player climb.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="direction"></param>
    public void StartPlayerClimbing(Vector2 start, Vector2 direction)
    {
        direction.Normalize();
        GetPlayer().GetComponent<PlayerController>().StartClimb(start, direction);
    }

    /// <summary>
    /// Stop player climb.
    /// </summary>
    public void EndPlayerClimbing()
    {
        GetPlayer().GetComponent<PlayerController>().EndClimb();
    }

    /// <summary>
    /// Is player interacting
    /// </summary>
    /// <returns> interacting with others </returns>
    public bool IsInteracting()
    {
        return !m_InteractStateMachine.GetCurrentState().Equals(InteractState.NONE);
    }

    /// <summary>
    /// FrameworkBased. Change UI
    /// </summary>
    public void OnPlayerHPChange()
    {
        // change UI
    }

    public void StartCinematic(float time)
    {
        GameSceneController.Inst().ChangeToCinematic(time);
    }


    public void OnDie(UnitData unit)
    {
        Debug.Log("Unit died : " + unit.name);
        if (unit.gameObject == GetPlayer())
        {
            OnEndGame(false);
        }
        else if (SectionManager.Inst().GetCurrentSection() != null)
        {
            SectionManager.Inst().GetCurrentSection().OnDie(unit);
        }
        else
        {
            // unit died on no section enabled

        }
    }
    

    /// <summary>
    /// FrameworkBased.
    /// </summary>
    /// <param name="cleared"></param>
    public void OnEndGame(bool cleared)
    {
        Debug.Log("Ending Game with cleared : " + cleared);

        // Save data
        SaveData();

        // End GameScene
        GameSceneController.Inst().EndScene();
    }

    /// <summary>
    /// FrameworkBased.
    /// </summary>
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

    public float GameTime()
    {
        if (GameSceneController.Inst().IsInGame())
            return Time.deltaTime;
        return 0;
    }
}
