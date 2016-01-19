using UnityEngine;
using System.Collections;

public class InputManager : SingleTonBehaviour<InputManager>
{

    [SerializeField]
    private bool LeftClicked;
    [SerializeField]
    private bool RightClicked;
    [SerializeField]
    private bool JumpClicked;

    void Awake()
    {
        LeftClicked = false;
        RightClicked = false;
        JumpClicked = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            LeftClicked = true;
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            LeftClicked = false;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            RightClicked = true;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            RightClicked = false;
        if (Input.GetKeyDown(KeyCode.Space))
            JumpClicked = true;
        if (Input.GetKeyUp(KeyCode.Space))
            JumpClicked = false;
    }


    public void OnHealButtonClick()
    {
        Debug.Log("Heal Button Pressed");
        GameManager.Inst().HealArea();
    }

    public void OnPauseButtonClick()
    {
        Debug.Log("Pause Button Pressed");
        GameSceneController.Inst().ChangeToGameMenu();
    }

    public void OnInteractButtonClick()
    {
        Debug.Log("Interact Button Clicked");
        if (GameManager.Inst().IsInteracting())
            GameManager.Inst().StopInteraction();
        else
            GameManager.Inst().StartInteraction();

    }

    public void OnContinueButtonClick()
    {
        Debug.Log("Continue Button Pressed");
        GameSceneController.Inst().ChangeToInGame();
    }

    public void OnSoundOptionButtonClick()
    {
        Debug.Log("Sound Option Button Pressed");
    }

    public void OnLeftClicked() { LeftClicked = true; }
    public void OnLeftReleased() { LeftClicked = false; }
    public bool IsLeftClicked()
    {
        if (GameSceneController.Inst().IsInGame() && !GameSceneController.Inst().IsCinematic())
            return LeftClicked;
        return false;
    }


    public void OnRightClicked() { RightClicked = true; }
    public void OnRightReleased() { RightClicked = false; }
    public bool IsRightClicked()
    {
        if (GameSceneController.Inst().IsInGame() && !GameSceneController.Inst().IsCinematic())
            return RightClicked;
        return false;
    }


    public void OnJumpClicked() { JumpClicked = true; }
    public void OnJumpReleased() { JumpClicked = false; }
    public bool IsJumpClicked()
    {
        if (GameSceneController.Inst().IsInGame() && !GameSceneController.Inst().IsCinematic())
            return JumpClicked;
        return false;
    }
}
