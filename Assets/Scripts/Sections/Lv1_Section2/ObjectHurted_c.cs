using UnityEngine;
using System.Collections;

public class ObjectHurted_c : UnitData, IHealable {

    public float DamageSecond;
    public float DamageAmount;
    public HealType m_Type;
    private float t = 0;
    [SerializeField]
    private ObjectJungSoo JS;
    [SerializeField]
    private PlayerController Player;
    [SerializeField]
    private bool Jumping = false;
    private int JumpCount = 1;
    private bool reach = false;
    [SerializeField]
    private float walktoplace;

    void Update()
    {
        if (JS.IsHealthy() && this.IsHealthy())
        {
            this.SetHealthy(false);
        }
        if (!IsHealthy())
        {
            t += GameTime.deltaTime;
            if (t >= DamageSecond)
            {
                GiveDamage(DamageAmount);
                t = 0;
            }
            Vector3 a = PlayerManager.Inst().GetPlayer().gameObject.transform.position - this.transform.position;
            if (a.x > 0)
                this.transform.position = new Vector3(this.transform.position.x + 0.05f,this.transform.position.y);
            else
                this.transform.position = new Vector3(this.transform.position.x - 0.05f, this.transform.position.y);
           if (!Player.IsGrounded() && !Jumping && JumpCount > 0)
           {
               Jumping = true;
               this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 15.0f);
               --JumpCount;
           }
           if (this.GetComponent<Rigidbody2D>().velocity.y == 0)
               Jumping = false;
           if (Player.IsGrounded() && JumpCount == 0)
               ++JumpCount;
           if (this.transform.position.y > 24 && IsHealthy() && !Jumping && !reach)
           {
               reach = true;
               StartCoroutine(Move());
           }
        }
    }
    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
    }

    public override void GiveHeal(float amount)
    {
        if (!IsHealthy())
            CurrentHP += amount;

        if (CurrentHP >= MaxHP )
        {
            CurrentHP = MaxHP;
            SetHealthy(true);
        }
        else if (CurrentHP < MaxHP)
        {
            SetHealthy(false);
        }
    }

    public bool IsHealable()
    {
        return true;
    }

    public Vector2 GetPOS()
    {
        return transform.position;
    }

    IEnumerator Move()
    {
        for (float n = this.transform.localPosition.x; n >= walktoplace; n -= 0.03f)
        {
            this.transform.localPosition = new Vector3(n, this.transform.localPosition.y);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
        for (float n = 1; n >= 0; n -= 0.01f)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, n);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
        this.gameObject.SetActive(false);
    }
}
