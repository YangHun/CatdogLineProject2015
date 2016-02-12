using UnityEngine;
using System;
using System.Collections.Generic;

class SoundManager : SingleTonBehaviour<SoundManager>
{

    // info
    private float m_Volume = 1.0f;
    private enum SoundManagerStates
    {
        PLAYING,
        PAUSE
    }
    private StateMachine m_State;

    // sounds
    private List<AudioSource> m_Sounds;
    private AudioSource m_BGM;




    void Start()
    {
        m_State = new StateMachine();
        m_State.AddState(SoundManagerStates.PLAYING, OnStatePlaying);
        m_State.AddState(SoundManagerStates.PAUSE, OnStatePause);
        m_State.SetInitialState(SoundManagerStates.PLAYING);

        m_Sounds = new List<AudioSource>();
        m_BGM = null;
    }


    public void Update() { m_State.Update(); }
    public void LateUpdate() { m_State.LateUpdate(); }


    void OnStatePlaying()
    {
        if (m_State.IsFirstUpdate())
        {
            foreach (var sound in m_Sounds)
            {
                sound.UnPause();
            }
        }
    }

    void OnStatePause()
    {
        if(m_State.IsFirstUpdate())
        {
            foreach(var sound in m_Sounds)
            {
                sound.Pause();
            }
        }
    }



    /// <summary>
    /// add new sound
    /// </summary>
    /// <param name="source"> source of sound </param>
    /// <param name="playnew"> override current sound with same source </param>
    public void AddSound(AudioSource source, bool playnew = true)
    {
        if (source == null)
            return;

        bool isSourceAdded = false;

        // source already playing
        if (m_Sounds.Contains(source) && playnew)
        {
            source.Stop();
            source.volume = m_Volume;
            source.Play();

            isSourceAdded = true;
        }
        else
        {
            // play new sound
            m_Sounds.Add(source);
            source.volume = m_Volume;
            m_BGM.Play();

            isSourceAdded = true;
        }

        // pause if current state is pause
        if (isSourceAdded && m_State.IsCurrentState(SoundManagerStates.PAUSE))
        {
            source.Pause();
        }
    }

    /// <summary>
    /// set bgm as new BGM
    /// stop BGM if bgm == null
    /// </summary>
    /// <param name="bgm"> new BGM </param>
    public void SetBGM(AudioSource bgm)
    {
        // stop current bgm
        if (m_BGM != null)
            m_BGM.Stop();

        // play new bgm
        m_BGM = bgm;
        if (m_BGM != null)
        {
            m_BGM.volume = m_Volume;
            m_BGM.Play();
        }

    }

    /// <summary>
    /// get current volume
    /// </summary>
    /// <returns> range [0, 1]</returns>
    public float GetVolume()
    {
        return m_Volume;
    }

    /// <summary>
    /// set current volume
    /// even change volume on playing
    /// </summary>
    /// <param name="volume"> range [0, 1] </param>
    public void SetVolume(float volume)
    {
        // set volume parameter
        if (volume >= 1.0f)
            m_Volume = 1.0f;
        else if (volume <= 0.0f)
            m_Volume = 0.0f;
        else
            m_Volume = volume;


        // change volume of sound
        foreach(var sound in m_Sounds)
        {
            sound.volume = m_Volume;
        }

        if (m_BGM != null)
            m_BGM.volume = m_Volume;

    }

    /// <summary>
    /// pause all playing sounds
    /// does not pause bgm
    /// </summary>
    public void PauseSounds()
    {
        m_State.ChangeState(SoundManagerStates.PAUSE);
    }

    /// <summary>
    /// resume all playing sounds
    /// </summary>
    public void ResumeSounds()
    {
        m_State.ChangeState(SoundManagerStates.PLAYING);
    }

    /// <summary>
    /// stop all playing sounds
    /// does not stop bgm
    /// </summary>
    public void StopSounds()
    {
        // stop all sound
        foreach(var sound in m_Sounds)
        {
            sound.Stop();
        }

        // clear list
        m_Sounds.Clear();

        // change state
        m_State.ChangeState(SoundManagerStates.PLAYING);
    }
}
