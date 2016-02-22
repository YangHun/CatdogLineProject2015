using UnityEngine;
using System.Collections;

public class ObjectHunter_a : MonoBehaviour {

    private float WaitTime = 0.0f;
    public bool Move = false;
    public Rigidbody2D rigid;
    private float ReloadTime = 3.0f;
    private bool Reload = false;
    private bool FirstJump = false;
    void Start () {
        rigid = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (WaitTime > 0)
            WaitTime -= GameTime.deltaTime;
        if (WaitTime <= 0 && !Move)
            Move = true;
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
}
