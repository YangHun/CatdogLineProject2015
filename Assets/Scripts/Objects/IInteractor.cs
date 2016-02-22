using UnityEngine;
using System.Collections;

public enum InteractorType
{
	DEFAULT,
	QUEST
}

public interface IInteractor
{
    Vector2 GetPosition();
    void OnInteractStart();
    void OnInteractStay();
    void OnInteractEnd();
    bool IsInteractable();
}
