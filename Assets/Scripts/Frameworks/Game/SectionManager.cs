using UnityEngine;
using System.Collections;

public class SectionManager : SingleTonBehaviour<SectionManager>
{

    private Section m_CurrentSection = null;
    private SectionUI m_CurrentSectionUI = null;

    public void OnSectionEnter(Section section)
    {
        if (m_CurrentSection != null)
        {
            // reject
            return;
        }

        Debug.Log("Section Start : " + section.name);
        m_CurrentSection = section;
        m_CurrentSection.OnSectionStart();
        m_CurrentSectionUI = m_CurrentSection.GetSectionUI();
    }


    public void OnSectionExit(Section section)
    {
        if (m_CurrentSection == null)
        {
            // reject
            return;
        }

        Debug.Log("Section Exit : " + section.name);
        m_CurrentSection.OnSectionEnd();
        m_CurrentSection = null;
        m_CurrentSectionUI = null;

        GameManager.Inst().OnEndSection();
    }


    public void PauseSection()
    {
        if (m_CurrentSection == null)
        {
            // reject
            return;
        }

        Debug.Log("Section Pause : " + m_CurrentSection.name);
        m_CurrentSection.OnSectionPause();
    }

    public void ResumeSection()
    {
        if (m_CurrentSection == null)
        {
            // reject
            return;
        }

        Debug.Log("Section Resume : " + m_CurrentSection.name);
        m_CurrentSection.OnSectionResume();
    }

    public void PauseSectionUI()
    {
        if(m_CurrentSectionUI == null)
        {
            // reject
            return;
        }


        Debug.Log("SectionUI Pause : " + m_CurrentSectionUI.name);
        m_CurrentSectionUI.PauseSectionUI();
    }

    public void ResumeSectionUI()
    {
        if (m_CurrentSectionUI == null)
        {
            // reject
            return;
        }


        Debug.Log("SectionUI Resume : " + m_CurrentSectionUI.name);
        m_CurrentSectionUI.ResumeSectionUI();
    }

    public bool IsSectionEnabled(Section section)
    {
        if (m_CurrentSection == null)
            return false;
        return section == m_CurrentSection;
    }

    public Section GetCurrentSection()
    {
        return m_CurrentSection;
    }

    public SectionUI GetCurrentSectionUI()
    {
        return m_CurrentSectionUI;
    }
}
