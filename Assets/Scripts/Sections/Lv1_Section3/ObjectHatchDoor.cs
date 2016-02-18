using UnityEngine;
using System.Collections;

public class ObjectHatchDoor : ObjectData, IInteractor {

    private float Door1x, Door2x, Door1y, Door2y;
    [SerializeField]
    private bool closed = true;
    [SerializeField]
    private GameObject Door1, Door2;

    void Start()
    {
        Door1x = Door1.transform.localPosition.x;
        Door2x = Door2.transform.localPosition.x;
        Door1y = Door1.transform.localPosition.y;
        Door2y = Door2.transform.localPosition.y;
        StartCoroutine(Open(Door1));
        StartCoroutine(Open(Door2));
    }

    IEnumerator Open(GameObject Door)
    {
        if (Door == Door1)
        {
            for (float n = Door1x; n < Door1x + 1.21f; n += 0.05f)
            {
                Door1.transform.localPosition = new Vector3(n, Door1y + (n - Door1x) * Mathf.Tan((Mathf.PI * Door1.transform.localEulerAngles.z)/ 180));
                yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
            }
            closed = false;

        }
        if (Door == Door2)
        {
            for (float n = Door2x; n > Door2x - 1.21f; n -= 0.05f)
            {
                Door2.transform.localPosition = new Vector3(n, Door2y + (n - Door2x) * Mathf.Tan((Mathf.PI * Door2.transform.localEulerAngles.z) / 180));
                yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
            }
            closed = false;

        }
    }

    IEnumerator Close(GameObject Door)
    {
        if (Door == Door1)
        {
            for (float n = this.transform.localPosition.x; n > Door1x; n -= 0.05f)
            {
                float y = this.transform.localPosition.y;
                Door1.transform.localPosition = new Vector3(n, y - (n - Door1x) * Mathf.Tan(this.transform.rotation.z));
                yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
            }
            closed = true;

        }
        if (Door == Door2)
        {
            for (float n = Door1x; n > Door2x - 1.21f; n -= 0.05f)
            {
                float y = this.transform.localPosition.y;
                Door2.transform.localPosition = new Vector3(n, y + (n - Door2x) * Mathf.Tan(this.transform.rotation.z));
                yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
            }
            closed = true;
        }
    }

    public bool IsOpen()
    {
        return !closed;
    }

    public void OnInteractStart()
    {
        StopAllCoroutines();
        if (closed)
        {
            StartCoroutine(Open(Door1));
            StartCoroutine(Open(Door2));
        }
        else
        {
            StartCoroutine(Close(Door1));
            StartCoroutine(Close(Door2));
        }
    }

    public void OnInteractStay()
    {
        
    }

    public void OnInteractEnd()
    {
        
    }

    public bool IsInteractable()
    {
        return true;
    }
}