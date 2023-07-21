using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : Singleton<SoundManager>
{
    public enum SFX
    {
        activeBtn,
        clickBtn,
        closeBtn,
        move1,
        move2,
        cantMove,
        finishiResult
    }

    [SerializeField] private AudioClip[] m_arrSoundSfx;

    [SerializeField] private AudioSource m_sfx;
    [SerializeField] private AudioSource m_bgm;

    public void PlaySfxSound(SFX _eSFX, float _volume = 1.0f)
    {
        m_sfx.clip = m_arrSoundSfx[(int)_eSFX];
        m_sfx.volume = _volume;
        m_sfx.PlayOneShot(m_arrSoundSfx[(int)_eSFX]);
    }

    public void PlayBGM(float _volume = 1.0f)
    {
        m_bgm.volume = _volume;
        m_bgm.Play();
    }
}
