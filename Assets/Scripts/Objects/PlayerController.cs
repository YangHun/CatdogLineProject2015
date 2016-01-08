using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour, IController
{
    public float m_Speed = 1.0f;
    public float m_JumpForce = 100f;
    public Transform GroundPoint = null;
    private float dir_x;

    private Rigidbody2D rigid = null;
    public bool m_IsGrounded = false;
    private bool m_JumpCoolDown = false;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (m_IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }
        else if (!m_JumpCoolDown)
        {

            int layer = LayerMask.GetMask("Map");
            var collides = Physics2D.OverlapPoint(GroundPoint.position, layer);
            if (collides)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, 0.0f, 0.0f);
                m_IsGrounded = true;
            }
        }
    }

    void Move()
    {
        //dir_x = Input.GetAxis("Horizontal");
        float tmp = (dir_x * m_Speed);

        rigid.velocity = new Vector3(tmp, rigid.velocity.y, 0);
    }

    void Jump()
    {
        m_IsGrounded = false;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, m_JumpForce));
        StartCoroutine(DelayJump());
    }

    public void OnPause()
    {
        throw new NotImplementedException();
    }

    public void OnResume()
    {
        throw new NotImplementedException();
    }

    IEnumerator DelayJump()
    {
        m_JumpCoolDown = true;
        yield return new WaitForSeconds(0.01f);
        m_JumpCoolDown = false;
    }
#if UNITY_ANDROID
    public void LeftMove() { dir_x = -1; }
    public void RightMove() { dir_x = 1; }
    public void StopMove() { dir_x = 0; }
#endif
}
