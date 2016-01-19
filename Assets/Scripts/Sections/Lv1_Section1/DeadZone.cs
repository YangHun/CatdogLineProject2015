using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = GameManager.Inst().GetPlayer();
        if (other.gameObject == player)
        {
            GameManager.Inst().OnDie(player.GetComponent<UnitData>());
        }
    }

}
