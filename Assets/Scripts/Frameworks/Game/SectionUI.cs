using UnityEngine;
using System.Collections;

public abstract class SectionUI : MonoBehaviour{
    public GameObject SectionUICanvas = null;

    public abstract void PauseSectionUI();
    public abstract void ResumeSectionUI();
}
