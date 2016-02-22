using UnityEngine;
using System.Collections;

public class ObjectMole_b : Hideable {

    [SerializeField]
    private ObjectHatchDoor Door;
    private bool move = false;

	void Update ()
    {
	    if (Door.IsOpen() && !move)
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(Move());
            move = true;
        }
	}
    
    IEnumerator Move()
    {
        for (float n = this.transform.localPosition.x; n <= 131.21; n += 0.1f)
        {
            this.transform.localPosition = new Vector3(n, this.transform.localPosition.y);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }
}
