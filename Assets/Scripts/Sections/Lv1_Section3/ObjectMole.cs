﻿using UnityEngine;
using System.Collections;

public class ObjectMole : ObjectHurted {

    public float waiting;
    public float PoisonWaitTime;
    public float PoisonTime;
    private bool underground = true;
    private bool poisoning = false;
    private bool fix = false;
    [SerializeField]
    private Breakable block;
    [SerializeField]
    private SpriteRenderer spriterender;
    Color PoisonedColor = new Color(0.5f, 0f, 0.5f);
    private float t;

    void Update()
    {
        if (PoisonWaitTime > 0 && !fix)
        {
            waiting -= GameTime.deltaTime;
            if (waiting <= 0)
            {
                waiting = 20f;
                underground = false;
                StartCoroutine(Appear());
            }
        }

        if (block.IsBroken() && PoisonWaitTime > 0)
            PoisonWaitTime -= GameTime.deltaTime;

        if (PoisonWaitTime <= 0 && underground)
        {
            underground = false;
            StartCoroutine(AppearFixed());
        }

        if (PoisonWaitTime <= 0)
        {
            SetHealthy(false);
            DamageAmount = 10;
            poisoning = true;
            fix = true;
        }

        if (PoisonWaitTime < 0 && PoisonTime > 0)
        {
            PoisonTime -= GameTime.deltaTime;
        }

        if (PoisonTime < 0)
            poisoning = false;

        if (!IsHealthy())
        {
            spriterender.color = PoisonedColor;
            t += Time.deltaTime;
            if (t >= DamageSecond)
            {
                GiveDamage(DamageAmount);
                t = 0;
            }
        }
    }

    public override void GiveHeal(float amount)
    {
        if (!IsHealthy())
            CurrentHP += amount;

        if (CurrentHP >= MaxHP && !poisoning)
        {
            CurrentHP = MaxHP;
            SetHealthy(true);
            spriterender.color = new Color(0,0,0);
        }
        else if (CurrentHP >= MaxHP)
        {
            CurrentHP = MaxHP;
        }
        else if (CurrentHP < MaxHP)
        {
            SetHealthy(false);
        }
    }


    IEnumerator Appear()
    {
        for (int n = 0; n < 20; ++n)
        {
            this.transform.position += new Vector3(0,1.57f/20);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
        yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(24 / 60f));
        for (int n = 0; n < 20; ++n)
        {
            this.transform.position -= new Vector3(0, 1.57f / 20);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
        underground = true;
    }

    IEnumerator AppearFixed()
    {
        for (int n = 0; n < 20; ++n)
        {
            this.transform.position += new Vector3(0, 1.57f / 20);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }

    public bool IsHide()
    {
        return !underground;
    }

    IEnumerator AttackedHide()
    {
        for (int n = 0; n < 5; ++n)
        {
            this.transform.position -= new Vector3(0, 1.57f / 5);
            yield return StartCoroutine(GameSceneController.Inst().WaitOnInGame(1 / 60f));
        }
    }
}