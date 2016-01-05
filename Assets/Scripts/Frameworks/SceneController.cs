using UnityEngine;
using System.Collections;

public interface SceneController
{
    bool IsEnabled();

    void OnInitController();
    void OnDestroyController();
}
