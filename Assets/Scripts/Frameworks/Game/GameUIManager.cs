using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : SingleTonBehaviour<GameUIManager>
{

    public GameObject InGameCanvas = null;
    public GameObject GameMenuCanvas = null;




    // InGame UIs
    public Image HealTypeUI = null;


    public GameObject MoveLeft = null;
    public GameObject MoveRight = null;
    public GameObject MoveJump = null;
    public GameObject HealButton = null;
    public GameObject InteractButton = null;



    void LateUpdate()
    {
        var scene = GameSceneController.Inst();
        if (scene.IsInGame() && !scene.IsCinematic())
            if (InteractButton != null)
                InteractButton.SetActive(GameManager.Inst().IsInteractinButtonEnabled());
    }



    public void OnPlayerHealTypeChange(HealType type)
    {
        if (HealTypeUI == null)
            return;
        switch (type)
        {
            case HealType.BLUE:
                HealTypeUI.color = new Color(0, 0, 255);
                break;
            case HealType.GREEN:
                HealTypeUI.color = new Color(0, 255, 0);
                break;
            case HealType.RED:
                HealTypeUI.color = new Color(255, 0, 0);
                break;
            case HealType.YELLOW:
                HealTypeUI.color = new Color(125, 125, 0);
                break;
        }
    }

    public void EnableInteractButton()
    {
        if (InteractButton == null)
            return;

        InteractButton.SetActive(true);
    }

    public void DisableInteractButton()
    {
        if (InteractButton == null)
            return;

        InteractButton.SetActive(false);
    }

    public void OnGameMenu()
    {
        ActiveCanvas(false, true);
    }

    public void OnIngame()
    {
        ActiveCanvas(true, false);
    }

    public void OnCinematic()
    {
        ActiveCanvas(true, false, true);
    }

    public void OnDisabled()
    {
        ActiveCanvas(false, false);
    }

    private void ActiveCanvas(bool ingame, bool gamemenu) { ActiveCanvas(ingame, gamemenu, false); }
    private void ActiveCanvas(bool ingame, bool gamemenu, bool cinematic)
    {
        if (GameMenuCanvas != null)
        {
            GameMenuCanvas.SetActive(gamemenu);
        }
        if (InGameCanvas != null)
        {
            InGameCanvas.SetActive(ingame);
        }

        if (cinematic && ingame)
        {
            if (MoveLeft != null)
            {
                InputManager.Inst().OnLeftReleased();
                MoveLeft.SetActive(false);
            }
            if (MoveRight != null)
            {
                InputManager.Inst().OnRightReleased();
                MoveRight.SetActive(false);
            }
            if (MoveJump != null)
            {
                InputManager.Inst().OnJumpReleased();
                MoveJump.SetActive(false);
            }
            if (HealButton != null)
                HealButton.SetActive(false);
            if (InteractButton != null)
                InteractButton.SetActive(false);
        }
        else
        {
            if (MoveLeft != null)
                MoveLeft.SetActive(true);
            if (MoveRight != null)
                MoveRight.SetActive(true);
            if (MoveJump != null)
                MoveJump.SetActive(true);
            if (HealButton != null)
                HealButton.SetActive(true);
            if (InteractButton != null)
                InteractButton.SetActive(true);
        }
    }

}
