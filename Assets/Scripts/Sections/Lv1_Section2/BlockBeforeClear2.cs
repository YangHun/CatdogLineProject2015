using UnityEngine;
using System.Collections;

public class BlockBeforeClear2 : MonoBehaviour {

    [SerializeField]
    private ObjectJungSoo JS;

	void Update () {
	    if (JS.IsHealthy())
        {
            Destroy(this);
        }
	}
}
