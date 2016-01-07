using UnityEngine;
using System.Collections;

public class SceneManager : SingleTonBehaviour<SceneManager> {

    [System.Serializable]
    private enum SceneState
    {
        MainScene,
        GameScene,
        Exit
    }
    [SerializeField]
    private SceneState InitialState = SceneState.MainScene;

    private StateMachine m_StateMachine = null;

    [SerializeField]
    public string MainSceneName = null;

    [SerializeField]
    public string[] GameSceneName = null;


    // MonoBehaviour Framework

    void Awake()
    {
        m_StateMachine = new StateMachine();
        m_StateMachine.AddState(SceneState.MainScene, () =>
        {
            if (m_StateMachine.IsFirstUpdate())
            {
                Debug.Log("SceneManager: state : " + m_StateMachine.GetCurrentState());
                MainSceneController.Inst().OnInitController();
            }
        });
        m_StateMachine.AddState(SceneState.GameScene, () =>
        {
            if (m_StateMachine.IsFirstUpdate())
            {
                Debug.Log("SceneManager: state : " + m_StateMachine.GetCurrentState());
                GameSceneController.Inst().OnInitController();
            }
        });
        m_StateMachine.AddState(SceneState.Exit, () =>
        {
            if (m_StateMachine.IsFirstUpdate())
                Debug.Log("SceneManager: state : " + m_StateMachine.GetCurrentState());
        });

        m_StateMachine.SetInitialState(InitialState);
    }

    void Start () { SetStatic(); }
    void Update() { m_StateMachine.Update(); }
    void LateUpdate() { m_StateMachine.LateUpdate(); }

    // State Info

    public SceneController GetCurrentSceneController()
    {
        switch ((SceneState)m_StateMachine.GetCurrentState())
        {
            case SceneState.MainScene:
                return MainSceneController.Inst();
            case SceneState.GameScene:
                return GameSceneController.Inst();

            default:
                return null;
        }
    }

    private void EndCurrentScene()
    {
        switch ((SceneState)m_StateMachine.GetCurrentState())
        {
            case SceneState.MainScene:
                MainSceneController.Inst().OnDestroyController();
                break;
            case SceneState.GameScene:
                GameSceneController.Inst().OnDestroyController();
                break;
        }
    }


    // events from others

    public void LoadMainScene()
    {
        if (m_StateMachine.GetCurrentState().Equals(SceneState.Exit))
            return;

        EndCurrentScene();
        m_StateMachine.ChangeState(SceneState.MainScene);
        Application.LoadLevel(MainSceneName);
    }

    public void LoadGameScene(int level = 1)
    {
        if (m_StateMachine.GetCurrentState() .Equals(SceneState.Exit))
            return;
        if(GameSceneName.Length < level - 1 || level - 1 < 0)
        {
            Debug.LogError("Level " + (level - 1) + "Is not assigned");
            return;
        }

        EndCurrentScene();
        m_StateMachine.ChangeState(SceneState.GameScene);
        Application.LoadLevel(GameSceneName[level - 1]);
    }

    public void ExitApp()
    {
        if (!m_StateMachine.GetCurrentState().Equals(SceneState.MainScene))
            return;

        EndCurrentScene();
        m_StateMachine.ChangeState(SceneState.Exit);
        Application.Quit();
    }
}
