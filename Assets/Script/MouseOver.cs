using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject m_gameObject;

    public bool m_isMoveOk = true;

    float DEF_SCALE_VALUE = 1.1f;
    float DEF_DURATION = 0.3f;

    private void Start()
    {
        InitBtn();
    }

    private void InitBtn()
    {
        m_gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySfxSound(SoundManager.SFX.clickBtn);

            this.enabled = false;
            GetComponent<Button>().enabled = false;

            m_gameObject.transform.DOKill();
            m_gameObject.transform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
            {
                m_gameObject.transform.DOPunchScale(Vector3.one * 0.1f, DEF_DURATION, 5).OnComplete(() =>
                {
                    GameManager.Instance.ShowScene(GameManager.GameScene.InGame);
                });
            });
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySfxSound(SoundManager.SFX.activeBtn);
        m_gameObject.transform.DOScale(new Vector3(DEF_SCALE_VALUE, DEF_SCALE_VALUE, DEF_SCALE_VALUE), DEF_DURATION).SetLoops(-1,LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_gameObject.transform.DOKill();
        m_gameObject.transform.DOScale(Vector3.one, DEF_DURATION);
    }
}
