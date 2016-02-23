using UnityEngine;
using System.Collections;

public class ObjectHunter_a : MonoBehaviour {

    public float WaitTime = 0.0f;
    private bool Wait = true;
    public bool Move = false;
    public Rigidbody2D rigid;
    private float ReloadTime = 3.0f;
    private bool Reload = false;
    private bool FirstJump = false;
    [SerializeField]
    private GameObject Attackable;
    public GameObject foo;
    void Start () {
        rigid = this.GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        if (ReloadTime > 0)
            ReloadTime -= GameTime.deltaTime;
        if (ReloadTime < 0 && !Move)
            Move = true;
	    if (WaitTime > 0)
            WaitTime -= GameTime.deltaTime;
        if (WaitTime <= 0 && Wait)
        {
            Move = true;
            Wait = false;
        }
        if (Move)
        {
            if (this.transform.position.x <= 128.5 && !FirstJump)
            {
                rigid.velocity = new Vector2(0, 20f);
                FirstJump = true;
            }
            this.transform.position = new Vector2(this.transform.position.x - 0.05f, this.transform.position.y);
            if (rigid.velocity.x < 0)
                rigid.velocity = new Vector2(0, 0);
         //   rigid.AddForce(new Vector2(0.02f, 0));
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        foo = other.gameObject;
        if (other.transform.parent.gameObject == Attackable && ReloadTime <= 0 && !other.GetComponent<Hideable>().IsHide())
        {
            ReloadTime = 3.0f;
            Move = false;
            other.GetComponent<UnitData>().GiveDamage(40);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        foo = other.gameObject;
        if (other.transform.parent.gameObject == Attackable && ReloadTime <= 0 && !other.GetComponent<Hideable>().IsHide())
        {
            ReloadTime = 3.0f;
            Move = false;
            other.GetComponent<UnitData>().GiveDamage(40);
        }
    }
}
