using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;
using System.Linq;

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
    [SerializeField] private GameObject m_touchBlock;
    [SerializeField] private Button m_btnReStart;

    [Header("------------- 인게임 -------------")]
    [SerializeField] private GameObject m_inGame;
    [SerializeField] private Image m_inGameTower;
    [SerializeField] private InGameBar[] m_arrInGameBar;
    [SerializeField] private GameObject[] m_arrInGameDisk;

    private List<Sprite> m_liTowerSprite = new List<Sprite>();

    public static InGameBar SelectedInGameBar = null;

    private void Start()
    {
        LoadTowerSprite();

        m_btnCheck.onClick.AddListener(() => { SetBtnCheck(); });

        m_btnReStart.onClick.AddListener(()=> { ActiveChooseLevelObj(true, true); });

        InitDisk();

        SetPlaceholderText();

        SetModeBtn();

        ActiveChooseLevelObj(false);

        m_btnCheck.GetComponent<MouseOver>().InitBtn();
    }

    private void LoadTowerSprite()
    {
        Sprite[] towerSprite0 = Resources.LoadAll<Sprite>("img/InGame/InGame-0");
        Sprite[] towerSprite1 = Resources.LoadAll<Sprite>("img/InGame/InGame-1");

        m_liTowerSprite.Add(towerSprite0[11]);
        m_liTowerSprite.Add(towerSprite1[1]);
        m_liTowerSprite.Add(towerSprite0[12]);
    }

    private void InitDisk()
    {
        List<Transform> inGameBarDisk = new List<Transform>();

        for (int i = 0; i < m_arrInGameDisk.Length; i++)
        {
            // 1번 막대로 전부 위치 옮김
            Transform inGameDisk = m_arrInGameDisk[i].transform;
            inGameDisk.transform.parent = m_arrInGameBar[0].transform;

            inGameBarDisk.Add(inGameDisk);

            // 디스크 강조 점선 비활성화
            m_arrInGameDisk[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        // 디스크 정렬시켜주기
        inGameBarDisk.ForEach(x => x.SetSiblingIndex((x.name[x.name.Length - 1] - '0') - 1));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetInGameBar(m_arrInGameBar[0]);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SetInGameBar(m_arrInGameBar[1]);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SetInGameBar(m_arrInGameBar[2]);
    }

    public void SetInGameBar(InGameBar _inGameBar)
    {
        if (GameManager.Instance.m_inGameState != GameManager.InGameState.Play) return;

        if (SelectedInGameBar == null)
        {
            if (_inGameBar.transform.childCount != 0)
            {
                SelectedInGameBar = _inGameBar;

                GameObject diskDot = SelectedInGameBar.transform.GetChild(0).GetChild(0).gameObject;
                diskDot.SetActive(true);
            }
            else return;
        }
        else
        {
            if (SelectedInGameBar == _inGameBar) return;
            else
            {
                MoveDisk(_inGameBar);
            }
        }
    }
    private void MoveDisk(InGameBar _inGameBar)
    {
        SoundManager.Instance.PlaySfxSound(Random.Range(0, 2) == 0 ? SoundManager.SFX.move1 : SoundManager.SFX.move2);

        Transform disk = SelectedInGameBar.transform.GetChild(0);
        GameObject diskDot = disk.GetChild(0).gameObject;

        diskDot.SetActive(false);
        disk.parent = _inGameBar.transform;
        disk.SetAsFirstSibling();
        SelectedInGameBar = null;
    }

    private void SetModeBtn()
    {
        SetBtnEvent(m_arrBtnMode[0]);
        SetBtnEvent(m_arrBtnMode[1]);
        SetBtnEvent(m_arrBtnMode[2]);
        SetBtnEvent(m_arrBtnMode[3]);
        SetBtnEvent(m_arrBtnMode[4]);
        SetBtnEvent(m_arrBtnMode[5]);
        SetBtnEvent(m_arrBtnMode[6]);
    }

    private void SetBtnEvent(Button _btnMode)
    {
        _btnMode.GetComponent<MouseOver>().m_buttonEvent = () =>
        {
            ActiveChooseLevelObj(false, true, () =>
            {
                SetInGame(_btnMode.name);
            });
        };
        _btnMode.GetComponent<MouseOver>().InitBtn();
    }

    // 인게임 세팅
    private void SetInGame(string _btnName)
    {
        CanvasGroup CGinGame = m_inGame.GetComponent<CanvasGroup>();
        int nDiskCnt = _btnName[_btnName.Length - 1] - '0';

        DOTween.To(() => CGinGame.alpha, x => CGinGame.alpha = x, 0f, 0.5f).OnComplete(()=> 
        {
            InitDisk();

            // 모두 비활성화 후 필요한 갯수만큼 활성화 시켜주기
            ActiveDisk(nDiskCnt);

            // 배경 변경
            ChangeInGameTower(nDiskCnt);

            DOTween.To(() => CGinGame.alpha, x => CGinGame.alpha = x, 1f, 0.5f).OnComplete(()=> { GameManager.Instance.m_inGameState = GameManager.InGameState.Play; });
        });
    }

    private void ChangeInGameTower(int nDiskCnt)
    {
        if (nDiskCnt <= 3)
        {
            m_inGameTower.sprite = m_liTowerSprite[0];
        }
        else if(nDiskCnt <= 6)
        {
            m_inGameTower.sprite = m_liTowerSprite[1];
        }
        else if(nDiskCnt <= 9)
        {
            m_inGameTower.sprite = m_liTowerSprite[2];
        }
        m_inGameTower.SetNativeSize();
    }

    private void ActiveDisk(int nDiskCnt)
    {
        Array.ForEach(m_arrInGameDisk, x => x.SetActive(false));
        for (int i = 0; i < nDiskCnt; i++)
        {
            m_arrInGameDisk[i].SetActive(true);
        }
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
        GameManager.Instance.m_inGameState = GameManager.InGameState.ChooseLevel;
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

        ActiveTouchBlock(_isActive);
    }

    private void ActiveBtnMode(bool _isActive)
    {
        for (int i = 0; i < m_arrBtnMode.Length; i++)
        {
            m_arrBtnMode[i].enabled = _isActive;
            m_arrBtnMode[i].GetComponent<MouseOver>().enabled = _isActive;
        }
    }

    private void ActiveTouchBlock(bool _isActive)
    {
        m_touchBlock.SetActive(_isActive);
    }
}
