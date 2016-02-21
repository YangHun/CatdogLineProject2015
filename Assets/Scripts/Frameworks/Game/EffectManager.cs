using UnityEngine;
using System.Collections;


public struct EffectInfo
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;

    /// <summary>
    /// create effect after [createtime]
    /// </summary>
    public float createtime;
    /// <summary>
    /// effect will exist in world for [existtime]
    /// </summary>
    public float existtime;


    public HealType type;
}

public class EffectManager : SingleTonBehaviour<EffectManager> {

    public GameObject m_HealingEffectDefault = null;
    public GameObject m_HealingEffectGreen = null;
    public GameObject m_HealingEffectBlue = null;


    public GameObject m_HeathyObject = null;

    public void CreateEffect(EffectInfo info)
    {
        switch(info.name)
        {
            case "HealByPlayer":
                {
                    GameObject effect = null;
                    switch(info.type)
                    {
                        case HealType.DEFAULT:
                            effect = GameObject.Instantiate(m_HealingEffectDefault, info.position, info.rotation) as GameObject;
                            break;
                        case HealType.BLUE:
                            effect = GameObject.Instantiate(m_HealingEffectBlue, info.position, info.rotation) as GameObject;
                            break;
                        case HealType.GREEN:
                            effect = GameObject.Instantiate(m_HealingEffectGreen, info.position, info.rotation) as GameObject;
                            break;
                    }
                    if(effect != null)
                        StartCoroutine(EffectGC(effect, info.existtime));
                    if (Debug.isDebugBuild)
                        Debug.Log("Player Heal effect create. color : " + info.type);
                }
                break;
            case "Healthy":
                {
                    GameObject effect = GameObject.Instantiate(m_HeathyObject, info.position, info.rotation) as GameObject;
                    StartCoroutine(EffectGC(effect, info.existtime));

                }
                break;
            default:
                Debug.Log("No effect exist : " + info.name);
                break;
        }
    }

    IEnumerator EffectGC(GameObject effect, float time)
    {

        while(time > 0)
        {
            time -= GameTime.deltaTime;
            yield return null;
        }

        Destroy(effect);

        yield return null;
    }

}
