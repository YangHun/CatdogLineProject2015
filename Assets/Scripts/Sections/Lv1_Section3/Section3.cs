using UnityEngine;
using System.Collections;

public class Section3 : Section
{
    [SerializeField]
    private Collider2D Block;

    public override void OnSectionStart()
    {
        IInteractor[] tmp = GetComponentsInChildren<IInteractor>();
        GameManager.Inst().RefreshInteractorList(tmp);
        Block.gameObject.SetActive(true);
    }

    public override void OnSectionPause()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSectionResume()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSectionEnd()
    {
        throw new System.NotImplementedException();
    }

    public override float GetSectionScore()
    {
        return 0;
    }
}