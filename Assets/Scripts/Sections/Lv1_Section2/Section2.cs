using UnityEngine;
using System.Collections;

public class Section2 : Section
{
    [SerializeField]
    private Collider2D Block;

    public GameObject[] StartActivateList = null;

    public override void OnSectionStart()
    {
        IInteractor[] tmp = GetComponentsInChildren<IInteractor>();
        GameManager.Inst().RefreshInteractorList(tmp);
        Block.gameObject.SetActive(true);

        foreach (var obj in StartActivateList)
            obj.SetActive(true);
    }

    public override void OnSectionPause()
    {
        
    }

    public override void OnSectionResume()
    {
        
    }

    public override void OnSectionEnd()
    {
        
    }

    public override float GetSectionScore()
    {
        return 0;
    }
}
