using UnityEngine;
using System.Collections;

public interface IInteractor
{
    Vector2 GetPosition();
    void OnInteractStart();
    void OnInteractStay();
    void OnInteractEnd();
    bool IsInteractable();
}
