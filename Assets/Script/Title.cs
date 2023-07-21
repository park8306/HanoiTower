using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Title : MonoBehaviour
{

    [Header("------ 디스크 ------")]
    [SerializeField] private GameObject[] m_arrDisk;

    [Header("------ 타이틀 ------")]
    [SerializeField] private Transform m_titleHanoi;
    [SerializeField] private Transform m_startBtn;

    void Start()
    {
        InitTitle();

        AnimDisk();
    }

    private void InitTitle()
    {
        m_titleHanoi.localScale = new Vector2(0, 0);
        m_startBtn.localScale = new Vector2(0, 0);

        Button btnStart = m_startBtn.GetComponent<Button>();
        btnStart.enabled = false;
        btnStart.GetComponent<MouseOver>().enabled = false;

        int nDiskInitPosY = 900;

        for (int i = 0; i < m_arrDisk.Length; i++)
        {
            m_arrDisk[i].transform.localPosition = new Vector3(0, nDiskInitPosY, 0);
        }
    }

    private void AnimDisk()
    {
        float fDiskDistance = 67;
        float fAnimDuration = 1.5f;
        float fAnimDelay = 0.1f;

        float fTitleAnimDuration = 0.5f;

        for (int i = 0; i < m_arrDisk.Length; i++)
        {
            Tween TweenDisk = m_arrDisk[i].transform.DOLocalMoveY(fDiskDistance * i, fAnimDuration).SetEase(Ease.InBack).SetDelay(fAnimDelay * i);

            if (i == m_arrDisk.Length - 1)
            {
                TweenDisk.OnComplete(() =>
                {
                    SoundManager.Instance.PlaySfxSound(Random.Range(0, 2) == 0 ? SoundManager.SFX.move1 : SoundManager.SFX.move2);
                    SoundManager.Instance.PlayBGM(0.3f);

                    m_titleHanoi.DOScale(new Vector3(1f, 1f, 1f), fTitleAnimDuration).SetEase(Ease.OutBack);
                    m_startBtn.DOScale(new Vector3(1f, 1f, 1f), fTitleAnimDuration).SetEase(Ease.OutBack).OnComplete(()=> { m_startBtn.GetComponent<Button>().enabled = true; m_startBtn.GetComponent<MouseOver>().enabled = true; });

                    GameManager.Instance.m_isBgMove = true;
                });
            }
            else TweenDisk.OnComplete(() => { SoundManager.Instance.PlaySfxSound(Random.Range(0,2) == 0 ? SoundManager.SFX.move1 : SoundManager.SFX.move2); });
        }
    }
}
