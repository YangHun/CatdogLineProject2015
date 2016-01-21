using UnityEngine;
using System.Collections;

public class ObjectBarrierSeed : MonoBehaviour {

    [SerializeField]
    private GameObject N3;
    [SerializeField]
    private GameObject S1, S2;

    private bool seedOn = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == N3)
        {
            S1.SetActive(true);
            S2.SetActive(true);
            seedOn = true;
            Debug.Log("Barrier : N3 In Range");
            StartCoroutine(FadeIn(S1));
            StartCoroutine(FadeIn(S2));
        }
    }

    public bool IsOn()
    {
        return seedOn;
    }

    IEnumerator FadeIn(GameObject S)
    {
        Color c = new Color(255, 255, 255, 0);
        for (float f = 0; f <= 1f; f += 0.01f)
        {
            c.a = f;
            S.GetComponent<SpriteRenderer>().color = c;
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
        DestroyObject(this);
    }
}
