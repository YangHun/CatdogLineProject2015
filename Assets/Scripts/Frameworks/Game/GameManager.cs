using UnityEngine;
using System.Collections;

public class GameManager : SingleTonBehaviour<GameManager>
{
    // Player

    [SerializeField]
    private GameObject Player = null;

    // Heal Info

    [SerializeField]
    private float m_HealRange = 1.0f;


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
        m_InteractStateMachine.Update();
        if (GameSceneController.Inst().IsInGame() 
            && (InteractState)m_InteractStateMachine.GetCurrentState() == InteractState.NONE)
            CheckInteractor();
    }

    void LateUpdate()
    {
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
        return Player;
    }

    // Events

    public void HealArea()
    {
        HealArea(Player.transform.position, m_HealRange);
    }

    public void HealArea(Vector2 position, float radius)
    {

    }

    public void SetPlayerHealType(HealType healtype)
    {

    }

    public void StopInteraction()
    {
        m_InteractStateMachine.ChangeState(InteractState.END);
    }

    public void OnPlayerHPChange()
    {

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
}
