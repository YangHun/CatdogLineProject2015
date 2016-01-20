using UnityEngine;
using System.Collections;
using System;

public class PlayerController : UnitData, IController, IHealable
{
    // parameter
    public float m_Speed = 8.0f;
    public float m_JumpSpeed = 15.0f;

    // animation
    private Animator anim;

    // horizontal move
    private bool m_FacingRight = true;

    // vertical move
    public Transform GroundPoint = null;
    [SerializeField]
    private bool m_IsJumpCoolDownOn = true;

    // climb move
    public float m_ClimbSpeed = 3.0f;
    public Vector2 m_ClimbDirection;

    // player state
    private enum PlayerActionState
    {
        WALKING,
        JUMPING,
        JUMP_FALL,
        FALLING,
        CLIMBING
    }
    private StateMachine mPlayerAction;

    // jump state
    private Collider2D[] mCollisionIgnoreColliders = null;


    // physics
    private Rigidbody2D rigid = null;
    private Collider2D playerCollision;

    void Awake()
    {
        mCollisionIgnoreColliders = new Collider2D[0];

        mPlayerAction = new StateMachine();
        mPlayerAction.AddState(PlayerActionState.WALKING, OnStateWalking);
        mPlayerAction.AddState(PlayerActionState.JUMPING, OnStateJumping);
        mPlayerAction.AddState(PlayerActionState.JUMP_FALL, OnStateJumpFall);
        mPlayerAction.AddState(PlayerActionState.CLIMBING, OnStateClimb);
        mPlayerAction.SetInitialState(PlayerActionState.WALKING);
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameSceneController.Inst().IsInGame())
            mPlayerAction.Update();
    }

    void LateUpdate()
    {
        if (GameSceneController.Inst().IsInGame())
            mPlayerAction.LateUpdate();
    }

    void OnStateWalking()
    {
        MoveHorizontal();


        IgnoreCollision(mCollisionIgnoreColliders, false);

        var bottomleft = new Vector2(GroundPoint.position.x - 0.5f, GroundPoint.position.y + 0.8f);
        var topright = new Vector2(GroundPoint.position.x + 0.5f, GroundPoint.position.y + 2.5f);
        var mask = LayerMask.GetMask("PassableMap");

        mCollisionIgnoreColliders = Physics2D.OverlapAreaAll(bottomleft, topright, mask);

        IgnoreCollision(mCollisionIgnoreColliders, true);


        // check falling


        if (InputManager.Inst().IsJumpClicked() && m_IsJumpCoolDownOn)
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);
            mPlayerAction.ChangeState(PlayerActionState.JUMPING);
            StartCoroutine(DelayJump());
        }
    }

    void OnStateJumping()
    {
        if(mPlayerAction.IsFirstUpdate())
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);

            rigid.velocity = new Vector2(rigid.velocity.x, m_JumpSpeed);

            var bottomleft = new Vector2(GroundPoint.position.x - 8.0f, GroundPoint.position.y + 0.5f);
            var topright = new Vector2(GroundPoint.position.x + 8.0f, GroundPoint.position.y + 4.5f);
            var mask = LayerMask.GetMask("PassableMap");

            mCollisionIgnoreColliders = Physics2D.OverlapAreaAll(bottomleft, topright, mask);

            IgnoreCollision(mCollisionIgnoreColliders,  true);
        }


        MoveHorizontal();


        if (m_IsJumpCoolDownOn && rigid.velocity.y <= 0)
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);
            mPlayerAction.ChangeState(PlayerActionState.JUMP_FALL);
        }
    }

    void OnStateJumpFall()
    {

        if (mPlayerAction.IsFirstUpdate())
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);

            var bottomleft = new Vector2(GroundPoint.position.x - 0.4f, GroundPoint.position.y + 0.2f);
            var topright = new Vector2(GroundPoint.position.x + 0.4f, GroundPoint.position.y + 2.5f);
            var mask = LayerMask.GetMask("PassableMap");

            mCollisionIgnoreColliders = Physics2D.OverlapAreaAll(bottomleft, topright, mask);

            IgnoreCollision(mCollisionIgnoreColliders, true);
        }

        MoveHorizontal();

        bool grounded = false;
        var mask2 = LayerMask.GetMask("Map", "PassableMap");
        var collidings = Physics2D.OverlapPointAll(GroundPoint.position, mask2);

        if (collidings.Length != 0)
        {
            foreach(var colliding in collidings)
            {
                if (!Array.Exists(mCollisionIgnoreColliders, element => element == colliding))
                    grounded = true;
            }
        }



        if (grounded)
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);
            mPlayerAction.ChangeState(PlayerActionState.WALKING);
        }
    }


    void OnStateClimb()
    {
        if(mPlayerAction.IsFirstUpdate())
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);
        }

        var prev_position = transform.position;
        prev_position += new Vector3(m_ClimbDirection.x, m_ClimbDirection.y, 0) * m_ClimbSpeed * Time.deltaTime;
        transform.position = prev_position;
    }


    void MoveHorizontal()
    {
        float dir_x = 0;
        if (InputManager.Inst().IsLeftClicked())
            dir_x -= 1;
        if (InputManager.Inst().IsRightClicked())
            dir_x += 1;

        anim.SetFloat("Speed", Mathf.Abs(dir_x));
        if (dir_x > 0 && !m_FacingRight) { Flip(); }
        else if (dir_x < 0 && m_FacingRight) { Flip(); }

        rigid.velocity = new Vector3(dir_x * m_Speed, rigid.velocity.y, 0);
    }


    void IgnoreCollision(Collider2D[] colliders, bool enable = true)
    {
        foreach (var collider in colliders)
            Physics2D.IgnoreCollision(playerCollision, collider, enable);
    }

    IEnumerator DelayJump()
    {
        m_IsJumpCoolDownOn = false;
        yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(0.2f));
        m_IsJumpCoolDownOn = true;
    }
    

    public void StartClimb(Vector2 start, Vector2 direction)
    {
        transform.position = start;
        m_ClimbDirection = direction;
        rigid.isKinematic = true;

        mPlayerAction.ChangeState(PlayerActionState.CLIMBING, 10);
    }

    public void EndClimb()
    {
        rigid.isKinematic = false;
        mPlayerAction.ChangeState(PlayerActionState.JUMP_FALL, 10);
    }

    public void OnPause()
    {
        throw new NotImplementedException();
    }

    public void OnResume()
    {
        throw new NotImplementedException();
    }

    void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void OnHealed(HealInfo heal)
    {
        GiveHeal(10f);
    }

    public bool IsHealable()
    {
        return true;
    }

    public void OnDamaged(float damage)
    {
        GiveDamage(damage);
    }
}
