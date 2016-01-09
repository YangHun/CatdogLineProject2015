using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : SingleTonBehaviour<GameUIManager>
{

    public GameObject InGameCanvas = null;
    public GameObject GameMenuCanvas = null;

    public GameObject HealButton = null;
    public GameObject PauseButton = null;
    public GameObject MenuButton1 = null;
    public GameObject MenuButton2 = null;
    public GameObject MenuButton3 = null;
    public Image HealTypeUI = null;

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

    public void OnContinueButtonClick()
    {
		Debug.Log ("Continue Button Pressed");
		GameSceneController.Inst().ChangeToInGame();
    }

    public void OnSoundOptionButtonClick()
    {
		Debug.Log ("Sound Option Button Pressed");
		//GameSceneController.Inst().
    }

    public void OnMenuButton3Click()
    {

    }

    public void OnPlayerHealTypeChange(HealType type)
    {
        if (HealTypeUI == null)
            return;
        switch(type)
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

    public void OnGameMenu()
    {
        ActiveCanvas(false, true);
    }

    public void OnIngame()
    {
        ActiveCanvas(true, false);
    }

    private void ActiveCanvas(bool ingame, bool gamemenu)
    {
        if (InGameCanvas != null)
            InGameCanvas.SetActive(ingame);

        if (GameMenuCanvas != null)
            GameMenuCanvas.SetActive(gamemenu);
    }
}
