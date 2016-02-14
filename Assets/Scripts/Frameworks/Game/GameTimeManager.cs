using UnityEngine;
using System.Collections;

public class GameTime
{
    public static float deltaTime
    {
        get { return GameTimeManager.Inst().GetInGameDeltaTime(); }
    }

    public static IEnumerator WaitOnInGame(float time)
    {
        float remain = time;

        while (remain > 0)
        {
            remain -= deltaTime;

            yield return null;
        }
    }
}


public class GameTimeManager : SingleTonBehaviour<GameTimeManager>
{

    private bool m_IsTimeUpdated = false;
    private float m_InGameDeltaTime = 0.0f;

    void Start() { Inst(); }

    void LateUpdate() { m_IsTimeUpdated = false; }
    public float GetInGameDeltaTime()
    {
        if (m_IsTimeUpdated)
            return m_InGameDeltaTime;

        if (GameSceneController.Inst().IsInGame())
            m_InGameDeltaTime = Time.deltaTime;
        else
            m_InGameDeltaTime = 0.0f;

        m_IsTimeUpdated = true;
        return m_InGameDeltaTime;
    }


}
