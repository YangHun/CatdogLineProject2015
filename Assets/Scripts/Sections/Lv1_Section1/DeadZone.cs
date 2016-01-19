using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameManager.Inst().GetPlayer())
        {
            DestroyObject(GameManager.Inst().GetPlayer());
            Debug.Log("U R dead");
        }
    }

}
