using UnityEngine;
using System.Collections;

public class SetActive : MonoBehaviour {

    [SerializeField]
    private ObjectJungSoo JS;
    [SerializeField]
    private GameObject JS2;

    void Update()
    {
        if (JS.IsHealthy())
        {
            JS2.SetActive(true);
        }
    }
}
