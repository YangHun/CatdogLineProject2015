using UnityEngine;
using System.Collections;
using System;

public class Section1UI : SectionUI
{
    
    /// <summary>
    /// Indicates HP of unit if unit is hurt
    /// </summary>
    /// <param name="unit"> unit </param>
    public void AddUnitHPGuage(UnitData unit)
    {

    }

    /// <summary>
    /// Create Text on left top sides for 5 seconds
    /// </summary>
    /// <param name="text"> text </param>
    public void CreateEventText(string text)
    {

    }

    public override void PauseSectionUI()
    {
        // disable HP guage
        // stop event text moving
    }

    public override void ResumeSectionUI()
    {
        // enable HP guage
        // move event text
    }

    public void Update()
    {

    }
}
