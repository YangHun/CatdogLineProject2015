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

    public void OnMenuButton1Click()
    {

    }

    public void OnMenuButton2Click()
    {

    }

    public void OnMenuButton3Click()
    {

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
