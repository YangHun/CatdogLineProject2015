using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : SingleTonBehaviour<GameUIManager>
{

    public GameObject InGameCanvas = null;
    public GameObject GameMenuCanvas = null;
    

    // InGame UIs
    public Image HealTypeUI = null;
    public Image PlayerHPBar = null;



    public GameObject MoveLeft = null;
    public GameObject MoveRight = null;
    public GameObject MoveJump = null;
    public GameObject HealButton = null;
    public GameObject InteractButton = null;

    private Section CurrentSection = null;
    public UnitData[] Deadable;
    public Image[] WarningUI;
    [SerializeField]
    private Camera MainCamera;
    [SerializeField]
    private Image PrefabWarning;

    void Update()
    {
        Warning();
    }

    void LateUpdate()
    {
        var scene = GameSceneController.Inst();
        if (scene.IsInGame() && !scene.IsCinematic())
            if (InteractButton != null)
                InteractButton.SetActive(InteractionManager.Inst().IsInteractinButtonEnabled());

        var player = PlayerManager.Inst().GetPlayer().GetComponent<UnitData>();
        if(PlayerHPBar != null && player != null)
            PlayerHPBar.rectTransform.localScale = new Vector3(player.GetHP() / player.GetMaxHP(), 1, 1);
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

    void Warning()
    {
        /*
        Section tmp = SectionManager.Inst().GetCurrentSection();
        //if (CurrentSection != tmp)
        //{
        CurrentSection = tmp;
        if (CurrentSection == null)
            return;
        Deadable = CurrentSection.transform.Find("Objects").GetComponentsInChildren<UnitData>();
        if (Deadable == null)
            return;
        if (WarningUI.Length != Deadable.Length)
        {
            for (int i = 0; i < WarningUI.Length; ++i)
            {
                Destroy(WarningUI[i].gameObject);
            }
            WarningUI = new Image[Deadable.Length];
            for (int i = 0; i < Deadable.Length; ++i)
            {
                WarningUI[i] = (Image)Instantiate(PrefabWarning);
                WarningUI[i].transform.parent = InGameCanvas.transform.Find("WarningUI").transform;
            }
        }
        //}
        if (CurrentSection == null)
            return;
        if (WarningUI == null)
            return;
        for (int i = 0; i < WarningUI.Length; ++i)
        {
            if (Deadable[i] == null || Deadable[i].IsHealthy())
            {
                WarningUI[i].gameObject.SetActive(false);
                continue;
            }
            Vector3 b = Deadable[i].transform.position - MainCamera.transform.position;
            Vector3 a = MainCamera.WorldToScreenPoint(b);
            if (Deadable[i].GetComponentInChildren<SpriteRenderer>().isVisible)
            {
                WarningUI[i].gameObject.SetActive(false);
            }
            else
            {
                WarningUI[i].gameObject.SetActive(true);
                if (Mathf.Abs(b.y / b.x) >= MainCamera.pixelHeight / (float) MainCamera.pixelWidth)
                {
                    if (b.y > 0)
                        WarningUI[i].rectTransform.anchoredPosition = new Vector2(b.x / b.y * (MainCamera.pixelHeight / 2 - 5), MainCamera.pixelHeight / 2 - 5);
                    else
                        WarningUI[i].rectTransform.anchoredPosition = new Vector2(b.x / b.y * (-MainCamera.pixelHeight / 2 - 5), -MainCamera.pixelHeight / 2 + 5);
                }
                else
                {
                    if (b.x > 0)
                        WarningUI[i].rectTransform.anchoredPosition = new Vector2(MainCamera.pixelWidth / 2 - 5, b.y / b.x * (MainCamera.pixelWidth / 2 - 5));
                    else
                        WarningUI[i].rectTransform.anchoredPosition = new Vector2(-MainCamera.pixelWidth / 2 + 5, b.y / b.x * (-MainCamera.pixelWidth / 2 - 5));
                }
            }
        }
        */
    }
}
