using UnityEngine;
using System.Collections;

public class ObjectPocketPlant : UnitData, IHealable
{

    public HealType m_Type = HealType.GREEN;

    public GameObject BPOff;
    public GameObject BPOn;
    public GameObject APOff;
    public GameObject APOn;
    public ObjectJungSoo JS;
    public float Timer;
    private float BackTimer;

    void Update()
    {
        if (JS.IsHealthy())
        {
            if (BPOff.activeInHierarchy)
            {
                DisableAll();
                APOff.SetActive(true);
            }
            if (BPOn.activeInHierarchy)
            {
                DisableAll();
                APOn.SetActive(true);
            }
        }
        BackTimer -= Time.deltaTime;
        if (BackTimer <= 0)
        {
            DisableAll();
            if (JS.IsHealthy())
                APOff.SetActive(true);
            if (!JS.IsHealthy())
                BPOff.SetActive(true);
        }
    }

    public void OnHealed(HealInfo heal)
    {
        if (heal.type == m_Type)
            GiveHeal(20f);
        else
            GiveHeal(3f);
        if (IsOn())
        {
            BackTimer = Timer;
            DisableAll();
            if (JS.IsHealthy())
                APOn.SetActive(true);
            if (!JS.IsHealthy())
                BPOn.SetActive(true);
        }

    }

    public bool IsHealable()
    {
        return true;
    }

    public bool IsOn()
    {
        return IsHealthy();
    }

    private void DisableAll()
    {
        APOff.SetActive(false);
        APOn.SetActive(false);
        BPOff.SetActive(false);
        BPOn.SetActive(false);
    }
}