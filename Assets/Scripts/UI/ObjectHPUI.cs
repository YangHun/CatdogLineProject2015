using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ObjectHPUI : MonoBehaviour {

    public Image A = null;
    public UnitData Unit = null;
    private Vector2 pos;
    private Vector2 viewportPoint;
    public float a;
	void Update () {
        if (Unit == null)
            return;
        pos = Unit.transform.position;
        viewportPoint = Camera.main.WorldToScreenPoint(pos);
        A.rectTransform.position = new Vector2(viewportPoint.x, viewportPoint.y + a);
	}
}
