using UnityEngine;
using System.Collections;

public class Section2_1 : Section
{
    [SerializeField]
    private Collider2D Block;

    public GameObject[] StartActivateList = null;

    public override void OnSectionStart()
    {
        foreach (var obj in StartActivateList)
            obj.SetActive(true);


        IInteractor[] tmp = GetComponentsInChildren<IInteractor>();
        InteractionManager.Inst().RefreshInteractorList(tmp);
        Block.gameObject.SetActive(true);
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
