using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {
    [SerializeField]
    public GameObject breaking;
    [SerializeField]
    private Rigidbody2D body, body2;
    private bool broken = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == breaking)
        {
            Debug.Log("Is Broken");
            body.constraints = RigidbodyConstraints2D.None;
            body2.constraints = RigidbodyConstraints2D.None;
            broken = true;
        }
    }
    public bool IsBroken()
    {
        return broken;
    }
}
