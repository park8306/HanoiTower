using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class InGameSystem : MonoBehaviour
{
    [Header("------------- 이름 입력 -------------")]
    [SerializeField] private InputField m_writeInputField;
    [SerializeField] private Button m_btnCheck;

    private string m_userName;
    private string[] m_arrRandName = new string[] {"지렁이", "쿵야", "철형", "훈형", "솔형" };

    [Header("------------- 난이도 선택 -------------")]
    [SerializeField] private GameObject m_chooseLevel;
    [SerializeField] private Button[] m_arrBtnMode;

    private void Start()
    {
        m_btnCheck.onClick.AddListener(() => { SetBtnCheck(); });

        SetPlaceholderText();

        SetModeBtn();

        ActiveChooseLevelObj(false);
    }

    private void SetModeBtn()
    {
        for (int i = 0; i < m_arrBtnMode.Length; i++)
        {
            m_arrBtnMode[i].GetComponent<MouseOver>().m_buttonEvent = () => 
            {
                Debug.Log("버튼 클릭");
                ActiveChooseLevelObj(false, true, ()=> { Debug.Log(m_arrBtnMode[i].name); SetInGame(m_arrBtnMode[i].name); });
            };
        }
    }

    private void SetInGame(string _btnName)
    {
        throw new NotImplementedException();
    }

    private void SetBtnCheck()
    {
        string userName = m_writeInputField.text;

        if (userName != null || userName != string.Empty) m_userName = userName;
        else m_userName = GetPlaceholderText();

        m_writeInputField.gameObject.SetActive(false);

        GameObject GOwriteName = m_writeInputField.transform.parent.gameObject;
        CanvasGroup CGwriteName = m_writeInputField.transform.parent.GetComponent<CanvasGroup>();

        DOTween.To(() => CGwriteName.alpha, x => CGwriteName.alpha = x, 0, 0.5f).OnComplete(()=> 
        { 
            GOwriteName.SetActive(false);

            ActiveChooseLevelObj(true, true);
        });
    }

    private void SetPlaceholderText()
    {
        m_writeInputField.placeholder.GetComponent<Text>().text = m_arrRandName[Random.Range(0, m_arrRandName.Length)];
    }

    private string GetPlaceholderText()
    {
        return m_writeInputField.placeholder.GetComponent<Text>().text;
    }

    private void ActiveChooseLevelObj(bool _isActive, bool _isTween = false, Action _onComplete = null)
    {
        if (_isTween)
        {
            CanvasGroup CGchooseLevel = m_chooseLevel.GetComponent<CanvasGroup>();
            ActiveBtnMode(false);

            if (_isActive)
            {
                CGchooseLevel.alpha = 0;

                m_chooseLevel.SetActive(_isActive);

                DOTween.To(() => CGchooseLevel.alpha, x => CGchooseLevel.alpha = x, 1, 0.5f).OnComplete(()=> { ActiveBtnMode(true); });
            }
            else
            {
                CGchooseLevel.alpha = 1;

                DOTween.To(() => CGchooseLevel.alpha, x => CGchooseLevel.alpha = x, 0, 0.5f).OnComplete(()=> 
                {
                    m_chooseLevel.SetActive(_isActive);

                    if (_onComplete != null) _onComplete();
                });
            }
        }
        else
        {
            m_chooseLevel.SetActive(_isActive);
            ActiveBtnMode(_isActive);
        }
    }

    private void ActiveBtnMode(bool _isActive)
    {
        for (int i = 0; i < m_arrBtnMode.Length; i++)
        {
            m_arrBtnMode[i].enabled = _isActive;
        }
    }
}
