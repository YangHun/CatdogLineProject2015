using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ObjectHPUI : MonoBehaviour {

    public Text A;
    public ObjectHurted B;
    private Vector2 pos;
    private Vector2 viewportPoint;

	void Update () {
        pos = B.GetPOS();
        viewportPoint = Camera.main.WorldToViewportPoint(pos);
        A.rectTransform.anchorMin = viewportPoint;
        A.rectTransform.anchorMax = viewportPoint; 
        A.text = B.GetHP().ToString();
        if (B.IsHealthy() || B.GetHP() <= 0)
            Destroy(A.gameObject);
	}
}
