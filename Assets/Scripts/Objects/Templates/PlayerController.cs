using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : UnitData, IController, IHealable
{
    // parameter
    public float m_Speed = 8.0f;
    public float m_JumpSpeed = 15.0f;

    // animation
    private Animator anim;

    // horizontal move
    public bool m_FacingRight = true;

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
    private bool m_IsPaused = false;

    // jump state
    [SerializeField]
    private Collider2D[] mCollisionIgnoreColliders = null;
    [SerializeField]
    private bool m_IsGrounded = false;
    private bool[] m_IsGroundedBuffed = new bool[2];
    private List<Collider2D>[] m_GroundedCollider = new List<Collider2D>[2];

    // physics
    private Rigidbody2D rigid = null;
    private Collider2D playerCollision;
    private Vector3 m_PrevPosition = new Vector3();

    [SerializeField]
    private Rect m_JumpCollisionIgnore;
    [SerializeField]
    private Rect m_FallCollisionIgnore;
    [SerializeField]
    private Rect m_WalkCollisionIgnore;
    [SerializeField]
    private Rect m_WallCollision;

    void Awake()
    {
        mCollisionIgnoreColliders = new Collider2D[0];

        mPlayerAction = new StateMachine();
        mPlayerAction.AddState(PlayerActionState.WALKING, OnStateWalking);
        mPlayerAction.AddState(PlayerActionState.JUMPING, OnStateJumping);
        mPlayerAction.AddState(PlayerActionState.JUMP_FALL, OnStateJumpFall);
        mPlayerAction.AddState(PlayerActionState.CLIMBING, OnStateClimb);
        mPlayerAction.SetInitialState(PlayerActionState.WALKING);

        m_PrevPosition = transform.position;
    }

	// Use this for initialization
	public override void Start()
    {
		base.Start();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(mPlayerAction.GetCurrentState());
        if (!GameSceneController.Inst().IsInGame())
        {
            return;
        }
        
        // initialize buffer
        m_IsGroundedBuffed[0] = m_IsGroundedBuffed[1];
        m_IsGroundedBuffed[1] = false;
        m_GroundedCollider[0] = m_GroundedCollider[1];
        m_GroundedCollider[1] = new List<Collider2D>();

        // overlap check
        var mask = LayerMask.GetMask("Map", "PassableMap", "MovableObject");
        Vector2 ground = GroundPoint.position;
        var collidings = Physics2D.OverlapAreaAll(ground + new Vector2(-0.3f, -0.25f),
            ground + new Vector2(0.3f, 0.25f), mask);

        if (collidings.Length != 0)
        {
            foreach (var colliding in collidings)
            {
                if (mCollisionIgnoreColliders.Length == 0
                    || !Array.Exists(mCollisionIgnoreColliders, element => element == colliding))
                {
                    m_IsGroundedBuffed[1] = true;
                    m_GroundedCollider[1].Add(colliding);
                }
            }
        }

        // sets
        m_IsGrounded = m_IsGroundedBuffed[0] || m_IsGroundedBuffed[1];


        // update each state
        mPlayerAction.Update();
        SetJumpAnimation();

    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (!GameSceneController.Inst().IsInGame())
        {
            transform.position = m_PrevPosition;
            anim.enabled = false;
            m_IsPaused = true;
            rigid.velocity = new Vector3();
            return;
        }
        else if (m_IsPaused)
        {
            anim.enabled = true;
        }

        m_PrevPosition = transform.position;
        mPlayerAction.LateUpdate();
    }
    
    void OnStateWalking()
    {
        //anim.SetBool("IsJump", false);
        IgnoreCollision(mCollisionIgnoreColliders, false);

        //var bottomleft = new Vector2(transform.position.x - 1.1f, transform.position.y - 0.3f);
        //var topright = new Vector2(transform.position.x + 1.1f, transform.position.y + 1.0f);
        var mask = LayerMask.GetMask("PassableMap");
        List<Collider2D> overlaps = new List<Collider2D>(OverlapArea(transform.position, m_WalkCollisionIgnore, mask));

        // remove grounded map objects
        if (m_IsGrounded)
        {
            //anim.SetBool("IsJump", false);
            // remove grounded objects from collision ignores
            foreach (var ground in m_GroundedCollider[0])
                if (overlaps.Contains(ground))
                    overlaps.Remove(ground);

            foreach (var ground in m_GroundedCollider[1])
                if (overlaps.Contains(ground))
                    overlaps.Remove(ground);
        }
        mCollisionIgnoreColliders = overlaps.ToArray();

        IgnoreCollision(mCollisionIgnoreColliders, true);

        MoveHorizontal();

        // check falling
        if (!m_IsGrounded)
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);
            mPlayerAction.ChangeState(PlayerActionState.JUMP_FALL, 2);
        }

        // check jumping
        if (InputManager.Inst().IsJumpClicked() && m_IsJumpCoolDownOn)
        {
            
            IgnoreCollision(mCollisionIgnoreColliders, false);
            mPlayerAction.ChangeState(PlayerActionState.JUMPING, 1);
            StartCoroutine(DelayJump());
        }
    }

    void OnStateJumping()
    {
        if (mPlayerAction.IsFirstUpdate())
        {
            IgnoreCollision(mCollisionIgnoreColliders, false);

            rigid.velocity = new Vector2(rigid.velocity.x, m_JumpSpeed);

            //var bottomleft = new Vector2(GroundPoint.position.x - 8.0f, GroundPoint.position.y + 0.5f);
            //var topright = new Vector2(GroundPoint.position.x + 8.0f, GroundPoint.position.y + 4.5f);
            var mask = LayerMask.GetMask("PassableMap");
            var overlaps = OverlapArea(transform.position, m_JumpCollisionIgnore, mask);
            mCollisionIgnoreColliders = overlaps;

            IgnoreCollision(mCollisionIgnoreColliders,  true);
        }


        MoveHorizontal();


        // check falling
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

            //var bottomleft = new Vector2(GroundPoint.position.x - 8.0f, GroundPoint.position.y + 0.5f);
            //var topright = new Vector2(GroundPoint.position.x + 8.0f, GroundPoint.position.y + 4.5f);
            var bottomleft = new Vector2(transform.position.x - 1.5f, transform.position.y - 1.2f);
            var topright = new Vector2(transform.position.x + 1.5f, transform.position.y + 0.9f);

            var mask = LayerMask.GetMask("PassableMap");
            var overlaps = OverlapArea(transform.position, m_FallCollisionIgnore, mask);
            //var overlaps = Physics2D.OverlapAreaAll(bottomleft, topright, mask);
            mCollisionIgnoreColliders = overlaps;

            IgnoreCollision(mCollisionIgnoreColliders, true);
        }

        MoveHorizontal();
        
        // check grounded
        if (m_IsGrounded)
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
        prev_position += new Vector3(m_ClimbDirection.x, m_ClimbDirection.y, 0) * m_ClimbSpeed * GameTime.deltaTime;
        transform.position = prev_position;
    }

    void SetJumpAnimation()
    {
        bool isjump = false;
        if(mPlayerAction.IsFirstUpdate())
        {
            if (mPlayerAction.IsCurrentState(PlayerActionState.JUMPING))
                isjump = true;
        }
        anim.SetBool("IsJump", isjump);
    }

    
    void MoveHorizontal()
    {
        // get input
        float dir_x = 0;
        if (InputManager.Inst().IsLeftClicked())
            dir_x -= 1;
        if (InputManager.Inst().IsRightClicked())
            dir_x += 1;

        // check collision with unmovable walls
        var mask = LayerMask.GetMask("Map", "PassableMap");
        if (!mPlayerAction.IsCurrentState(PlayerActionState.WALKING))
            mask = LayerMask.GetMask("Map", "PassableMap", "MovableObject");
        Vector2 current = transform.position;
        Rect tmp = m_WallCollision;
        tmp.x = tmp.x * dir_x;

        var collidings = OverlapArea(transform.position, tmp, mask);
        if (collidings.Length != 0)
        {
            // stop moving if collide
            foreach (var colliding in collidings)
            {
                if (!Array.Exists(mCollisionIgnoreColliders, element => element == colliding))
                    dir_x = 0;
            }
        }


        if (mPlayerAction.IsCurrentState(PlayerActionState.WALKING) && dir_x != 0)
            StepUp(current + new Vector2(dir_x * (m_Speed * GameTime.deltaTime + 0.375f), -1), 1f);

        rigid.velocity = new Vector3(dir_x * m_Speed, rigid.velocity.y, 1);


        // set animation
        anim.SetFloat("Speed", Mathf.Abs(dir_x));
        if (dir_x > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (dir_x < 0 && m_FacingRight)
        {
            Flip();
        }

    }

    void StepUp(Vector2 nextPosition, float up)
    {
        // check stepup available
        int mask = LayerMask.GetMask("Map", "MovableObject");
        var overlapdown = Physics2D.OverlapPoint(nextPosition, mask);
        var overlapup = Physics2D.OverlapPoint(nextPosition + new Vector2(0, up), mask);
        if (overlapup != null || overlapdown == null || overlapdown.Equals(overlapup))
            return;

        // raycast available move point
        var collide = Physics2D.Raycast(nextPosition + new Vector2(0, up), new Vector2(0, -1), 0.5f, mask);
        var delta = collide.point - nextPosition;
        if (Vector2.SqrMagnitude(delta) > 1)
            return;


        // force moving
        Vector2 current = transform.position;
        transform.position = current + delta;
        rigid.velocity = new Vector2(0, 0);
        if(Debug.isDebugBuild)
            Debug.Log("Stepup for : " + delta.y);

    }


    Collider2D[] OverlapArea(Vector2 center, Rect area, int layermask)
    {
        Vector2 bottomleft = center + new Vector2(area.x - area.width / 2, area.y - area.height / 2);
        Vector2 topright = center + new Vector2(area.x + area.width / 2, area.y + area.height / 2);
        return Physics2D.OverlapAreaAll(bottomleft, topright, layermask);
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


    public void GiveGuilty(float amount)
    {
        MaxHP -= amount;
		if (MaxHP <= 0)
		{
			MaxHP = 0;
		}
		
		if (CurrentHP > MaxHP)
		{
			CurrentHP = MaxHP;
		}
    }

    public bool IsGrounded()
    {
        return m_IsGrounded;
    }
}
