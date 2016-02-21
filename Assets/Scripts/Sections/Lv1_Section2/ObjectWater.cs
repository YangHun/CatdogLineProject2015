using UnityEngine;
using System.Collections;

public class ObjectWater : MonoBehaviour {

    [SerializeField]
    private ObjectJungSoo JS;
    [SerializeField]
    private GameObject Healed, Polluted;
    private bool Healthy = false;

	void Update ()
    {
        if (JS.IsHealthy() && !Healthy)
        {
            Healed.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
            StartCoroutine(FadeOut());
            Healthy = true;
        }
	}
    IEnumerator FadeOut()
    {
        for (float x = 1; x >= 0; x -= 0.01f)
        {
            Polluted.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, x);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
        Polluted.gameObject.SetActive(false);
    }
    IEnumerator FadeIn()
    {
        for (float x = 0; x <= 1; x += 0.01f)
        {
            Healed.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, x);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }
}
