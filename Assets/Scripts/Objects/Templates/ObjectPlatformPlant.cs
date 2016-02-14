using UnityEngine;
using System.Collections;

public class ObjectPlatformPlant : UnitData, IHealable {

    public HealType m_Type = HealType.GREEN;

    // reference
    public GameObject UnHealed = null;
    public GameObject Healed = null;

    // activation
    [SerializeField]
    public PlantStates InitialState = PlantStates.UnHealed;
    public float ActivateTime = 1.0f;
    private float m_HealedAlpha = 0;

    // return
    public bool ReturnToUnHealed = false;
    public float ReturnHPRate = 0.9f;

    public enum PlantStates
    {
        UnHealed,
        Activating,
        Healed,
        Deactivating
    };
    protected StateMachine m_PlantState = null;


    void Awake()
    {
        m_PlantState = new StateMachine();
        m_PlantState.AddState(PlantStates.UnHealed, OnStateUnHealed);
        m_PlantState.AddState(PlantStates.Activating, OnStateActivating);
        m_PlantState.AddState(PlantStates.Healed, OnStateHealed);
        m_PlantState.AddState(PlantStates.Deactivating, OnStateDeactivating);
        m_PlantState.SetInitialState(InitialState);
    }

    public void Update() { m_PlantState.Update(); }
    public override void LateUpdate()
    {
        base.LateUpdate();
        m_PlantState.LateUpdate();
    }

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);

        if (m_PlantState.GetCurrentState().Equals(PlantStates.UnHealed) && IsFullHealth())
        {
            m_PlantState.ChangeState(PlantStates.Activating);
        }
    }

    public void OnStateActivating()
    {
        if(m_PlantState.IsFirstUpdate())
        {
            m_HealedAlpha = 0;
        }

        m_HealedAlpha += GameTime.deltaTime / ActivateTime;
        if (m_HealedAlpha > 1)
            m_HealedAlpha = 1;

        SetFading();

        if(m_HealedAlpha == 1)
            m_PlantState.ChangeState(PlantStates.Healed);
    }

    public void OnStateDeactivating()
    {
        if (m_PlantState.IsFirstUpdate())
        {
            m_HealedAlpha = 1;
        }

        m_HealedAlpha -= GameTime.deltaTime / ActivateTime;
        if (m_HealedAlpha < 0)
            m_HealedAlpha = 0;

        SetFading();

        if (m_HealedAlpha == 0)
            m_PlantState.ChangeState(PlantStates.UnHealed);
    }

    public void OnStateUnHealed()
    {
        if (m_PlantState.IsFirstUpdate())
        {
            if(UnHealed != null)
                UnHealed.SetActive(true);
            Healed.SetActive(false);
        }
    }

    public virtual void OnStateHealed()
    {
        if(m_PlantState.IsFirstUpdate())
        {
            UnHealed.SetActive(false);
            Healed.SetActive(true);
        }

        if (ReturnToUnHealed && GetHP() <= GetMaxHP() * ReturnHPRate)
            m_PlantState.ChangeState(PlantStates.Deactivating);
    }

    private void SetFading()
    {
        if (UnHealed != null)
        {
            UnHealed.SetActive(true);
            var render = UnHealed.GetComponent<SpriteRenderer>();
            if (render != null)
            {
                var color = render.color;
                color.a = 1 - m_HealedAlpha;
                render.color = color;
            }
        }

        if (Healed != null)
        {
            Healed.SetActive(true);
            var render = Healed.GetComponent<SpriteRenderer>();
            if (render != null)
            {
                var color = render.color;
                color.a = m_HealedAlpha;
                render.color = color;
            }
        }
    }

    public bool IsHealable() { return true; }
    public bool IsPlantActivated() { return m_PlantState.GetCurrentState().Equals(PlantStates.Healed); }
}
