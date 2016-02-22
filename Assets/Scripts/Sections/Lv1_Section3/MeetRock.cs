using UnityEngine;
using System.Collections;

public class MeetRock : MonoBehaviour {

    [SerializeField]
    private ObjectHunter_a Hunter;
    [SerializeField]
    private GameObject Rock1, Rock2;
    private bool a = false;
    private float t = 2.0f;
	void Update () {
	    if (a)
        {
            t -= GameTime.deltaTime;
        }
        if (a && t <= 0)
        {
            Hunter.rigid.velocity = new Vector2(0, 15f);
            a = false;
            Hunter.Move = true;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Rock1 || other.gameObject == Rock2)
        {
            Hunter.Move = false;
            a = true;
            t = 2.0f;
        }
    }
}
