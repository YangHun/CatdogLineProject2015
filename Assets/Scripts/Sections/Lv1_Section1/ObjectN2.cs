using UnityEngine;
using System.Collections;

public class ObjectN2 : UnitData, IHealable
{
    private Animator anim;
    private Rigidbody2D Rigid = null;
    [SerializeField]
    private HealType m_Type = HealType.GREEN;
    public Transform Rock = null;
    public Transform Talk1 = null;
    public Transform Talk2 = null;
    public Transform Talk3 = null;
    public Transform Talk4 = null;

	public override void Start()
	{
		base.Start();
		Rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Talk1.transform.position = new Vector2(1000f, 1000f);
        Talk2.transform.position = new Vector2(1000f, 1000f);
        Talk3.transform.position = new Vector2(1000f, 1000f);
        Talk4.transform.position = new Vector2(1000f, 1000f);
    }

    void Update()
    {
        if(Rock.position.y > -2.3f)
        {
            Talk1.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
        }
        else if(this.GetHP() < this.GetMaxHP())
        {
            Talk2.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
        }
        else if(this.GetHP() == this.GetMaxHP())
        {
            Talk3.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
            if (this.transform.position.x < 37f)
            {
                anim.SetBool("IsWalking", false);
                Rigid.velocity = new Vector3(0f, 0f, 0f);
                Talk4.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
            }
            else
            {
                anim.SetBool("IsWalking", true);
                Rigid.velocity = new Vector3(-5f, 0f, 0f);
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

    public bool IsHealable()
    {
        return true;
    }
}
