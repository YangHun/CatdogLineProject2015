using UnityEngine;
using System.Collections;
using System;

public class MainSceneController : SingleTonBehaviour<MainSceneController>, SceneController {
    private bool m_IsEnabled = false;
    public bool IsEnabled() { return m_IsEnabled; }

    public void OnDestroyController() { m_IsEnabled = false; }
    public void OnInitController() { m_IsEnabled = true; }

    void Update()
    {
        Application.Quit();
    }
}
