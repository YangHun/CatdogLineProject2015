using UnityEngine;
using System.Collections;

public class ObjectN3 : UnitData, IHealable
{
    private Rigidbody2D Rigid = null;
    [SerializeField]
    private HealType m_Type = HealType.GREEN;
    public Transform Rock = null;
    [SerializeField]
    private ObjectPlant_b Plant = null;
    public Transform Talk1 = null;
    public Transform Talk2 = null;
    public Transform Talk3 = null;
    public Transform Talk4 = null;

    void Start()
    {
        Rigid = GetComponent<Rigidbody2D>();

        Talk1.transform.position = new Vector2(1000f, 1000f);
        Talk2.transform.position = new Vector2(1000f, 1000f);
        Talk3.transform.position = new Vector2(1000f, 1000f);
        Talk4.transform.position = new Vector2(1000f, 1000f);
    }

    void Update()
    {
       if (this.GetHP() == this.GetMaxHP())
       {
            if(!Plant.IsOn() && this.transform.position.y > 3f)
            {
                Talk2.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
            }
            else if(Plant.IsOn() || this.transform.position.y <= 3f)
            {
                //Rigid.velocity = new Vector3(-5f, 0f, 0f);
                StartCoroutine(Moveout());
                Talk3.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
            }
       }
       else if (Rock.position.y > -2.3f)
       {
            Talk1.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3f);
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

    IEnumerator Moveout()
    {
        for (float x = this.transform.localPosition.x; x >= 34.82; x -= 0.1f)
        {
            this.transform.localPosition = new Vector3 (x,this.transform.localPosition.y);            
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }
}
