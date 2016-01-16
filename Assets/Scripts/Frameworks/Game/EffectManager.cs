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
}

public class EffectManager : SingleTonBehaviour<EffectManager> {
    


    public void CreateEffect(EffectInfo info)
    {
        switch(info.name)
        {

            case "":
                break;
            default:
                break;
        }
    }

}
