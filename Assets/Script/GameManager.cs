using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameScene
    {
        Title,
        InGame,
    }

    [SerializeField] private GameObject[] m_arrBg;
    [SerializeField] private GameObject[] m_arrScene;

    [HideInInspector] public bool m_isBgMove = false;

    public float m_fBgSpeed = 1f;

    public float timeScale = 1f;

    void Start()
    {
        ShowScene(GameScene.Title);
    }

    private void Update()
    {
        Time.timeScale = timeScale;

        if (m_isBgMove)
        {
            for (int i = 0; i < m_arrBg.Length; i++)
            {
                m_arrBg[i].transform.Translate(new Vector3(-1f , 0f, 0f)* m_fBgSpeed * Time.deltaTime);
                if (m_arrBg[i].transform.localPosition.x <= -1920)
                {
                    m_arrBg[i].transform.localPosition = new Vector3(m_arrBg[i-1 > 0 ? i-1 : m_arrBg.Length-1].transform.localPosition.x + 1920, 0f, 0f);
                }
            }
        }
    }

    public void ShowScene(GameScene _gameScene)
    {
        for (int i = 0; i < m_arrScene.Length; i++)
        {
            m_arrScene[i].SetActive(false);
        }

        m_arrScene[(int)_gameScene].SetActive(true);
    }
}
