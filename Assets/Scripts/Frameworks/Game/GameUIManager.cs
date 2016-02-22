using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public delegate bool NeedsWarning(UnitData unit);

public class WarningManager
{
	GameObject sign;
	UnitData target;
	NeedsWarning checker;
	bool m_IsDestroyed = false;

	public WarningManager(GameObject sign, UnitData unit, NeedsWarning check)
	{
		this.sign = sign;
		target = unit;
		checker = check;
	}

	public bool NeedWarning()
	{
		if (checker == null)
			return false;
		return checker(target);
	}

	public void UpdatePosition()
	{
		sign.transform.position = target.transform.position;
	}

	public void Destroy()
	{
		GameObject.Destroy(sign);
		m_IsDestroyed = true;
	}

	public void SetEnable(bool enable)
	{
		sign.SetActive(enable);
	}

	public bool IsDestroyed()
	{
		return m_IsDestroyed;
	}

};

public class GameUIManager : SingleTonBehaviour<GameUIManager>
{

	public GameObject InGameCanvas = null;
	public GameObject GameMenuCanvas = null;


	// InGame UIs
	public Sprite TypeDefault = null;
	public Sprite TypeBlue = null;
	public Sprite TypeGreen = null;
	public Image HealTypeUI = null;
	public Image PlayerHPBar = null;
	public Image PlayerGuiltyBar = null;



	public GameObject MoveLeft = null;
	public GameObject MoveRight = null;
	public GameObject MoveJump = null;
	public GameObject HealButton = null;
	public GameObject InteractButton = null;

	public Section CurrentSection = null;
	public GameObject WarningUI = null;

    [SerializeField]
    private Image prefabHPbar = null;
    public Image[] HPbarUI;
    public UnitData[] Units;
    [SerializeField]
    private GameObject ObjectHPUI;


    void Update()
    {
        ObjectHP();
    }

	void LateUpdate()
	{
		var scene = GameSceneController.Inst();
		if (scene.IsInGame() && !scene.IsCinematic())
			if (InteractButton != null)
				InteractButton.SetActive(InteractionManager.Inst().IsInteractinButtonEnabled());

		var player = PlayerManager.Inst().GetPlayer().GetComponent<UnitData>();
		if (PlayerHPBar != null && player != null)
			PlayerHPBar.rectTransform.localScale = new Vector3(player.GetHP() / 100, 1, 1);
		if (PlayerGuiltyBar != null && player != null)
			PlayerGuiltyBar.rectTransform.localScale = new Vector3(1 - player.GetMaxHP() / 100, 1, 1);
	}



	public void OnPlayerHealTypeChange(HealType type)
	{
		if (HealTypeUI == null)
			return;
		switch (type)
		{
			case HealType.DEFAULT:
				HealTypeUI.sprite = TypeDefault;
				break;
			case HealType.BLUE:
				HealTypeUI.sprite = TypeBlue;
				break;
			case HealType.GREEN:
				HealTypeUI.sprite = TypeGreen;
				break;
			case HealType.RED:
				HealTypeUI.sprite = null;
				break;
			case HealType.YELLOW:
				HealTypeUI.sprite = null;
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



	public WarningManager CreateWarningManager(UnitData unit, NeedsWarning check = null)
	{
		GameObject warning = Instantiate(WarningUI) as GameObject;
		WarningManager manager = new WarningManager(warning, unit, check);

		StartCoroutine(ManageWarning(manager));

		return manager;
	}

	public void DestroyWarningManager(WarningManager manager)
	{
		manager.Destroy();
    }

	IEnumerator ManageWarning(WarningManager manager)
	{
		while (true)
		{
			if (manager.NeedWarning())
			{
				// destroy sign
				manager.UpdatePosition();
				manager.SetEnable(true);
			}
			else
				manager.SetEnable(false);


			if (manager.IsDestroyed())
				break;

			yield return null;
		}
	}

    public void ObjectHP()
    {
        Section tmp = SectionManager.Inst().GetCurrentSection();
        CurrentSection = tmp;
        if (CurrentSection == null)
            return;
        Units = CurrentSection.transform.Find("Objects").Find("Deadable").GetComponentsInChildren<UnitData>();
        if (Units == null)
            return;
        if (Units.Length != HPbarUI.Length)
        {
            for (int i = 0; i < HPbarUI.Length; ++i)
            {
                Destroy(HPbarUI[i].gameObject);
            }
            HPbarUI = new Image[Units.Length];
            for (int i = 0; i < Units.Length; ++i)
            {
                HPbarUI[i] = (Image)Instantiate(prefabHPbar);
                HPbarUI[i].GetComponent<ObjectHPUI>().Unit = Units[i];
                HPbarUI[i].transform.parent = ObjectHPUI.transform;
            }
        }
        for (int i = 0; i < HPbarUI.Length; ++i)
            HPbarUI[i].rectTransform.localScale = new Vector2(Units[i].GetHP() / 100f, 1);
    }
}
