using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = PlayerManager.Inst().GetPlayer();
        if (other.gameObject == player)
        {
            UnitManager.Inst().OnDie(player.GetComponent<UnitData>());
        }
    }

}
