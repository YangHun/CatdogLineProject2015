using UnityEngine;
using System.Collections;

public class FadeOutNPCs : MonoBehaviour
{
    [SerializeField]
    private GameObject N1, N2, N3;
    [SerializeField]
    private ObjectSeed S1, S2;
    private bool activated = false;
    private SpriteRenderer[] SpritesN1, SpritesN2, SpritesN3;
    void Update()
    {
        if (S1.IsHealthy() && S2.IsHealthy() && !activated)
        {
            SpritesN1 = N1.GetComponentsInChildren<SpriteRenderer>();
            SpritesN2 = N2.GetComponentsInChildren<SpriteRenderer>();
            SpritesN3 = N3.GetComponentsInChildren<SpriteRenderer>();
            activated = true;
            for (int i = 0; i < SpritesN1.Length; ++i )
                StartCoroutine(FadeOut(SpritesN1[i]));
            for (int i = 0; i < SpritesN2.Length; ++i)
                StartCoroutine(FadeOut(SpritesN2[i]));
            for (int i = 0; i < SpritesN3.Length; ++i)
                StartCoroutine(FadeOut(SpritesN3[i]));
            activated = true;
        }
    }

    IEnumerator FadeOut(SpriteRenderer sprite)
    {
        for (float x = 1; x >= 0; x -= 0.01f)
        {
            sprite.color = new Color(255, 255, 255, x);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }
}