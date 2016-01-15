using UnityEngine;
using System.Collections;

public class ObjectVine : UnitData, IInteractor, IHealable {

    private HealType m_Type = HealType.GREEN;
    //[SerializeField] private GameObject player;
    //private Rigidbody2D playerRigid;
    //private Collider2D col;
    //[SerializeField]
    //private Colliding down;
    //[SerializeField]
    //private Colliding up;
    private bool m_IsPlayerInRange = false;
    private bool climbing = false;
//    private bool NowInterAct = false;
//    private int GoUp = 1;
    //void Start()
    //{
    //    playerRigid = player.GetComponent<Rigidbody2D>();
    //    col = player.GetComponent<Collider2D>();
    //}
    //
    //void Update ()
    //{
    //    if (NowInterAct)
    //    {
    //        playerRigid.velocity = new Vector3(0, 5.0f * GoUp, 0);
    //    }
    //    if (!m_IsPlayerInRange)
    //    {
    //        NowInterAct = false;
    //        playerRigid.gravityScale = 3.0f;
    //        col.isTrigger = false;
    //    }
    //}

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
            //NowInterAct = false;
            Debug.Log("Vine : Player Out of Range");
            climbing = false;
        }
    }

    public void OnInteractStart()
    {
        if (m_IsPlayerInRange)
        {
            climbing = true;
            //NowInterAct = true;
            //playerRigid.gravityScale = 0.0f;
            //col.isTrigger = true;
            //if (down.IsCollided())
            //    GoUp = 1;
            //else if (up.IsCollided())
            //    GoUp = -1;
            //else
            //    GoUp *= -1;
        }
            
    }

    public void OnInteractStay()
    {
        GameManager.Inst().StopInteraction();
    }

    public void OnInteractEnd()
    {
        //do nothing
    }

    public bool IsInteractable()
    {
        if (IsHealthy() && m_IsPlayerInRange)
            return true;
        return false;
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

    public bool IsClimbing()
    {
        return climbing;
    }
}
