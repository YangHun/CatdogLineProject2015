using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MainSceneController : SingleTonBehaviour<MainSceneController>, SceneController {
    private bool m_IsEnabled = false;


    public GameObject MainCanvas = null;
    public GameObject DeveloperCanvas = null;
    public GameObject SoundConfigCanvas = null;
    public GameObject QuitCanvas = null;

    public Slider SoundSlider = null;


    private enum MainSceneStates
    {
        TITLE,
        SOUND,
        LOADING,
        DEVLEOPER,
        EXITING
    };
    private StateMachine m_StateMachine = null;


    void Start()
    {
        m_StateMachine = new StateMachine();
        m_StateMachine.AddState(MainSceneStates.TITLE, OnStateTitle);
        m_StateMachine.AddState(MainSceneStates.SOUND, OnStateSound);
        m_StateMachine.AddState(MainSceneStates.LOADING, () => { });
        m_StateMachine.AddState(MainSceneStates.DEVLEOPER, OnStateDevelop);
        m_StateMachine.AddState(MainSceneStates.EXITING, OnStateExiting);
        m_StateMachine.SetInitialState(MainSceneStates.TITLE);
    }

    void Update() { m_StateMachine.Update(); }
    void LateUpdate() { m_StateMachine.LateUpdate(); }


    void OnStateTitle()
    {
        if(m_StateMachine.IsFirstUpdate())
        {
            SetCanvasActivation(true, false, false, false);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
            m_StateMachine.ChangeState(MainSceneStates.EXITING);
    }

    void OnStateDevelop()
    {
        if (m_StateMachine.IsFirstUpdate())
        {
            SetCanvasActivation(false, true, false, false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            m_StateMachine.ChangeState(MainSceneStates.TITLE);
    }

    void OnStateSound()
    {
        if (m_StateMachine.IsFirstUpdate())
        {
            SetCanvasActivation(false, false, true, false);
        }

        if(SoundSlider != null)
        {
            SoundManager.Inst().SetVolume(SoundSlider.value);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            m_StateMachine.ChangeState(MainSceneStates.TITLE);

    }

    void OnStateExiting()
    {
        if (m_StateMachine.IsFirstUpdate())
        {
            SetCanvasActivation(false, false, false, true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            m_StateMachine.ChangeState(MainSceneStates.TITLE);
    }
    

    public void OnButtonClickedTitle(GameObject button)
    {
        if (!m_StateMachine.IsCurrentState(MainSceneStates.TITLE))
            return;

        if (Debug.isDebugBuild)
            Debug.Log("Button Clicked : " + button.name);

        switch (button.name)
        {
            case "StartNewButton":
                {
                    m_StateMachine.ChangeState(MainSceneStates.LOADING);
                    SceneManager.Inst().LoadGameScene();
                    break;
                }
            case "ContinueButton":
                {
                    m_StateMachine.ChangeState(MainSceneStates.LOADING);
                    SceneManager.Inst().LoadGameScene();
                    break;
                }

            case "DeveloperButton":
                {
                    m_StateMachine.ChangeState(MainSceneStates.DEVLEOPER);

                    break;
                }
            case "SoundConfigButton":
                {
                    m_StateMachine.ChangeState(MainSceneStates.SOUND);

                    break;
                }
            default:
                {
                    Debug.LogError("Invalid Button Input");

                    break;
                }
        }
    }

    public void OnButtonClickedExiting(GameObject button)
    {
        if (!m_StateMachine.IsCurrentState(MainSceneStates.EXITING))
            return;

        if (Debug.isDebugBuild)
            Debug.Log("Button Clicked : " + button.name);

        switch(button.name)
        {
            case "Confirm":
                {
                    SceneManager.Inst().ExitApp();
                    break;
                }
            case "Back":
                {
                    m_StateMachine.ChangeState(MainSceneStates.TITLE);
                    break;
                }
            default:
                {
                    Debug.LogError("Invalid Button Input");

                    break;
                }
        }
    }



    void SetCanvasActivation(bool main, bool develop, bool sound, bool quit)
    {
        if (MainCanvas != null)
            MainCanvas.SetActive(main);
        if (DeveloperCanvas != null)
            DeveloperCanvas.SetActive(develop);
        if (SoundConfigCanvas != null)
            SoundConfigCanvas.SetActive(sound);
        if (QuitCanvas != null)
            QuitCanvas.SetActive(quit);
    }

    public bool IsEnabled() { return m_IsEnabled; }

    public void OnDestroyController() { m_IsEnabled = false; }
    public void OnInitController() { m_IsEnabled = true; }
}
