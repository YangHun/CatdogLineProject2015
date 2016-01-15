using UnityEngine;
using System.Collections;
using System;

public class Section1 : Section {
    public override float GetSectionScore()
    {
        return 0;
    }

    public override void OnSectionEnd()
    {
        //
    }

    public override void OnSectionPause()
    {
        // send pause message to all object
    }

    public override void OnSectionResume()
    {
        // send resume message to all object
    }

    public override void OnSectionStart()
    {
        IInteractor[] tmp = GetComponentsInChildren<IInteractor>();
        GameManager.Inst().RefreshInteractorList(tmp);
    }
    
}
