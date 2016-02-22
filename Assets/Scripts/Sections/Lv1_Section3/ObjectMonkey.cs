using UnityEngine;
using System.Collections;

public class ObjectMonkey : UnitData {
    public ObjectSickVine vine;
    [SerializeField]
    private float MoveSpeed_Normal;
    [SerializeField]
    private float MoveSpeed_Attacked;
    private bool attacked = false;
    private float MovingTime;
    private int Direction = 1;
    private bool waiting = false;
    private float WaitingTime;
    private bool climbing = false;
    public bool hide = false;
    [SerializeField]
    private GameSceneController Scene;

    void Start()
    {
        MovingTime = Random.Range(1, 5);
        Direction = Random.Range(1, 0);
        if (Direction == 0)
            Direction = -1;
    }
    void Update()
    {
        if (!Scene.IsInGame())
            this.GetComponent<Rigidbody2D>().velocity = new Vector2();
        else
        {
            if (!climbing)
            {
                if (vine.IsHealthy())
                {
                    if (vine.transform.localPosition.x - this.transform.localPosition.x > 0)
                    {
                        this.transform.localPosition = new Vector3(this.transform.localPosition.x + 0.1f * MoveSpeed_Attacked, this.transform.localPosition.y);
                    }
                    else
                    {
                        this.transform.localPosition = new Vector3(this.transform.localPosition.x - 0.1f * MoveSpeed_Attacked, this.transform.localPosition.y);
                    }
                }

                if (!vine.IsHealthy())
                {
                    if (!waiting)
                    {
                        MovingTime -= GameTime.deltaTime;
                        if (MovingTime <= 0)
                        {
                            MovingTime = Random.Range(1, 5);
                            Direction *= -1;
                        }
                        if (MovingTime > 0)
                        {
                            if (attacked)
                            {
                                this.transform.localPosition = new Vector3(this.transform.localPosition.x - 0.1f * MoveSpeed_Attacked * Direction, this.transform.localPosition.y);
                            }
                            else
                            {
                                this.transform.localPosition = new Vector3(this.transform.localPosition.x - 0.1f * MoveSpeed_Normal * Direction, this.transform.localPosition.y);
                            }
                        }
                        if (this.transform.localPosition.x <= 78.6 || this.transform.localPosition.x >= 98.87)
                        {
                            waiting = true;
                            WaitingTime = 0.5f;
                            MovingTime = 0;
                        }
                    }
                    if (waiting)
                    {
                        WaitingTime -= GameTime.deltaTime;
                        if (WaitingTime <= 0)
                            waiting = false;
                    }
                }
            }
        }
        //1~5초의 랜덤한 시간을 정해 한쪽으로 움직이다가, 시간이 다 되면 다시 랜덤한 시간을 정해 반대로 움직임.
        //but, 움직이는 도중에 플랫폼의 끝까지 가면 0.5초 동안 멈춘 후에, 반대로 감.
        //공격당하면 5초간 빠르게 움직임
        //덩쿨이 치료되면 덩쿨쪽으로 움직인다.
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == vine.gameObject && vine.IsHealthy())
        {
            climbing = true;
            StartCoroutine(ClimbUp());
            Debug.Log("Vine In Range");
            this.GetComponent<Rigidbody2D>().gravityScale = -0.01f;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == vine.gameObject && vine.IsHealthy() && !climbing)
        {
            climbing = true;
            StartCoroutine(ClimbUp());
            Debug.Log("Vine In Range");
            this.GetComponent<Rigidbody2D>().gravityScale = -0.01f;
        }
    }

    public bool IsHide()
    {
        return hide;
    }

    IEnumerator ClimbUp()
    {
        float y = this.transform.localPosition.y;
        for (float x = 0; x < 8; x += 0.1f)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, y + x);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
        hide = true;
    }
}