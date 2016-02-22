using UnityEngine;
using System.Collections;

public class ObjectMonkey_b : UnitData {

    private float t = 8;

	public override void Start()
	{
		base.Start();
		StartCoroutine(ClimbUpDown());
    }

    IEnumerator ClimbUpDown()
    {
        while (true)
        {
            float y = this.transform.localPosition.y;
            for (float x = 0; x < 5; x += 5 / 120f)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, y + x);
                yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
            }
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(2));
            y = this.transform.localPosition.y;
            for (float x = 0; x < 5; x += 5 / 120f)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, y - x);
                yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
            }
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(2));
        }
    }
}
