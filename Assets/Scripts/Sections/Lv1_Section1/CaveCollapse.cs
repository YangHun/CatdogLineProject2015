using UnityEngine;
using System.Collections;

public class CaveCollapse : TriggerZone {

    private bool IsCaveCollapse = false;
    [SerializeField]
    private float CollapseTime = 3.0f;
    [SerializeField]
    private GameObject Barrier;

    void Update()
    {
        if (IsCaveCollapse && CollapseTime > 0)
        {
            CollapseTime -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            GameManager.Inst().StartCinematic(CollapseTime);
            IsCaveCollapse = true;
            Debug.Log("Sam : Player In Range");
            Barrier.SetActive(true);
            Barrier.GetComponent<SpriteRenderer>().color = new Color(255,255,255,0);
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        Color c = new Color(255,255,255,0);
        for (float f = 0; f <= 1f; f += 0.01f)
        {
            c.a = f;
            Barrier.GetComponent<SpriteRenderer>().color = c;
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1/60f));
        }
        DestroyObject(this);
    }

}
