using UnityEngine;
using System.Collections;
using System;

public class GameSceneController : SingleTonBehaviour<GameSceneController>, SceneController
{

    public enum GameState
    {
        Disabled,
        InGame,
        GameMenu
    };
    private StateMachine m_StateMachine = null;
    [SerializeField]
    private GameState m_InitialState = GameState.Disabled;

    void Awake()
    {
        m_StateMachine = new StateMachine();
        m_StateMachine.AddState(GameState.Disabled, () => { });
        m_StateMachine.AddState(GameState.InGame, () => { });
        m_StateMachine.AddState(GameState.GameMenu, () => { });
        m_StateMachine.SetInitialState(m_InitialState);
    }

    void Update() { m_StateMachine.Update(); }
    void LateUpdate() { m_StateMachine.LateUpdate(); }

    // Scene Info

    public bool IsInGame()
    {
        return m_StateMachine.GetCurrentState().Equals(GameState.InGame);
    }

    public bool IsGameMenu()
    {
        return m_StateMachine.GetCurrentState().Equals(GameState.GameMenu);
    }

    public bool IsEnabled()
    {
        return !m_StateMachine.GetCurrentState().Equals(GameState.Disabled);
    }

    // Events

    public void ChangeToDisabled()
    {
        m_StateMachine.ChangeState(GameState.Disabled);
        GameUIManager.Inst().OnDisabled();
    }

    public void ChangeToGameMenu()
    {
        m_StateMachine.ChangeState(GameState.GameMenu);
        GameUIManager.Inst().OnGameMenu();
    }

    public void ChangeToInGame()
    {
        m_StateMachine.ChangeState(GameState.InGame);
        GameUIManager.Inst().OnIngame();
    }

    public void OnInitController()
    {
        Debug.Log("Initialiing GameScene");
        m_StateMachine.ChangeState(GameState.InGame);

        ChangeToInGame();
    }

    public void OnDestroyController()
    {
        Debug.Log("Destroying GameScene");
        m_StateMachine.ChangeState(GameState.Disabled);
    }

    public void EndScene()
    {
        m_StateMachine.ChangeState(GameState.Disabled);
        SceneManager.Inst().LoadMainScene();
    }
}
