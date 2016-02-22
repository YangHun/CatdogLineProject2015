using UnityEngine;
using System.Collections;
using System;

public class GameSceneController : SingleTonBehaviour<GameSceneController>, SceneController
{

    public enum GameState
    {
        Disabled,
        InGame,
        GameMenu,
        Cinematic
    };
    private StateMachine m_StateMachine = null;
    [SerializeField]
    private GameState m_InitialState = GameState.Disabled;

    public AudioSource m_BGM = null;
    
    private bool m_CinematicEnabled = false;

    void Awake()
    {
        m_StateMachine = new StateMachine();
        m_StateMachine.AddState(GameState.Disabled, () => { if (!m_StateMachine.IsFirstUpdate()) Application.Quit(); });
        m_StateMachine.AddState(GameState.InGame, () => { });
        m_StateMachine.AddState(GameState.GameMenu, () => { });
        m_StateMachine.AddState(GameState.Cinematic, () => { });
        m_StateMachine.SetInitialState(m_InitialState);

        m_CinematicEnabled = false;
    }

    void Start()
    {
        if (m_BGM != null)
            SoundManager.Inst().SetBGM(m_BGM);
    }

    void Update() { m_StateMachine.Update(); }
    void LateUpdate() { m_StateMachine.LateUpdate(); }

    // Scene Info

    public bool IsInGame()
    {
        return m_StateMachine.GetCurrentState().Equals(GameState.InGame) 
            || m_StateMachine.GetCurrentState().Equals(GameState.Cinematic);
    }

    public bool IsGameMenu()
    {
        return m_StateMachine.GetCurrentState().Equals(GameState.GameMenu);
    }

    public bool IsEnabled()
    {
        return !m_StateMachine.GetCurrentState().Equals(GameState.Disabled);
    }

    public bool IsCinematic()
    {
        return m_StateMachine.GetCurrentState().Equals(GameState.Cinematic);
    }

    // Events
    

    public void ChangeToGameMenu()
    {
        m_StateMachine.ChangeState(GameState.GameMenu);
        GameUIManager.Inst().OnGameMenu();
    }

    public void ChangeToInGame()
    {
        if (m_CinematicEnabled)
        {
            m_StateMachine.ChangeState(GameState.Cinematic);
            GameUIManager.Inst().OnCinematic();
        }
        else
        {
            m_StateMachine.ChangeState(GameState.InGame);
            GameUIManager.Inst().OnIngame();
        }
    }

    public void ChangeToCinematic(float time)
    {
        StartCoroutine(OnCinematic(time));
    }

    IEnumerator OnCinematic(float time)
    {
        m_CinematicEnabled = true;
        m_StateMachine.ChangeState(GameState.Cinematic);
        GameUIManager.Inst().OnCinematic();

        yield return StartCoroutine(WaitOnCinematic(time));

        m_CinematicEnabled = false;
        m_StateMachine.ChangeState(GameState.InGame);
        GameUIManager.Inst().OnIngame();
    }

    public IEnumerator WaitOnCinematic(float time)
    {
        float remain = time;

        while (remain > 0)
        {
            if (m_StateMachine.GetCurrentState().Equals(GameState.Cinematic))
                remain -= Time.deltaTime;
            yield return null;
        }
    }
    
    public IEnumerator WaitOnInGame(float time)
    {
        float remain = time;

        while (remain > 0)
        {
            if (IsInGame())
                remain -= Time.deltaTime;

            yield return null;
        }
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



    public void StartCinematic(float time)
    {
        GameSceneController.Inst().ChangeToCinematic(time);
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
}
