using UnityEngine;
using System.Collections;

public class ObjectVine : UnitData, IInteractor, IHealable {

    private HealType m_Type = HealType.GREEN;
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D playerRigid;
    private bool m_IsPlayerInRange = false;
    private bool NowInterAct = false;
    void Update ()
    {
        if (NowInterAct)
            StartCoroutine("MoveUp");
        if (!m_IsPlayerInRange)
        {
            NowInterAct = false;
            playerRigid.gravityScale = 3.0f;
        }
            
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            m_IsPlayerInRange = true;
            Debug.Log("Vine : Player In Range");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            m_IsPlayerInRange = false;
            NowInterAct = false;
            Debug.Log("Vine : Player Out of Range");
        }
    }

    public void OnInteractStart()
    {
        if (m_IsPlayerInRange)
        {
            NowInterAct = true;
            playerRigid.gravityScale = 0.0f;
        }
            
    }

    public void OnInteractStay()
    {

    }

    public void OnInteractEnd()
    {

    }

    public bool IsInteractable()
    {
        if (IsHealthy() && m_IsPlayerInRange)
            return true;
        return false;
    }

    IEnumerator MoveUp()
    {
        player.transform.position += new Vector3(0, 0.1f, 0);
        yield return null;
    }

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
    }

    public bool IsHealable()
    {
        return true;
    }
}
