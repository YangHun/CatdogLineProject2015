using UnityEngine;
using System.Collections;

public class InteractionManager : SingleTonBehaviour<InteractionManager> {



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
        foreach (var tmp_interactor in m_Interactors)
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


    // Events





    /// <summary>
    /// Framework Based. Start interaction
    /// </summary>
    public void StartInteraction()
    {
        if (!m_InteractStateMachine.GetCurrentState().Equals(InteractState.NONE))
            return;

        var interactor = CheckInteractor();
        if (interactor == null)
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
    /// Is player interacting
    /// </summary>
    /// <returns> interacting with others </returns>
    public bool IsInteracting()
    {
        return !m_InteractStateMachine.GetCurrentState().Equals(InteractState.NONE);
    }
}
