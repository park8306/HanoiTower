using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject m_gameObject;

    public delegate void ButtonEvent();

    public ButtonEvent m_buttonEvent;

    Vector3 DEF_SCALE_VALUE;
    float DEF_DURATION = 0.3f;

    Tween m_btnTween;

    private void Start()
    {
        if (m_gameObject == null) m_gameObject = this.gameObject;

        DEF_SCALE_VALUE = m_gameObject.transform.localScale;

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
                    if (m_buttonEvent != null) m_buttonEvent();
                });
            });
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySfxSound(SoundManager.SFX.activeBtn);
        m_gameObject.transform.DOScale(m_gameObject.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f), DEF_DURATION).SetLoops(-1,LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_gameObject.transform.DOKill();
        m_gameObject.transform.DOScale(DEF_SCALE_VALUE, DEF_DURATION);
    }
}
