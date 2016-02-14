using UnityEngine;
using System.Collections;

public class PlayerManager : SingleTonBehaviour<PlayerManager>
{

    // Player
    [SerializeField]
    private GameObject m_Player = null;
    [SerializeField]
    private AudioSource m_HealingSound = null;

    // Heal Info
    [SerializeField]
    private float m_HealRange = 1.0f;
    [SerializeField]
    private HealType m_HealType = HealType.BLUE;
    [SerializeField]
    private float m_HealCoolDown = 1.0f;
    [SerializeField]
    private float m_HealCoolDownLeft = 0.0f;


    // Use this for initialization
    void Start()
    {
        SetPlayerHealType(m_HealType);

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameSceneController.Inst().IsInGame())
            return;

        UpdateHealCoolDown();
    }


    void UpdateHealCoolDown()
    {
        m_HealCoolDownLeft -= GameTime.deltaTime;
        if (m_HealCoolDownLeft < 0.0f)
            m_HealCoolDownLeft = 0.0f;
    }


    /// <summary>
    /// Calling this will heal area near player
    /// </summary>
    public void HealAreaByPlayer()
    {

        if (m_HealCoolDownLeft > 0.0f)
            return;

        m_HealCoolDownLeft = m_HealCoolDown;

        UnitManager.Inst().HealArea(m_Player.transform.position, m_HealRange);
        if (m_HealingSound != null)
            SoundManager.Inst().AddSound(m_HealingSound, true);
    }
    
    /// <summary>
    /// Change HealType of player
    /// </summary>
    /// <param name="healtype"> new healtype </param>
    public void SetPlayerHealType(HealType healtype)
    {
        Debug.Log("Player heal type change to : " + healtype.ToString());
        GameUIManager.Inst().OnPlayerHealTypeChange(healtype);
        m_HealType = healtype;
    }

    /// <summary>
    /// Get current HealType of player
    /// </summary>
    /// <returns></returns>
    public HealType GetPlayerHealType()
    {
        return m_HealType;
    }


    /// <summary>
    /// Get player
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return m_Player;
    }
    
    /// <summary>
    /// Make player climb.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="direction"></param>
    public void StartPlayerClimbing(Vector2 start, Vector2 destination)
    {
        var direction = destination - start;
        direction.Normalize();
        GetPlayer().GetComponent<PlayerController>().StartClimb(start, direction);
    }

    /// <summary>
    /// Stop player climb.
    /// </summary>
    public void EndPlayerClimbing()
    {
        GetPlayer().GetComponent<PlayerController>().EndClimb();
    }
}
