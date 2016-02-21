using UnityEngine;
using System.Collections;

public class ObjectWater_b : MonoBehaviour {

    [SerializeField]
    private GameObject water;
    [SerializeField]
    private ObjectJungSoo JS;
    private bool activated = false;

    void Update()
    {
        if (JS.IsHealthy() && !activated)
        {
            activated = true;
            water.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        for (float x = 0; x <= 1; x += 0.01f)
        {
            water.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, x);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }
}
